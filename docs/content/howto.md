Jabber.Net
==========

A set of libraries for connecting to [Jabber](http://www.jabber.org/) using
[.Net](http://msdn.microsoft.com/net) technology.

**Note:**

No, there are no current plans for a server implementation. If you are
interested in leading a sub-project to do that, drop me a line.

**WHAT IS JABBER-NET?**

Jabber-Net is a set of libraries for accessing Jabber functionality from .Net.
It is written in C#, but should be accessible from other .Net languages such as
VB.Net. Components exist for connecting to a Jabber server either as a client or
as a component. As you explore, you'll find there are some other goodies buried
inside, like Trees, CommandLine processing, etc.

If you wanted to use JabberCOM from .Net, but had difficulties, you should try
Jabber-Net instead.

**HOW DO I USE JABBER-NET?**

-   Create a new windows app in VS.Net.
-   Right-click in toolbox, select Customize Toolbox
-   Select .the .Net Framework Components tab
-   Click browse, and select jabber-net.dll, prefereably in the bin/Debug
    directory, where it gets built
-   Do the same for muzzle.dll (UI elements) if you want
-   Click OK, to add a the components
-   Drop a JabberClient component on your form
-   Set connection parameters in the property box
-   Call jabberClient1.Connect() in the Form.OnLoad event handler
-   Go to events in the property box (the lightning bolt), double-click
    OnMessage
-   Write code to handle Message's, like this:

```csharp
private void jabberClient1_OnMessage(object sender, Message msg)
{
  jabber.protocol.client.Message reply = new jabber.protocol.client.Message(jabberClient1.Document);
  reply.Body = "Hello!";
  reply.To = msg.From;
  jabberClient1.Write(reply);
}
```

Note that packet types such as Message are sub-classes of XmlElement with
easy-to-use getters and setters.

**HOW DO I MODIFY THE ROSTER?**

Just drop a jabber.client.RosterManager on the same form as your
jabber.client.JabberClient (you \*have\* followed the directions in this file to
get them into your toolbox, right?) Set the Client property on the RosterManager
to your JabberClient instance, and you're up and running.

RosterManager is currently more for local caching of your roster, and not for
setting things. That, unfortunately, has to be done relatively manually for now.
I'd suggest something like this:

```csharp
// <iq id="jcl_7" type="set">
//   <query xmlns="jabber:iq:roster">
//     <item jid="cnn@rss.rifetech.com" name="cnn rss">
//       <group>RSS</group>
//     </item>
//   </query>
// </iq>

jabber.protocol.iq.RosterIQ riq = new jabber.protocol.iq.RosterIQ(jabberClient1.Document);
riq.Type = jabber.protocol.client.IQType.set;
jabber.protocol.iq.Roster r =  (jabber.protocol.iq.Roster) riq.Query;
jabber.protocol.iq.Item i = r.AddItem();
i.JID = "cnn@rss.rifetech.com";
i.Nickname = "cnn rss";
i.AddGroup("RSS");
jabberClient1.Write(riq);
```

**HOW DO I ADD MY OWN PACKET TYPES?**

(thanks to Tom Waters for this)

Say you want to use a new packet type like this:

```xml
<!-- get your own list of all your objects... -->
<iq type='get' to='self' id='n0'>
 <query xmlns='your:namespace'/>
</iq>

<!-- reply with list of objects... -->
<iq type='result' to='self' id='n0'>
 <query xmlns='your:namespace'>
  <yourobj key='Object1' other='value1'/>
 </query>
</iq>
```

In order to get the inbound connection to create objects of your class, rather
than just plain XmlElements, you need to create a Factory class, and register it
with your connection object in the OnStreamInit event.


```csharp
private void jabberClient_OnStreamInit(object sender, ElementStream stream)
{
    stream.AddFactory(new your.protocol.Factory());
}
```

For an example of a factory class, see jabber.protocol.iq.Factory.

Next, make sure you implement both of these constructors for your type:

```csharp
namespace your.protocol
{
   public class YourQuery : Element
   {
      public const string YOUR_NS  = "your:namespace";

      public YourQuery(XmlDocument doc) : base("query", YOUR_NS, doc)
      {}

      // this constructor is used by the Factory!
      public YourQuery(string prefix, XmlQualifiedName qname, XmlDocument doc) :
          base(prefix, qname, doc)
      {}
   }


    public class Factory : jabber.protocol.IPacketTypes
   {
      private static QnameType[] s_qnt = new QnameType[]
      {
         new QnameType("query", YourQuery.YOUR_NS, typeof(YourQuery))
         // Add other types here, perhaps sub-elements of query...
      };
      QnameType[] IPacketTypes.Types { get { return s_qnt; } }
   }
}
```

One more note.  Most of the classes you are liable to write should derive from
jabber.protocol.Element. jabber.protocol.Packet is for top-level jabber packets,
like &lt;message/&gt;, &lt;iq/&gt;, and &lt;presence/&gt;.

**HOW DO I REGISTER A NEW USER?**

Set AutoLogin to false. Do something like this:

```csharp
void jabberClient1_OnLoginRequired(object sender)
{
    RegisterIQ riq = new RegisterIQ(jc.Document);
    riq.Type = IQType.get;
    Register r = (Register)riq.Query;
    r.Username = txtUser.Text;
    jabberClient1.Tracker.BeginIQ(riq, new IqCB(GotRegisterGet), null);
}

void GotRegisterGet(object sender, IQ iq, object state)
{
    // TODO: look in iq to see if user already registered, and ensure
    // that we get all of the correct fields filled out.
    RegisterIQ riq = new RegisterIQ(jc.Document);
    Register r = (Register)riq.Query;
    riq.Type = IQType.set;
    r.Username = txtUser.Text;
    r.Password = txtPass.Text;
    jabberClient1.Tracker.BeginIQ(riq, new IqCB(GotRegisterSet), null);
}


void GotRegisterSet(object sender, IQ iq, object state)
{
    if (iq.Type != IQType.error)
    {
       jabberClient1.User = txtUser.Text;
       jabberClient1.Password = txtPass.Text;
       jabberClient1.Login();
    }
}
```

**HOW DO I FIND OUT MORE?**

Included in both the source, subversion, and the binary release is a
[.chm](http://msdn.microsoft.com/library/default.asp?url=/library/en-us/htmlhelp/html/vsconHH1Start.asp)
file, generated with [NDoc](http://ndoc.sourceforge.net/), which fully documents
the API. If you prefer javadoc-style, you should be able to generate it pretty
easily.

**WHERE DO I FIND JABBER-NET?**

The project HAS BEEN MOVED (again) to <http://code.google.com/p/jabber-net/>.
Join the [mailing list](http://groups.google.com/group/jabber-net) to get
involved.

For more information, contact <joe-jabbernet@cursive.net>.
