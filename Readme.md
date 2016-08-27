Jabber-Net [![NuGet][nuget-badge]][nuget] [![Appveyor build][appveyor-badge]][appveyor]
==========

Jabber-Net is a set of .NET classes for sending and receiving Extensible
Messaging and Presence Protocol (XMPP), also known as the Jabber. Client
connections, server component connections, presence, service discovery, and the
like.

Packaging
---------

To build [Nuget][nuget] package for Jabber.Net, use the script
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

[appveyor-badge]: https://ci.appveyor.com/api/projects/status/fpe2djtjucsl89x3/branch/develop?svg=true
[nuget-badge]: https://img.shields.io/nuget/v/jabber-net.svg?maxAge=2592000
