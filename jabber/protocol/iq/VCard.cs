/* --------------------------------------------------------------------------
 * 
 * --------------------------------------------------------------------------*/
using System;
using System.Xml;

using bedrock.util;

namespace jabber.protocol.iq
{
	/// <summary>
	/// Telephone type attribute
	/// </summary>
	public enum TelephoneType
	{
		/// <summary>
		/// Voice 
		/// </summary>
		voice, 
		/// <summary>
		/// Fax
		/// </summary>
		fax, 
		/// <summary>
		/// Message
		/// </summary>
		message,
		/// <summary>
		/// Unknown
		/// </summary>
		unknown
	}

	/// <summary>
	/// Telephone location attribute
	/// </summary>
	public enum TelephoneLocation
	{
		/// <summary>
		/// Home
		/// </summary>
		home, 
		/// <summary>
		/// Work
		/// </summary>
		work,
		/// <summary>
		/// Unknown
		/// </summary>
		unknown
	}

	/// <summary>
	/// Address location attribute
	/// </summary>
	public enum AddressLocation
	{
		/// <summary>
		/// Home
		/// </summary>
		home, 
		/// <summary>
		/// Work
		/// </summary>
		work,
		/// <summary>
		/// Unknown
		/// </summary>
		unknown
	}

	/// <summary>
	/// Email type attribute
	/// </summary>
	public enum EmailType
	{
		/// <summary>
		/// Home
		/// </summary>
		home, 
		/// <summary>
		/// Work
		/// </summary>
		work, 
		/// <summary>
		/// Internet
		/// </summary>
		internet,
		/// <summary>
		/// x400
		/// </summary>
		x400,
		/// <summary>
		/// Unknown
		/// </summary>
		unknown
	}

	/// <summary>
	/// IQ packet with a version query element inside.
	/// </summary>
	[RCS(@"$Header$")]
	public class VCardIQ : jabber.protocol.client.IQ
	{
		/// <summary>
		/// Create a vCard IQ
		/// </summary>
		/// <param name="doc"></param>
		public VCardIQ(XmlDocument doc) : base(doc)
		{
			this.Query = new VCard(doc);
		}
	}

