/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2008 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net can be used under either JOSL or the GPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.IO;
using System.Xml;
using NUnit.Framework;

using bedrock.util;
using jabber.protocol;
using jabber.protocol.x;

namespace test.jabber.protocol.x
{
    /// <summary>
    /// Summary description for DataTest.
    /// </summary>
    [SVN(@"$Id$")]
    [TestFixture]
    public class DataTest
    {
        private bool gotElement = false;

        private const string tstring = @"<stream><x xmlns='jabber:x:data'>
      <instructions>
        Welcome to the BloodBank-Service!  We thank you for registering with
        us and helping to save lives.  Please fill out the following form.
      </instructions>
      <field type='hidden' var='form_number'>
        <value>113452</value>
      </field>
      <field type='fixed'>
        <value>We need your contact information.</value>
      </field>
      <field type='text-single' label='First Name' var='first'>
        <required/>
      </field>
      <field type='text-single' label='Last Name' var='last'>
        <required/>
      </field>
      <field type='list-single' label='Gender' var='gender'>
        <value>male</value>
        <option label='Male'><value>male</value></option>
        <option label='Female'><value>female</value></option>
      </field>
      <field type='fixed'>
        <value>We need your blood information.</value>
      </field>
      <field type='list-single' label='Blood Type' var='blood_type'>
        <required/>
        <value>a+</value>
        <option label='A-Positive'><value>a+</value></option>
        <option label='B-Negative'><value>b-</value></option>
        etc...
      </field>
      <field type='list-single' label='Pints to give' var='pints'>
        <required/>
        <value>2</value>
        <option label='Zero'><value>0</value></option>
        <option label='One'><value>1</value></option>
        <option label='Two'><value>2</value></option>
        <option label='Three'><value>3</value></option>
      </field>
      <field type='boolean' label='Willing to donate' var='willing'>
        <required/>
        <value>1</value>
      </field>
    </x></stream>";

        [Test] public void Test_Parse()
        {
            AsynchElementStream es = new AsynchElementStream();
            es.AddFactory(new global::jabber.protocol.x.Factory());
            es.OnElement += new ProtocolHandler(es_OnElement);
            es.Push(System.Text.Encoding.UTF8.GetBytes(tstring));

            Assert.IsTrue(gotElement);
        }

        void es_OnElement(object sender, XmlElement n)
        {
            Assert.IsInstanceOfType(typeof(global::jabber.protocol.x.Data), n);
            Data d = (Data)n;
            Assert.AreEqual(@"
        Welcome to the BloodBank-Service!  We thank you for registering with
        us and helping to save lives.  Please fill out the following form.
      ", d.Instructions);
            Field[] fields = d.GetFields();
            Assert.AreEqual(9, fields.Length);
            Assert.AreEqual("2", fields[7].Val);
            Assert.AreEqual(true, fields[7].IsRequired);
            Assert.AreEqual(false, fields[4].IsRequired);
            Assert.AreEqual("Pints to give", fields[7].Label);
            Assert.AreEqual("pints", fields[7].Var);
            Assert.AreEqual(FieldType.list_single, fields[7].Type);
            Assert.AreEqual(d.GetField("pints").Label, "Pints to give");

            Option[] options = fields[7].GetOptions();
            Assert.AreEqual(4, options.Length);
            Assert.AreEqual("Two", options[2].Label);
            Assert.AreEqual("2", options[2].Val);
            gotElement = true;
        }

        [Test]
        public void Test_Convert()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(tstring);

            ElementFactory f = new ElementFactory();
            f.AddType(new global::jabber.protocol.x.Factory());

            Element stream = Element.AddTypes(doc.DocumentElement, f);
            Data d = stream.GetChildElement<global::jabber.protocol.x.Data>();
            Assert.IsNotNull(d);
        }
    }
}
