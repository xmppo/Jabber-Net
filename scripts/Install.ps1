<#
.SYNOPSIS
    Installs the dependencies for the project build.
.PARAMETER Directory
    A directory where Paket binary should be downloaded.
.PARAMETER PaketVersion
    A version of Paket bootstrapper binary to download.
.PARAMETER PaketBootstrapperSha256Hash
    A SHA-256 hash of Paket bootstrapper binary.
#>
param (
    [string] $Directory = "$PSScriptRoot/../.paket",
    [string] $PaketVersion = '3.18.2',
    [string] $PaketBootstrapperSha256Hash = 'D5D600B53B6621CC40299E8B2FB7C20BDAB7DBD632E1B9D87BF274C93DCF0685'
)

$ErrorActionPreference = 'Stop'

$url = "https://github.com/fsprojects/Paket/releases/download/$PaketVersion/paket.bootstrapper.exe"

if (-not (Test-Path -PathType Container $Directory)) {
    Write-Output "Creating .paket directory"
    New-Item -Type Directory $Directory
}

$bootstrapper = "$Directory/paket.bootstrapper.exe"
if (-not (Test-Path $bootstrapper)) {
    Write-Output "Downloading paket bootstrapper"
    Invoke-WebRequest $url -OutFile $bootstrapper
    $hash = (Get-FileHash $bootstrapper -Algorithm SHA256).Hash
    if ($hash -ne $PaketBootstrapperSha256Hash) {
        Remove-Item $bootstrapper
        throw "Invalid Paket bootstrapper hash. Expected $PaketBootstrapperSha256Hash, got $hash"
    }
}

if (-not $?) {
    exit -1
}

$paket = "$Directory/paket.exe"
if (-not (Test-Path $paket)) {
    Write-Output "Running paket bootstrapper"
    & $bootstrapper $PaketVersion
}

Write-Output "Running paket restore"
& $paket restore -v

exit -not $?
