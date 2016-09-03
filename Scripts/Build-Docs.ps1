param (
    $fsi = 'C:\Program Files (x86)\Microsoft SDKs\F#\4.0\Framework\v4.0\Fsi.exe',
    $Root = 'https://fornever.github.io/Jabber-Net',
    $Output = "$PSScriptRoot/../../Jabber-Net_docs"
)

$localOutput = "$PSScriptRoot/../docs/output"
$env:JABBER_NET_ROOT = $Root
Remove-Item -Recurse $localOutput
& $fsi docs\generate.fsx
Copy-Item -Recurse -Force "$localOutput/*" $Output
