$ErrorActionPreference = 'Stop'
$ScriptPath = Split-Path $MyInvocation.MyCommand.Path
$MicroCHAP = $ScriptPath + '\MicroCHAP.dll'
Add-Type -Path $MicroCHAP

Function Sync-Unicorn {
	Param(
		[Parameter(Mandatory=$True)]
		[string]$ControlPanelUrl,

		[Parameter(Mandatory=$True)]
		[string]$SharedSecret,

		[string[]]$Configurations,

		[string]$basicAuth,

		[string]$Verb = 'Sync'
	)

	# PARSE THE URL TO REQUEST
	$parsedConfigurations = '' # blank/default = all
	
	if($Configurations) {
		$parsedConfigurations = ($Configurations) -join "^"
	}

	$url = "{0}?verb={1}&configuration={2}" -f $ControlPanelUrl, $Verb, $parsedConfigurations

	Write-Host "Sync-Unicorn: Preparing authorization for $url"

	# GET AN AUTH CHALLENGE
	$challenge = Get-Challenge -ControlPanelUrl $ControlPanelUrl -basicAuth $basicAuth

	Write-Host "Sync-Unicorn: Received challenge: $($challenge.Replace($challenge,"*"*$challenge.Length))"

	# CREATE A SIGNATURE WITH THE SHARED SECRET AND CHALLENGE
	$signatureService = New-Object MicroCHAP.SignatureService -ArgumentList $SharedSecret

	$signature = $signatureService.CreateSignature($challenge, $url, $null)

	Write-Host "Sync-Unicorn: Created signature $($signature.Replace($signature,"*"*$signature.Length)), executing $Verb..."

	# USING THE SIGNATURE, EXECUTE UNICORN
	if ([string]::IsNullOrEmpty($basicAuth)) {
		$Headers = @{ "X-MC-MAC" = $signature; "X-MC-Nonce" = $challenge }
	}
	else {
		$encodedCreds = [System.Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes($basicAuth))
		$Headers = @{ Authorization = "Basic $encodedCreds"; "X-MC-MAC" = $signature; "X-MC-Nonce" = $challenge}
	}
	
	$result = Invoke-WebRequest -Uri $url -Headers $Headers -TimeoutSec 10800 -UseBasicParsing

	$result.Content
}

Function Get-Challenge {
	Param(
		[Parameter(Mandatory=$True)]
		[string]$ControlPanelUrl,

		[Parameter(Mandatory=$False)]
		[string]$basicAuth = ""
	)

	$url = "$($ControlPanelUrl)?verb=Challenge"

	if ([string]::IsNullOrEmpty($basicAuth)) {
		$result = Invoke-WebRequest -Uri $url -TimeoutSec 360 -UseBasicParsing
	}
	else {
		$encodedCreds = [System.Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes($basicAuth))
		$Headers = @{ Authorization = "Basic $encodedCreds" }
		
		$result = Invoke-WebRequest -Uri $url -TimeoutSec 360 -UseBasicParsing -Headers $Headers
	}

	$result.Content
}

Export-ModuleMember -Function Sync-Unicorn