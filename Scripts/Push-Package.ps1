<#
.SYNOPSIS
    Pushed the package to the NuGet feed.
.PARAMETER ApiKey
    Nuget API key.
.PARAMETER nuget
    Path to the NuGet executable.
.PARAMETER Url
    URL of the NuGet feed.
.PARAMETER Package
    Path to the package file that will be published.
#>
param (
    [Parameter(Mandatory=$true)] [string]$ApiKey,
    [string]$nuget = "nuget",
    [string]$Url = 'https://www.nuget.org',
    [string]$Package = "$PSScriptRoot/../*.nupkg"
)

$ErrorActionPreference = 'Stop'

& $nuget setApiKey $ApiKey
& $nuget push (Resolve-Path $Package)
