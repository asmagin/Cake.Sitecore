param(
    [string]$url, 
    [string]$secret, 
    [string]$scriptDir, 
    [string[]]$configurations = "",
    [string]$basicAuth = ""
    )
$ErrorActionPreference = 'Stop'

# This is an example PowerShell script that will remotely execute a Unicorn sync using the new CHAP authentication system.

Import-Module "$scriptDir/Unicorn.psm1"

Sync-Unicorn -ControlPanelUrl $url -SharedSecret $secret -Configurations $configurations -basicAuth $basicAuth

# Note: you may pass -Verb 'Reserialize' for remote reserialize. Usually not needed though.