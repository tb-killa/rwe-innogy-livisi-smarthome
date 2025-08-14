using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Rebex.Mime.Headers;
using onrkn;

namespace Rebex.Mime;

public class MimeMessage : MimeEntity
{
	private class rfhyv<T0, T1> : afmdb where T0 : HeaderValueCollection, new()
	{
		private sealed class xxdzp : IEnumerator<object>, IEnumerator, IDisposable
		{
			private object zmaxv;

			private int vrlcj;

			public rfhyv<T0, T1> lagce;

			public T0 obhjf;

			public IEnumerator aaqll;

			public IEnumerator<T0> vnjlg;

			private object qyuno => zmaxv;

			private object mipmr => zmaxv;

			private bool reaqo()
			{
				try
				{
					switch (vrlcj)
					{
					case 0:
						vrlcj = -1;
						vnjlg = lagce.afwzp().GetEnumerator();
						vrlcj = 1;
						goto IL_00ad;
					case 2:
						{
							vrlcj = 1;
							goto IL_0098;
						}
						IL_00ad:
						if (vnjlg.MoveNext() ? true : false)
						{
							obhjf = vnjlg.Current;
							T0 val = obhjf;
							aaqll = val.aqogv();
							goto IL_0098;
						}
						wafjm();
						break;
						IL_0098:
						if (aaqll.MoveNext() ? true : false)
						{
							zmaxv = aaqll.Current;
							vrlcj = 2;
							return true;
						}
						goto IL_00ad;
					}
					return false;
				}
				catch
				{
					//try-fault
					nwceg();
					throw;
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in reaqo
				return this.reaqo();
			}

			private void tuuvi()
			{
				throw new NotSupportedException();
			}

			void IEnumerator.Reset()
			{
				//ILSpy generated this explicit interface implementation from .override directive in tuuvi
				this.tuuvi();
			}

			private void nwceg()
			{
				switch (vrlcj)
				{
				case 1:
				case 2:
					try
					{
						break;
					}
					finally
					{
						wafjm();
					}
				}
			}

			void IDisposable.Dispose()
			{
				//ILSpy generated this explicit interface implementation from .override directive in nwceg
				this.nwceg();
			}

			public xxdzp(int _003C_003E1__state)
			{
				vrlcj = _003C_003E1__state;
			}

			private void wafjm()
			{
				vrlcj = -1;
				if (vnjlg != null && 0 == 0)
				{
					vnjlg.Dispose();
				}
			}
		}

		private readonly MimeHeaderCollection cpwdu;

		private readonly string flbbw;

		public override int srbcw
		{
			get
			{
				int num = 0;
				IEnumerator<T0> enumerator = afwzp().GetEnumerator();
				try
				{
					while (enumerator.MoveNext() ? true : false)
					{
						T0 current = enumerator.Current;
						num += current.Count;
					}
					return num;
				}
				finally
				{
					if (enumerator != null && 0 == 0)
					{
						enumerator.Dispose();
					}
				}
			}
		}

		public override object this[int index]
		{
			get
			{
				IEnumerator<T0> enumerator = afwzp().GetEnumerator();
				try
				{
					while (enumerator.MoveNext() ? true : false)
					{
						T0 current = enumerator.Current;
						int count = current.Count;
						if (index < count)
						{
							return current[index];
						}
						index -= count;
					}
				}
				finally
				{
					if (enumerator != null && 0 == 0)
					{
						enumerator.Dispose();
					}
				}
				throw new ArgumentOutOfRangeException("index");
			}
			set
			{
				IEnumerator<T0> enumerator = afwzp().GetEnumerator();
				try
				{
					while (enumerator.MoveNext() ? true : false)
					{
						T0 current = enumerator.Current;
						int count = current.Count;
						if (index < count)
						{
							current[index] = value;
							return;
						}
						index -= count;
					}
				}
				finally
				{
					if (enumerator != null && 0 == 0)
					{
						enumerator.Dispose();
					}
				}
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public rfhyv(MimeHeaderCollection headers, string headerName)
		{
			cpwdu = headers;
			flbbw = headerName;
		}

		public override IEnumerator nxtqg()
		{
			xxdzp xxdzp = new xxdzp(0);
			xxdzp.lagce = this;
			return xxdzp;
		}

		private IEnumerable<T0> afwzp()
		{
			List<T0> list = new List<T0>();
			IEnumerator enumerator = cpwdu.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					MimeHeader mimeHeader = (MimeHeader)enumerator.Current;
					if ((string.Compare(mimeHeader.Name, flbbw, StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0) && mimeHeader.Value is T0 item && 0 == 0)
					{
						list.Add(item);
					}
				}
				return list;
			}
			finally
			{
				if (enumerator is IDisposable disposable && 0 == 0)
				{
					disposable.Dispose();
				}
			}
		}

		public override int wecpv(object p0)
		{
			int num = 0;
			HeaderValueCollection headerValueCollection = null;
			IEnumerator<T0> enumerator = afwzp().GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					T0 current = enumerator.Current;
					headerValueCollection = current;
					num += current.Count;
				}
			}
			finally
			{
				if (enumerator != null && 0 == 0)
				{
					enumerator.Dispose();
				}
			}
			if (headerValueCollection == null || 1 == 0)
			{
				T0 val = new T0();
				val.kkdtb(p0);
				cpwdu.Add(new MimeHeader(flbbw, val, canonize: true));
			}
			else
			{
				headerValueCollection.kkdtb(p0);
			}
			return num;
		}