	/// <summary>
	/// A vCard element.
	/// </summary>
	[RCS(@"$Header$")]
	public class VCard : Element
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		public VCard(XmlDocument doc) : base("VCARD", URI.VCARD, doc)
		{
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="qname"></param>
		/// <param name="doc"></param>
		public VCard(string prefix, XmlQualifiedName qname, XmlDocument doc) :
			base(prefix, qname, doc)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public string FullName
		{
			get { return GetElem("FN"); }
			set { SetElem("FN", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		new public Name Name
		{
			get
			{
				if (this["N"] == null)
				{
					Name n = new Name(this.OwnerDocument);
					AddChild(n);
				}
				return this["N"] as Name;
			}
			set { ReplaceChild(value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Nickname
		{
			get { return GetElem("NICKNAME"); }
			set { SetElem("NICKNAME", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Url
		{
			get { return GetElem("URL"); }
			set { SetElem("URL", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Birthday
		{
			get { return GetElem("BDAY"); }
			set { SetElem("BDAY", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Location
		{
			get { return GetElem("LOCATION"); }
			set { SetElem("LOCATION", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Age
		{
			get { return GetElem("AGE"); }
			set { SetElem("AGE", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Gender
		{
			get { return GetElem("GENDER"); }
			set { SetElem("GENDER", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string MaritalStatus
		{
			get { return GetElem("MARITALSTATUS"); }
			set { SetElem("MARITALSTATUS", value); }
		}
		/// <summary>
		/// 
		/// </summary>
		public string WorkCell
		{
			get { return GetElem("WORKCELL"); }
			set { SetElem("WORKCELL", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string HomeCell
		{
			get { return GetElem("HOMECELL"); }
			set { SetElem("HOMECELL", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Organization Organization
		{
			get { return this["ORG"] as Organization; }
			set { this.ReplaceChild(value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			get { return GetElem("TITLE"); }
			set { SetElem("TITLE", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Role
		{
			get { return GetElem("ROLE"); }
			set { SetElem("ROLE", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string JabberId
		{
			get { return GetElem("JABBERID"); }
			set { SetElem("JABBERID", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Description
		{
			get { return GetElem("DESC"); }
			set { SetElem("DESC", value); }
		}

		/// <summary>
		/// List of telephone numbers
		/// </summary>
		/// <returns></returns>
		public Telephone[] GetTelephoneList()
		{
			XmlNodeList nl = GetElementsByTagName("TEL", URI.VCARD);
			Telephone[] numbers = new Telephone[nl.Count];
			int i=0;
			foreach (XmlNode n in nl)
			{
				numbers[i] = (Telephone) n;
				i++;
			}
			return numbers;
		}
		
		private Telephone GetTelephone(TelephoneType type, TelephoneLocation location)
		{
			Telephone ret = null;
			foreach (Telephone tel in GetTelephoneList())
			{
				if (tel.Location == location)
					if (tel.Type == type)
						ret = tel;
			}
			if (ret == null)
			{
				ret = new Telephone(this.OwnerDocument);
				ret.Type = type;
				ret.Location = location;
				ret.Number = "";

				AddChild(ret);
			}
			return ret;
		}

		/// <summary>
		/// Get the home voice telephone number
		/// </summary>
		/// <returns></returns>
		public string HomeVoiceNumber
		{
			get
			{
				return GetTelephone(TelephoneType.voice, TelephoneLocation.home).Number;
			}
			set
			{
				GetTelephone(TelephoneType.voice, TelephoneLocation.home).Number = value;
			}
		}

		/// <summary>
		/// Get the home voice telephone number
		/// </summary>
		/// <returns></returns>
		public string HomeFaxNumber
		{
			get
			{
				return GetTelephone(TelephoneType.fax, TelephoneLocation.home).Number;
			}
			set
			{
				GetTelephone(TelephoneType.fax, TelephoneLocation.home).Number = value;
			}
		}

		/// <summary>
		/// Get the home message telephone number
		/// </summary>
		/// <returns></returns>
		public string HomeMessageNumber
		{
			get
			{
				return GetTelephone(TelephoneType.message, TelephoneLocation.home).Number;
			}
			set
			{
				GetTelephone(TelephoneType.message, TelephoneLocation.home).Number = value;
			}
		}

		/// <summary>
		/// Get the home voice telephone number
		/// </summary>
		/// <returns></returns>
		public string WorkVoiceNumber
		{
			get
			{
				return GetTelephone(TelephoneType.voice, TelephoneLocation.work).Number;
			}
			set
			{
				GetTelephone(TelephoneType.voice, TelephoneLocation.work).Number = value;
			}
		}

		/// <summary>
		/// Get the home voice telephone number
		/// </summary>
		/// <returns></returns>
		public string WorkFaxNumber
		{
			get
			{
				return GetTelephone(TelephoneType.fax, TelephoneLocation.work).Number;
			}
			set
			{
				GetTelephone(TelephoneType.fax, TelephoneLocation.work).Number = value;
			}
		}

		/// <summary>
		/// Get the home message telephone number
		/// </summary>
		/// <returns></returns>
		public string WorkMessageNumber
		{
			get
			{
				return GetTelephone(TelephoneType.message, TelephoneLocation.work).Number;
			}
			set
			{
				GetTelephone(TelephoneType.message, TelephoneLocation.work).Number = value;
			}
		}

		/// <summary>
		/// List of addresses
		/// </summary>
		/// <returns></returns>
		public Address[] GetAddressList()
		{
			XmlNodeList nl = GetElementsByTagName("ADR", URI.VCARD);
			Address[] addresses = new Address[nl.Count];
			int i=0;
			foreach (XmlNode n in nl)
			{
				addresses[i] = (Address) n;
				i++;
			}
			return addresses;
		}

		private Address GetAddress(AddressLocation location)
		{
			Address ret = null;
			foreach (Address adr in GetAddressList())
			{
				if (adr.Location == location)
					ret = adr;
			}
			if (ret == null)
			{
				ret = new Address(this.OwnerDocument);
				ret.Location = location;

				AddChild(ret);
			}
			return ret;
		}

		/// <summary>
		/// Get the home address
		/// </summary>
		/// <returns></returns>
		public Address HomeAddress
		{
			get
			{
				return GetAddress(AddressLocation.home);
			}
		}

		/// <summary>
		/// Get the work address
		/// </summary>
		/// <returns></returns>
		public Address WorkAddress
		{
			get
			{
				return GetAddress(AddressLocation.work);
			}
		}

		/// <summary>
		/// List of Email addresses
		/// </summary>
		/// <returns></returns>
		public Email[] GetEmailList()
		{
			XmlNodeList nl = GetElementsByTagName("EMAIL", URI.VCARD);
			Email[] emails = new Email[nl.Count];
			int i=0;
			foreach (XmlNode n in nl)
			{
				emails[i] = (Email)n;
				i++;
			}
			return emails;
		}

		private Email GetEmail(EmailType type)
		{
			Email ret = null;
			foreach (Email email in GetEmailList())
			{
				if (email.Type == type)
					ret = email;
			}
			if (ret == null)
			{
				ret = new Email(this.OwnerDocument);
				ret.Type = type;

				AddChild(ret);
			}
			return ret;
		}

		/// <summary>
		/// Get the home email address
		/// </summary>
		/// <returns></returns>
		public string HomeEmail
		{
			get
			{
				return GetEmail(EmailType.home).UserId;
			}
		}

		/// <summary>
		/// Get the work email address
		/// </summary>
		/// <returns></returns>
		public string WorkEmail
		{
			get
			{
				return GetEmail(EmailType.work).UserId;
			}
		}

		/// <summary>
		/// Get the internet email address (default)
		/// </summary>
		/// <returns></returns>
		public string Email
		{
			get
			{
				return GetEmail(EmailType.internet).UserId;
			}
		}

		/*
		/// <summary>
		/// 
		/// </summary>
		public string EMail
		{
			get { return GetElem("EMAIL"); }
			set { SetElem("EMAIL", value); }
		}
		*/
    }

	/// <summary></summary>
	/// vCard Name Element
	/// </summary>
	public class Name : Element
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		public Name(XmlDocument doc) : base("N", URI.VCARD, doc)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="qname"></param>
		/// <param name="doc"></param>
		public Name(string prefix, XmlQualifiedName qname, XmlDocument doc) :
			base(prefix, qname, doc)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public string Given
		{
			get { return GetElem("GIVEN"); }
			set { SetElem("GIVEN", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Family
		{
			get { return GetElem("FAMILY"); }
			set { SetElem("FAMILY", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Middle
		{
			get { return GetElem("MIDDLE"); }
			set { SetElem("MIDDLE", value); }
		}
	}

	/// <summary></summary>
	/// vCard Org Element
	/// </summary>
	public class Organization : Element
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		public Organization(XmlDocument doc) : base("ORG", URI.VCARD, doc)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="qname"></param>
		/// <param name="doc"></param>
		public Organization(string prefix, XmlQualifiedName qname, XmlDocument doc) :
			base(prefix, qname, doc)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		new public string Name
		{
			get { return GetElem("NAME"); }
			set { SetElem("NAME", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Unit
		{
			get { return GetElem("UNIT"); }
			set { SetElem("UNIT", value); }
		}
	}

	/// <summary></summary>
	/// vCard Telephone Element
	/// </summary>
	public class Telephone : Element
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		public Telephone(XmlDocument doc) : base("TEL", URI.VCARD, doc)
		{
			SetElem("NUMBER", null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="qname"></param>
		/// <param name="doc"></param>
		public Telephone(string prefix, XmlQualifiedName qname, XmlDocument doc) :
			base(prefix, qname, doc)
		{
			SetElem("NUMBER", null);
		}

		/// <summary>
		/// 
		/// </summary>
		public string Number
		{
			get { return GetElem("NUMBER"); }
			set { SetElem("NUMBER", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public TelephoneType Type
		{
			get
			{
				if (this["VOICE"] != null) return TelephoneType.voice;
				else if (this["FAX"] != null) return TelephoneType.fax;
				else if (this["MSG"] != null) return TelephoneType.message;
				else return TelephoneType.unknown;
			}
			set
			{
				RemoveElem("VOICE");
				RemoveElem("FAX");
				RemoveElem("MSG");

				switch (value)
				{
					case TelephoneType.voice:
						SetElem("VOICE", null);
						break;
					case TelephoneType.fax:
						SetElem("FAX", null);
						break;
					case TelephoneType.message:
						SetElem("MSG", null);
						break;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public TelephoneLocation Location
		{
			get
			{
				if (GetElem("WORK") != null) return TelephoneLocation.work;
				else if (GetElem("HOME") != null) return TelephoneLocation.home;
				else return TelephoneLocation.unknown;
			}
			set
			{
				this.RemoveElem("WORK");
				this.RemoveElem("HOME");

				switch (value)
				{
					case TelephoneLocation.work:
						SetElem("WORK", null);
						break;
					case TelephoneLocation.home:
						SetElem("HOME", null);
						break;
				}
			}
		}
	}

	/// <summary></summary>
	/// vCard Address Element
	/// </summary>
	public class Address : Element
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		public Address(XmlDocument doc) : base("ADR", URI.VCARD, doc)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="qname"></param>
		/// <param name="doc"></param>
		public Address(string prefix, XmlQualifiedName qname, XmlDocument doc) :
			base(prefix, qname, doc)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public string Street
		{
			get { return GetElem("STREET"); }
			set { SetElem("STREET", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Locality
		{
			get { return GetElem("LOCALITY"); }
			set { SetElem("LOCALITY", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Region
		{
			get { return GetElem("REGION"); }
			set { SetElem("REGION", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string PostalCode
		{
			get { return GetElem("PCODE"); }
			set { SetElem("PCODE", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Country
		{
			get { return GetElem("CTRY"); }
			set { SetElem("CTRY", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Extra
		{
			get { return GetElem("EXTADD"); }
			set { SetElem("EXTADD", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public AddressLocation Location
		{
			get
			{
				if (this["WORK"] != null) return AddressLocation.work;
				else if (this["HOME"] != null) return AddressLocation.home;
				else return AddressLocation.unknown;
			}
			set
			{
				this.RemoveElem("WORK");
				this.RemoveElem("HOME");

				switch (value)
				{
					case AddressLocation.work:
						SetElem("WORK", null);
						break;
					case AddressLocation.home:
						SetElem("HOME", null);
						break;
				}
			}
		}
	}

	/// <summary></summary>
	/// vCard Email Element
	/// </summary>
	public class Email : Element
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		public Email(XmlDocument doc) : base("EMAIL", URI.VCARD, doc)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="qname"></param>
		/// <param name="doc"></param>
		public Email(string prefix, XmlQualifiedName qname, XmlDocument doc) :
			base(prefix, qname, doc)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public string UserId
		{
			get { return GetElem("USERID"); }
			set { SetElem("USERID", value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public EmailType Type
		{
			get
			{
				if (this["HOME"] != null) return EmailType.home;
				else if (this["WORK"] != null) return EmailType.work;
				else if (this["INTERNET"] != null) return EmailType.internet;
				else if (this["X400"] != null) return EmailType.x400;
				else return EmailType.unknown;
			}
			set
			{
				RemoveElem("HOME");
				RemoveElem("WORK");
				RemoveElem("INTERNET");
				RemoveElem("X400");

				switch (value)
				{
					case EmailType.home:
						SetElem("HOME", null);
						break;
					case EmailType.work:
						SetElem("WORK", null);
						break;
					case EmailType.internet:
						SetElem("INTERNET", null);
						break;
					case EmailType.x400:
						SetElem("X400", null);
						break;
				}
			}
		}
	}
}
