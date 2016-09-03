How come I get a certificate error?
===================================

In Mono, you get this error:

```
ERROR: System.IO.IOException: The authentication or decryption has failed. ---> Mono.Security.Protocol.Tls.TlsException: Invalid certificate received form server.
```

This means that your server has an SSL/TLS certificate that is not signed by a
Certificate Authority (CA) that your system trusts. You can either fix the
certificate problem (preferred), or set:

```csharp
bedrock.net.AsyncSocket.UntrustedRootOK = true;
```

`UntrustedRootOK` is now obsolete in the main development branch. If you want to
handle the error with your own code, please handle `OnIvalidCertificate` event.
