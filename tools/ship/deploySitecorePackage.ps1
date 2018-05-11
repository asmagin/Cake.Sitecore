param
(
  [parameter(Position = 0, Mandatory = $true)]
  [string]$Url,
  [parameter(Position = 1, Mandatory = $true)]
  [string]$FilePath,
  [parameter(Mandatory = $false, HelpMessage = "Duration to wait in seconds before timing out the request to Sitecore.Ship.")]
  [int]$Timeout = 600
)

function Get-EncodedDataFromFile() {
  param
  (
    [System.IO.FileInfo]$file = $null
  )
  process {
    $data = $null;
    $codePageName = "iso-8859-1";
 
    if ($file -and [System.IO.File]::Exists($file.FullName)) {
      $bytes = [System.IO.File]::ReadAllBytes($file.FullName);
      if ($bytes) {
        $enc = [System.Text.Encoding]::GetEncoding($codePageName);
        $data = $enc.GetString($bytes);
      }
    }
    else {
      Write-Host "ERROR; File '$file' does not exist";
    }
    $data;
  }
}

function Get-BoundaryId {
  $uniqueId = [System.Guid]::NewGuid().ToString().Replace("-", "")
  return "----FormBoundary{0}" -f $uniqueId
}

function Get-FormData([string[]]$dataArray, [string]$boundaryId) {
  $header = "--{0}" -f $boundaryId
  $footer = "--{0}--" -f $boundaryId
    
  [System.Text.StringBuilder]$contents = New-Object System.Text.StringBuilder
  if ($dataArray.Count -gt 0) {
    foreach ($data in $dataArray) {
      [void]$contents.AppendLine($header)
      [void]$contents.AppendLine($data)
    }
    [void]$contents.AppendLine($footer)
  }

  return $contents.ToString()
}

function Get-LocalTime([object]$time) {
  if ($time -is [PSObject] -and ($time.psobject.Properties | where { $_.Name -eq "date"})) {
    return $time.date.ToLocalTime()
  }

  return $time.ToLocalTime()
}

$Timeout = $Timeout * 1000
if ($FileUpload -and !(Test-Path $FilePath)) {
  throw [System.IO.FileNotFoundException] "$FilePath not found."
}

$servicePath = "/services/package/install/fileupload"
$serviceUrl = "{0}/{1}" -f $Url, $servicePath

# Generate Form Data
[System.Text.StringBuilder]$inputTextBuilder = New-Object System.Text.StringBuilder
$textArray = @()

# Create a filestream and encode it as text
[System.IO.FileInfo]$file = (Get-Item -Path $FilePath)
$filedata = Get-EncodedDataFromFile -file $file

[void]$inputTextBuilder.AppendLine("Content-Disposition: form-data; name=`"path`"; filename=`"{0}`"" -f (Split-Path $FilePath -Leaf))
[void]$inputTextBuilder.AppendLine("Content-Type: application/octet-stream")
[void]$inputTextBuilder.AppendLine("")
[void]$inputTextBuilder.AppendLine($filedata)
$textArray += $inputTextBuilder.ToString()

[void]$inputTextBuilder.Clear()
[void]$inputTextBuilder.AppendLine("Content-Disposition: form-data; name=`"PackageId`"")
[void]$inputTextBuilder.AppendLine("")
[void]$inputTextBuilder.Append("00")
$textArray += $inputTextBuilder.ToString()

[void]$inputTextBuilder.Clear()
[void]$inputTextBuilder.AppendLine("Content-Disposition: form-data; name=`"Description`"")
[void]$inputTextBuilder.AppendLine("")
[void]$inputTextBuilder.Append($FilePath)
$textArray += $inputTextBuilder.ToString()
  
$hash = Get-FileHash $FilePath

[void]$inputTextBuilder.Clear()
[void]$inputTextBuilder.AppendLine("Content-Disposition: form-data; name=`"Hash`"")
[void]$inputTextBuilder.AppendLine("")
[void]$inputTextBuilder.Append($hash)
$textArray += $inputTextBuilder.ToString()


$boundaryId = Get-BoundaryId
$bodyText = [byte[]][char[]](Get-FormData $textArray $boundaryId)

# Form web request
$request = [System.Net.HttpWebRequest]::CreateHttp($serviceUrl)
$request.Method = 'POST'
$request.Timeout = $Timeout
$request.Accept = "application/json, text/javascript, */*"
$request.KeepAlive = $true
$request.ContentType = "multipart/form-data; boundary={0}" -f $boundaryId
$request.Headers.Add("Accept-Encoding", "gzip,deflate")
$request.Headers.Add("Accept-Language", "en-US,en;q=0.8")
$requestStream = $request.GetRequestStream()
$requestStream.Write($bodyText, 0, $bodyText.Length)

# Get response
$response = $request.GetResponse()
$requestStream = $response.GetResponseStream()
$readStream = New-Object System.IO.StreamReader $requestStream
$data = $readStream.ReadToEnd()

$response.Close()
$response.Dispose()

$requestStream.Close()
$requestStream.Dispose()

$readStream.Close()
$readStream.Dispose()

$results = $data | ConvertFrom-Json
return $results
