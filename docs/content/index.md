Jabber-Net Library
===================

What Is Jabber-Net?
-------------------

Jabber-Net is a set of libraries for accessing Jabber functionality from .Net.
It is written in C#, but is accessible from other .NET languages such as F# and
VB.NET. As you explore, you'll find there are some other goodies buried inside,
like Trees, CommandLine processing, etc.

How to install
--------------

Install the library from NuGet:

    Install-Package jabber-net

Philosophy
----------

Here're the main qualities valued across Jabber-Net development:

-   Scale - scale out first, then scale up
-   Latency matters - don't automatically trade latency for scalability
    or fail-over
-   Reused - don't reinvent the wheel until there is a *proven* reason
    to do so (e.g. hard performance data pointing to a hot spot). 
    Prefer less lines of code.
-   Enabled - allow client and server module developers to hook functionality in
    their IDE.
-   Thread-safe - this thing is going to be maximum-async, so watch for
    correct locking.
-   Compatible - maintain **all** of the client-to-CCM protocol, and
    create a superset of the server-to-module protocol.  Try to leverage
    existing modules, but no need to maintain existing configuration
    file formats, etc.  As long as there exists a way to hook in
    existing base-accept and base-connect modules, we'll call that
    good enough.
-   Portable - keep platform-dependent code at the minimum level; the library
    should be compatible for full .NET, .NET Core and Mono developers.
-   Abstract - there are 20+ ways to implement queues.  Write an
    interface, and defer the implementation details until later.
