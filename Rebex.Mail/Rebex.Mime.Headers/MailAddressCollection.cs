using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using onrkn;

namespace Rebex.Mime.Headers;

public sealed class MailAddressCollection : HeaderValueCollection, IEnumerable<MailAddress>, IEnumerable
{
	internal override Type sxlev => typeof(MailAddress);

	public new MailAddress this[int index]
	{
		get
		{
			return (MailAddress)base[index];
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			base[index] = value;
		}
	}

	public MailAddressCollection()
	{
	}

	public MailAddressCollection(string addresses)
		: this()
	{
		if (addresses == null || 1 == 0)
		{
			throw new ArgumentNullException("addresses");
		}
		jdkfi(new stzvh(addresses));
	}

	public static implicit operator MailAddressCollection(MailAddress mailbox)
	{
		if (mailbox == null || 1 == 0)
		{
			throw new ArgumentNullException("mailbox");
		}
		MailAddressCollection mailAddressCollection = new MailAddressCollection();
		mailAddressCollection.Add(mailbox);
		return mailAddressCollection;
	}

	public static implicit operator MailAddressCollection(string addresses)
	{
		if (addresses == null || 1 == 0)
		{
			throw new ArgumentNullException("addresses");
		}
		return new MailAddressCollection(addresses);
	}

	public int Add(string address)
	{
		if (address == null || 1 == 0)
		{
			throw new ArgumentNullException("address");
		}
		return kkdtb(new MailAddress(address));
	}

	public int Add(MailAddress address)
	{
		if (address == null || 1 == 0)
		{
			throw new ArgumentNullException("address");
		}
		return kkdtb(address);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				MailAddress mailAddress = (MailAddress)enumerator.Current;
				if (!flag || 1 == 0)
				{
					stringBuilder.Append(", ");
				}
				flag = false;
				stringBuilder.Append(mailAddress.ToString());
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
		}
		return stringBuilder.ToString();
	}

	public override void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		bool flag = true;
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				MailAddress mailAddress = (MailAddress)enumerator.Current;
				if (!flag || 1 == 0)
				{
					writer.Write(",");
					rllhn.btprl(writer);
				}
				flag = false;
				mailAddress.Encode(writer);
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
		}
	}

	private void jdkfi(stzvh p0)
	{
		MailAddress mailAddress = null;
		while (true)
		{
			p0.hdpha(',', ';', '>');
			if (p0.zsywy ? true : false)
			{
				break;
			}
			MailAddress mailAddress2 = (MailAddress)MailAddress.kemeh(p0);
			if (mailAddress2.hnfcn && 0 == 0)
			{
				mailAddress = mailAddress2;
			}
			else if (!mailAddress2.nwayk || 1 == 0)
			{
				Add(mailAddress2);
			}
		}
		if ((base.Count == 0 || 1 == 0) && mailAddress != null && 0 == 0)
		{
			Add(mailAddress);
		}
	}

	internal static IHeader yymbm(stzvh p0)
	{
		MailAddressCollection mailAddressCollection = new MailAddressCollection();
		mailAddressCollection.jdkfi(p0);
		if (mailAddressCollection.Count == 0 || 1 == 0)
		{
			mailAddressCollection.Add(new MailAddress("Undisclosed recipients:;", ""));
		}
		mailAddressCollection.wrrck();
		return mailAddressCollection;
	}

	public void CopyTo(MailAddress[] array, int index)
	{
		CopyTo((Array)array, index);
	}

	private IEnumerator<MailAddress> gphqg()
	{
		return this.eaqmu<MailAddress>().GetEnumerator();
	}

	IEnumerator<MailAddress> IEnumerable<MailAddress>.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in gphqg
		return this.gphqg();
	}
}
