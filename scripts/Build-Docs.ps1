param (
    $fsi = 'C:\Program Files (x86)\Microsoft SDKs\F#\4.1\Framework\v4.0\Fsi.exe',
    $Root = 'https://fornever.github.io/Jabber-Net',
    $Output = "$PSScriptRoot/../../Jabber-Net_docs"
)

$localOutput = "$PSScriptRoot/../docs/output"
$env:JABBER_NET_ROOT = $Root
Remove-Item -Recurse $localOutput -ErrorAction SilentlyContinue
& $fsi docs\generate.fsx

Get-ChildItem $Output -Exclude '.git' | Remove-Item -Recurse
Copy-Item -Recurse -Force "$localOutput/*" $Output