		public override void ijhui(int p0)
		{
			IEnumerator<T0> enumerator = afwzp().GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					T0 current = enumerator.Current;
					int count = current.Count;
					if (p0 < count)
					{
						current.RemoveAt(p0);
						return;
					}
					p0 -= count;
				}
			}
			finally
			{
				if (enumerator != null && 0 == 0)
				{
					enumerator.Dispose();
				}
			}
			throw new ArgumentOutOfRangeException("index");
		}

		public override void rkqew()
		{
			cpwdu.Remove(flbbw);
		}
	}

	private const string ojlpa = "This is a multipart MIME message.\n";

	private string lfcry;

	public string EnvelopeId
	{
		get
		{
			if (lfcry == null || 1 == 0)
			{
				return "";
			}
			return lfcry;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			int num = 0;
			int num2 = 0;
			if (num2 != 0)
			{
				goto IL_0020;
			}
			goto IL_008c;
			IL_008c:
			if (num2 >= value.Length)
			{
				if (num > 100)
				{
					throw new ArgumentException(brgjd.edcru("Supplied envelope ID exceeds the maximum allowed length (100) by {0} characters.", num - 100), "value");
				}
				lfcry = value;
				return;
			}
			goto IL_0020;
			IL_0020:
			if (value[num2] >= '\u0080')
			{
				throw new ArgumentException(brgjd.edcru("Invalid character in envelope ID at position {0}.", num2), "value");
			}
			if (value[num2] < '!' || value[num2] > '~' || value[num2] == '+' || value[num2] == '=')
			{
				num += 2;
			}
			num++;
			num2++;
			goto IL_008c;
		}
	}

	public MimePriority Priority
	{
		get
		{
			MimeHeader mimeHeader = base.Headers["X-Priority"];
			if (mimeHeader == null || 1 == 0)
			{
				mimeHeader = base.Headers["Priority"];
			}
			if (mimeHeader == null || 1 == 0)
			{
				return MimePriority.Normal;
			}
			string text = mimeHeader.Raw.ToLower(CultureInfo.InvariantCulture);
			if (text.IndexOf("low") >= 0 || text.IndexOf("non-urgent") >= 0)
			{
				return MimePriority.Low;
			}
			if (text.IndexOf("hi") >= 0 || text.IndexOf("urgent") >= 0)
			{
				return MimePriority.High;
			}
			if (text.IndexOf("med") >= 0)
			{
				return MimePriority.Normal;
			}
			if (text.Length == 1)
			{
				int num = text[0] - 48;
				if (num >= 0 && num <= 2)
				{
					return MimePriority.High;
				}
				if (num >= 4 && num <= 9)
				{
					return MimePriority.Low;
				}
			}
			return MimePriority.Normal;
		}
		set
		{
			string value2 = (3 - (int)value * 2).ToString(CultureInfo.InvariantCulture);
			base.Headers.Remove("Priority");
			base.Headers.Remove("X-Priority");
			base.Headers["X-Priority"] = new MimeHeader("X-Priority", value2);
		}
	}

	public MailDateTime Date
	{
		get
		{
			IHeader header = base.Headers.etuur("date");
			return header as MailDateTime;
		}
		set
		{
			base.Headers.tyaam("date", value);
		}
	}

	public MailAddressCollection From
	{
		get
		{
			return zxmze<MailAddressCollection, MailAddress>("from");
		}
		set
		{
			njdfz("from", value);
		}
	}

	public MailAddress Sender
	{
		get
		{
			IHeader header = base.Headers.etuur("sender");
			return header as MailAddress;
		}
		set
		{
			base.Headers.tyaam("sender", value);
		}
	}

	public MailAddressCollection ReplyTo
	{
		get
		{
			return zxmze<MailAddressCollection, MailAddress>("reply-to");
		}
		set
		{
			njdfz("reply-to", value);
		}
	}

	public MailAddressCollection To
	{
		get
		{
			return zxmze<MailAddressCollection, MailAddress>("to");
		}
		set
		{
			njdfz("to", value);
		}
	}

	public MailAddressCollection CC
	{
		get
		{
			return zxmze<MailAddressCollection, MailAddress>("cc");
		}
		set
		{
			njdfz("cc", value);
		}
	}

	public MailAddressCollection Bcc
	{
		get
		{
			return zxmze<MailAddressCollection, MailAddress>("bcc");
		}
		set
		{
			njdfz("bcc", value);
		}
	}

	public MessageId MessageId
	{
		get
		{
			IHeader header = base.Headers.etuur("message-id");
			return header as MessageId;
		}
		set
		{
			base.Headers.tyaam("message-id", value);
		}
	}

	public MessageIdCollection InReplyTo
	{
		get
		{
			return zxmze<MessageIdCollection, MessageId>("in-reply-to");
		}
		set
		{
			njdfz("in-reply-to", value);
		}
	}

	public MessageIdCollection References
	{
		get
		{
			return zxmze<MessageIdCollection, MessageId>("references");
		}
		set
		{
			njdfz("references", value);
		}
	}

	public string Subject
	{
		get
		{
			IHeader header = base.Headers.etuur("subject");
			if (header == null || 1 == 0)
			{
				return string.Empty;
			}
			return header.ToString();
		}
		set
		{
			base.Headers.tyaam("subject", new Unstructured(value));
		}
	}

	public string Comments
	{
		get
		{
			IHeader header = base.Headers.etuur("comments");
			if (header == null || 1 == 0)
			{
				return string.Empty;
			}
			return header.ToString();
		}
		set
		{
			base.Headers.tyaam("comments", new Unstructured(value));
		}
	}

	public PhraseCollection Keywords
	{
		get
		{
			return zxmze<PhraseCollection, hszhl>("keywords");
		}
		set
		{
			njdfz("keywords", value);
		}
	}

	public MailAddressCollection DispositionNotificationTo
	{
		get
		{
			return zxmze<MailAddressCollection, MailAddress>("disposition-notification-to");
		}
		set
		{
			njdfz("disposition-notification-to", value);
		}
	}

	public ListCommandUrlCollection ListArchive
	{
		get
		{
			return zxmze<ListCommandUrlCollection, ListCommandUrl>("list-archive");
		}
		set
		{
			njdfz("list-archive", value);
		}
	}

	public ListCommandUrlCollection ListHelp
	{
		get
		{
			return zxmze<ListCommandUrlCollection, ListCommandUrl>("list-help");
		}
		set
		{
			njdfz("list-help", value);
		}
	}

	public ListCommandUrlCollection ListOwner
	{
		get
		{
			return zxmze<ListCommandUrlCollection, ListCommandUrl>("list-owner");
		}
		set
		{
			njdfz("list-owner", value);
		}
	}

	public ListCommandUrlCollection ListSubscribe
	{
		get
		{
			return zxmze<ListCommandUrlCollection, ListCommandUrl>("list-subscribe");
		}
		set
		{
			njdfz("list-subscribe", value);
		}
	}

	public ListCommandUrlCollection ListPost
	{
		get
		{
			return zxmze<ListCommandUrlCollection, ListCommandUrl>("list-post");
		}
		set
		{
			njdfz("list-post", value);
		}
	}

	public ListCommandUrlCollection ListUnsubscribe
	{
		get
		{
			return zxmze<ListCommandUrlCollection, ListCommandUrl>("list-unsubscribe");
		}
		set
		{
			njdfz("list-unsubscribe", value);
		}
	}

	public MimeMessage()
	{
		base.Preamble = "This is a multipart MIME message.\n";
		base.Epilogue = null;
	}

	internal MimeMessage(MimeEntity entity)
		: base(entity)
	{
		if (entity is MimeMessage mimeMessage && 0 == 0)
		{
			lfcry = mimeMessage.lfcry;
		}
	}

	public override MimeEntity Clone()
	{
		return new MimeMessage(this);
	}

	private TList zxmze<TList, TItem>(string p0) where TList : HeaderValueCollection, new()
	{
		TList result = new TList();
		rfhyv<TList, TItem> p1 = new rfhyv<TList, TItem>(base.Headers, p0);
		result.kgrvh(p1);
		return result;
	}

	private void njdfz<TList>(string p0, TList p1) where TList : HeaderValueCollection, new()
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("headerName");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("list");
		}
		if (p1 == null || false || p1.Count == 0 || 1 == 0)
		{
			base.Headers.tyaam(p0, null);
			return;
		}
		TList p2 = new TList();
		p2.AddRange(p1);
		base.Headers.tyaam(p0, p2);
	}
}
