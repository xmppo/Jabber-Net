using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
using jabber.connection;
using System.Threading;

namespace test.bedrock.net
{
    [TestFixture]
    public class HttpUploadTest
    {
        private object m_lock;

        private void uploader_OnUpload(object sender)
        {
            lock(m_lock)
            {
                Monitor.Pulse(m_lock);
            }
        }

        [Test]
        public void UploadFile()
        {
            m_lock = new object();

            HttpUploader uploader = new HttpUploader();
            uploader.OnUpload += new global::bedrock.ObjectHandler(uploader_OnUpload);
            uploader.Upload("http://opodsadny.kiev.luxoft.com:7335/webclient", "les@primus.com/Bass", "upload.txt");
            lock (m_lock)
            {
                Monitor.Wait(m_lock);
            }
        }
    }
}
