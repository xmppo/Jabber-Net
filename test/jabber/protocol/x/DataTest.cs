/* --------------------------------------------------------------------------
 *
 * License
 *
 * The contents of this file are subject to the Jabber Open Source License
 * Version 1.0 (the "License").  You may not copy or use this file, in either
 * source code or executable form, except in compliance with the License.  You
 * may obtain a copy of the License at http://www.jabber.com/license/ or at
 * http://www.opensource.org/.  
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied.  See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002-2004 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
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
    [RCS(@"$Header$")]
    [TestFixture]
    public class DataTest
    {
        private const string tstring = @"<x xmlns='jabber:x:data'>
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
    </x>";

        public void Test_Parse()
        {
            XmlDocument doc = new XmlDocument();
            XmlTextReader tr = new XmlTextReader(new StringReader(tstring));
            XmlLoader l = new XmlLoader(tr, doc);
            ElementFactory ef = new ElementFactory();
            ef.AddType(new Factory());
            l.Factory = ef;
            tr.Read();
            XmlNode n = l.ReadCurrentNode();
            Assertion.AssertEquals("jabber.protocol.x.Data", n.GetType().ToString());
            Data d = (Data) n;
            Assertion.AssertEquals(@"
        Welcome to the BloodBank-Service!  We thank you for registering with
        us and helping to save lives.  Please fill out the following form.
      ", d.Instructions);
            Field[] fields = d.GetFields();
            Assertion.AssertEquals(9, fields.Length);
            Assertion.AssertEquals("2", fields[7].Val);
            Assertion.AssertEquals(true, fields[7].IsRequired);
            Assertion.AssertEquals(false, fields[4].IsRequired);
            Assertion.AssertEquals("Pints to give", fields[7].Label);
            Assertion.AssertEquals("pints", fields[7].Var);
            Assertion.AssertEquals(FieldType.list_single, fields[7].Type);
            Assertion.AssertEquals(d.GetField("pints").Label, "Pints to give");

            Option[] options = fields[7].GetOptions();
            Assertion.AssertEquals(4, options.Length);
            Assertion.AssertEquals("Two", options[2].Label);
            Assertion.AssertEquals("2", options[2].Val);
        }
    }
}