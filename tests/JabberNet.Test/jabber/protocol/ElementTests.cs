using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using JabberNet.jabber.protocol.client;
using NUnit.Framework;

namespace JabberNet.Test.jabber.protocol
{
    [TestFixture]
    public class ElementTests
    {
        [Test]
        public void ElementIdsShouldNotClash()
        {
            const int presenceCount = 300;
            var tasks = Enumerable.Range(0, presenceCount)
                .Select(_ => new Task<string>(
                    () => new Presence(new XmlDocument()).ID))
                .ToList();
            foreach (var task in tasks) task.Start();
            var ids = new HashSet<string>(tasks.Select(t => t.Result));
            Assert.AreEqual(presenceCount, ids.Count);
        }
    }
}
