Philosophies of a Jabber.Net development
==========================================

Sure, a lot of these are going to be Mom&ApplePie, but let's get them
down.

-   Scale - scale out first, then scale up
-   Latency matters - don't automatically trade latency for scalability
    or fail-over
-   Managed - Reuse the existing WinNT/Win2k/WinXP management
    infrastructure.  Target typical MCSE's as server admins.  Don't
    require hand-editing XML files for standard configurations.
-   Reused - don't reinvent the wheel until there is a *proven* reason
    to do so (e.g. hard performance data pointing to a hot spot). 
    Prefer less lines of code.
-   Enabled - allow client and server module developers to hook
    functionality in the VS.Net IDE.
-   Thread-safe - this thing is going to be maximum-async, so watch for
    correct locking.
-   Compatible - maintain **all** of the client-to-CCM protocol, and
    create a superset of the server-to-module protocol.  Try to leverage
    existing modules, but no need to maintain existing configuration
    file formats, etc.  As long as there exists a way to hook in
    existing base-accept and base-connect modules, we'll call that
    good enough.
-   Portable - heh.  Don't use stuff from the win32 namespace without a
    wrapper.  Assume that there will be a .Net runtime on non-MS
    platforms someday.
-   Abstract - there are 20+ ways to implement queues.  Write an
    interface, and defer the implementation details until later.
