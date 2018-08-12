Preparing a Docker environment for manual testing
=================================================

Sometimes, to reproduce a user test case, you'll want to set up your own XMPP
server. This instruction will help you.

To install an XMPP server, use Docker and the following command:

```console
$ docker run --name openfire --publish 9090:9090 --publish 5222:5222 --publish 7777:7777 --publish 7070:7070 sameersbn/openfire:3.10.3-19
```

After that, visit http://localhost:9090 with your browser and set up the
initial configuration.

By default, your server will be accessible at `localhost:5222`.
