<#
.SYNOPSIS
    Pushed the package to the Nuget feed.
.PARAMETER ApiKey
    Nuget API key.
.PARAMETER paket
    Path to the Paket executable.
.PARAMETER Url
    URL of the Nuget feed.
.PARAMETER Package
    Path to the package file that will be published.
#>
param (
    [Parameter(Mandatory=$true)] [string]$ApiKey,
    [string]$paket = "$PSScriptRoot/../.paket/paket.exe",
    [string]$Url = 'https://www.nuget.org',
    [string]$Package = "$PSScriptRoot/../*.nupkg"
)

& $paket push url $Url file (Resolve-Path $Package) apikey $ApiKey
