<#
.SYNOPSIS
    Build the project and pack it as a NuGet package.
.PARAMETER msbuild
    Path to the MSBuild executable.
.PARAMETER nuget
    Path to the Nuget executable.
.PARAMETER Solution
    Path to the solution that will be built.
.PARAMETER Project
    Path to the main project that will be packed.
.PARAMETER Configuration
    Solutuion configuration that will be built and packed.
#>
param (
    $msbuild = 'msbuild',
    $nuget = 'nuget',
    $Solution = "$PSScriptRoot/../JabberNet.sln",
    $Project = "$PSScriptRoot/../JabberNet.csproj",
    $Configuration = "Release"
)

$ErrorActionPreference = 'Stop'

& $msbuild $Solution /m "/p:Configuration=$Configuration" /p:Platform="Any CPU"
if (-not $?) {
    exit $LASTEXITCODE
}

& $nuget `
    pack `
    $Project `
    -IncludeReferencedProjects `
    -Prop "Configuration=$Configuration" `
    -Prop "OutDir=bin\$Configuration"

exit $LASTEXITCODE
