Jabber-Net [![NuGet][nuget-badge]][nuget] [![Appveyor build][appveyor-badge]][appveyor] [![Travis build][travis-badge]][travis]
==========

Jabber-Net is a set of .NET classes for sending and receiving Extensible
Messaging and Presence Protocol (XMPP), also known as the Jabber. Client
connections, server component connections, presence, service discovery, and the
like.

Build and test
--------------

Either use Visual Studio 2015 on Windows or `nuget` + `msbuild` / `xbuild` in
your terminal. On Windows:

```console
> nuget restore jabber-net.sln
> msbuild jabber-net.sln /p:Configuration=Debug
> .\packages\NUnit.ConsoleRunner.3.4.1\tools\nunit3-console.exe .\test\bin5\Debug\test.dll
```

On Linux:

```console
$ nuget restore jabber-net.sln
$ xbuild /p:Configuration=Debug jabber-net.sln
$ mono ./packages/NUnit.ConsoleRunner.3.4.1/tools/nunit3-console.exe ./test/bin5/Debug/test.dll
```

Documentation
-------------

The documentation is placed in the `docs` directory. To build HTML
documentation, invoke the following commands (PowerShell syntax):

```powershell
$fsi = 'C:\Program Files (x86)\Microsoft SDKs\F#\4.0\Framework\v4.0\Fsi.exe'
Remove-Item -Recurse .\docs\output
& $fsi docs\generate.fsx
docs\output\index.html
```

Packaging
---------

To build [NuGet][nuget] package for Jabber.Net, use the script
`Scripts/Package.ps1`. If you want to push this package to the Nuget feed, use
`Scripts/Push-Package.ps1`.

Consult the scripts documentation to get the information about their parameters.

Licensing
---------

The project source code is generally licensed under the terms of LGPLv3. Please
consult [Licensing.md][] for details on licensing of internal components.

[Licensing.md]: ./Licensing.md

[appveyor]: https://ci.appveyor.com/project/ForNeVeR/jabber-net/branch/develop
[nuget]: https://www.nuget.org/packages/jabber-net/
[travis]: https://travis-ci.org/ForNeVeR/jabber-net

[appveyor-badge]: https://ci.appveyor.com/api/projects/status/9q5rgknk80oh5g3a/branch/develop?svg=true
[nuget-badge]: https://img.shields.io/nuget/v/jabber-net.svg?maxAge=2592000
[travis-badge]: https://travis-ci.org/ForNeVeR/Jabber-Net.svg?branch=develop
