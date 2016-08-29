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

Packaging
---------

To build [NuGet][nuget] package for Jabber.Net, use the script
`Scripts/Package.ps1`. If you want to push this package to the Nuget feed, use
`Scripts/Push-Package.ps1`.

Consult the scripts documentation to get the information about their parameters.

Licensing
---------

-   Copyright © 2002-2008 Cursive Systems, Inc.
-   All the code changes from 2008 are copyrighted by the corresponding
    contributors.

Jabber-Net can be used under the GNU Lesser General Public License (LGPL),
version 3. Please consult `LICENSE.txt` for details.

[appveyor]: https://ci.appveyor.com/project/ForNeVeR/jabber-net/branch/develop
[nuget]: https://www.nuget.org/packages/jabber-net/
[travis]: https://travis-ci.org/ForNeVeR/jabber-net

[appveyor-badge]: https://ci.appveyor.com/api/projects/status/9q5rgknk80oh5g3a/branch/develop?svg=true
[nuget-badge]: https://img.shields.io/nuget/v/jabber-net.svg?maxAge=2592000
[travis-badge]: https://travis-ci.org/ForNeVeR/jabber-net.svg?branch=develop
