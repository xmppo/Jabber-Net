param (
    $Directory = '.'
)

$extensions = @(
    ''
    '.bat'
    '.build'
    '.config'
    '.cs'
    '.csproj'
    '.editorconfig'
    '.gitignore'
    '.md'
    '.nuspec'
    '.pl'
    '.proj'
    '.ps1'
    '.resx'
    '.sln'
    '.targets'
    '.txt'
    '.vb'
    '.vbproj'
    '.xml'
    '.yml'
)

Get-ChildItem $Directory -Recurse | ? { !$_.PSIsContainer -and $extensions -contains $_.Extension } | % {
    $path = $_.FullName
    $content = [IO.File]::ReadAllText($path) -replace "`r`n", "`n"
    [IO.File]::WriteAllText($path, $content)
}
