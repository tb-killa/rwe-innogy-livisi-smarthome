using System;
using System.Globalization;
using System.IO;
using Rebex.OutlookMessages;

namespace onrkn;

internal class wyfgf
{
	internal const string mpjfi = "embedded-rtf-attachment-id{0}@rebex.net";

	private readonly howhn jifdi;

	private readonly qacae jomzd;

	private readonly bool rotir;

	private readonly bool iasol;

	private readonly bool ykcgp;

	internal bool durhy => iasol;

	internal bool vgcwk => ykcgp;

	public howhn vbjpq => jifdi;

	public object rcncd
	{
		get
		{
			if (jomzd != null && 0 == 0)
			{
				return jomzd.tgbhs;
			}
			return null;
		}
	}

	public xcrar lcpzw
	{
		get
		{
			if (jomzd != null && 0 == 0)
			{
				return jomzd.pzpvc;
			}
			return xcrar.crqsh;
		}
	}

	public bool jgqfq => rotir;

	internal static bool usxuw(Stream p0)
	{
		int num = 0;
		byte[] array = new byte[8];
		int num2;
		do
		{
			num2 = p0.Read(array, num, array.Length - num);
			num += num2;
		}
		while (num2 > 0 && num < array.Length);
		return sujay(array);
	}

	internal static bool sujay(byte[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new InvalidOperationException("Attachment data cannot be null");
		}
		if (p0.Length < 2)
		{
			return false;
		}
		if (p0[0] == byte.MaxValue && p0[1] == 216)
		{
			return true;
		}
		if ((p0[0] == 66 && p0[1] == 77) || (p0[0] == 66 && p0[1] == 65) || (p0[0] == 67 && p0[1] == 73) || (p0[0] == 67 && p0[1] == 80) || (p0[0] == 73 && p0[1] == 67) || (p0[0] == 80 && p0[1] == 84))
		{
			return true;
		}
		if (p0.Length >= 8 && p0[0] == 137 && p0[1] == 80 && p0[2] == 78 && p0[3] == 71 && p0[4] == 13 && p0[5] == 10 && p0[6] == 26 && p0[7] == 10)
		{
			return true;
		}
		if (p0.Length >= 6 && p0[0] == 71 && p0[1] == 73 && p0[2] == 70 && p0[3] == 56 && (p0[4] == 55 || p0[4] == 57) && p0[5] == 97)
		{
			return true;
		}
		return false;
	}

	internal wyfgf(howhn properties)
	{
		jifdi = properties;
		jomzd = jifdi[MsgPropertyTag.AttachMethod];
		rotir = jomzd != null && 0 == 0 && (ulypf)jomzd.tgbhs == ulypf.goblg;
		jomzd = jifdi[MsgPropertyTag.AttachDataBinary];
		iasol = jomzd != null && 0 == 0 && jomzd.pzpvc == xcrar.xkvvt;
		rotir = rotir && 0 == 0 && iasol;
		if (!rotir || 1 == 0)
		{
			byte[] array = (byte[])rcncd;
			if (array != null && 0 == 0)
			{
				ykcgp = sujay(array);
			}
		}
	}

	internal wyfgf(jfxnb owner, jfxnb embeddedMessage, int attachNumber, string displayName)
	{
		embeddedMessage.xcprm(owner);
		jifdi = new howhn(owner);
		jomzd = new qacae(jifdi, MsgPropertyTag.AttachDataBinary, xcrar.xkvvt, embeddedMessage);
		rotir = (iasol = true);
		jifdi.oaesx(jomzd);
		jifdi.mycww(MsgPropertyTag.AttachNumber, attachNumber);
		jifdi.mycww(MsgPropertyTag.ObjectType, 7);
		jifdi.mycww(MsgPropertyTag.AttachMethod, 5);
		if (!string.IsNullOrEmpty(displayName) || 1 == 0)
		{
			jifdi.duwaw(MsgPropertyTag.DisplayName, displayName);
			return;
		}
		qacae qacae2 = embeddedMessage.suuuu[MsgPropertyTag.Subject];
		if (qacae2 != null && 0 == 0 && dzwgu.dovit(qacae2.pzpvc) && 0 == 0 && ((string)qacae2.tgbhs).Length > 0)
		{
			jifdi.duwaw(MsgPropertyTag.DisplayName, (string)qacae2.tgbhs);
		}
		else
		{
			jifdi.duwaw(MsgPropertyTag.DisplayName, "Untitled");
		}
	}

	private wyfgf(jfxnb owner, Stream inputStream)
	{
		long num = ((inputStream.CanSeek ? true : false) ? (inputStream.Length - inputStream.Position) : 0);
		if (num > int.MaxValue)
		{
			throw new NotSupportedException("Input stream is too long.");
		}
		byte[] array = null;
		MemoryStream memoryStream = inputStream as MemoryStream;
		eorvm eorvm2 = inputStream as eorvm;
		int num2;
		if (memoryStream != null && 0 == 0)
		{
			if (memoryStream.Position == 0)
			{
				array = memoryStream.ToArray();
				memoryStream.Position = array.Length;
			}
			else if (inputStream.CanSeek && 0 == 0)
			{
				array = new byte[(int)num];
				num2 = 0;
				if (num2 != 0)
				{
					goto IL_009c;
				}
				goto IL_00d7;
			}
		}
		else if (eorvm2 != null && 0 == 0)
		{
			array = new byte[eorvm2.Length];
			eorvm2.Read(array, 0, array.Length);
		}
		goto IL_0103;
		IL_009c:
		int num3 = memoryStream.Read(array, num2, array.Length - num2);
		if (num3 <= 0)
		{
			if (num2 > 0)
			{
				byte[] src = array;
				array = new byte[num2];
				Buffer.BlockCopy(src, 0, array, 0, num2);
			}
			goto IL_0103;
		}
		num2 += num3;
		goto IL_00d7;
		IL_00d7:
		if (num2 < num)
		{
			goto IL_009c;
		}
		goto IL_0103;
		IL_0103:
		if (array == null || 1 == 0)
		{
			array = new byte[1024];
			MemoryStream memoryStream2 = new MemoryStream((int)num);
			num = 0L;
			while (true)
			{
				int num4 = inputStream.Read(array, 0, array.Length);
				if (num4 <= 0)
				{
					break;
				}
				num += num4;
				if (num > int.MaxValue)
				{
					throw new NotSupportedException("Input stream is too long.");
				}
				memoryStream2.Write(array, 0, num4);
			}
			array = memoryStream2.ToArray();
		}
		jifdi = new howhn(owner);
		jomzd = new qacae(jifdi, MsgPropertyTag.AttachDataBinary, xcrar.yesjh, array);
		jifdi.oaesx(jomzd);
		jifdi.mycww(MsgPropertyTag.ObjectType, 7);
		jifdi.mycww(MsgPropertyTag.AttachMethod, 1);
		jifdi.mycww(MsgPropertyTag.AttachSize, array.Length);
		if (owner.kfurm && 0 == 0)
		{
			jifdi.mycww(MsgPropertyTag.StoreSupportMask, 16384);
		}
	}

	internal wyfgf(jfxnb owner, Stream inputStream, int attachNumber, string fileName, string displayName)
		: this(owner, inputStream, attachNumber, fileName, displayName, null, null, null, null)
	{
	}

	internal wyfgf(jfxnb owner, Stream inputStream, int attachNumber, string fileName, string displayName, string contentType, string contentId, string contentLocation)
		: this(owner, inputStream)
	{
		string p = null;
		string text = null;
		string[] array;
		if (contentType != null && 0 == 0)
		{
			ljrjo ljrjo2 = new ljrjo(contentType.ToLower(CultureInfo.InvariantCulture));
			text = ljrjo2["media-type"];
			p = ljrjo2["charset"];
			if (fileName == null || 1 == 0)
			{
				if (ljrjo2["name"] != null && 0 == 0)
				{
					fileName = ljrjo2["name"];
				}
				else
				{
					array = text.Split('/');
					fileName = brgjd.edcru("{0}{1:D3}", array[0], attachNumber);
					string text2;
					if ((text2 = text) == null)
					{
						goto IL_0176;
					}
					if (!(text2 == "message/rfc822") || 1 == 0)
					{
						if (!(text2 == "application/octet-stream") || 1 == 0)
						{
							if (!(text2 == "text/plain") || 1 == 0)
							{
								if (!(text2 == "image/jpeg") || 1 == 0)
								{
									goto IL_0176;
								}
								fileName += ".jpg";
							}
							else
							{
								fileName += ".txt";
							}
						}
						else
						{
							fileName += ".bin";
						}
					}
					else
					{
						fileName += ".eml";
					}
				}
			}
		}
		goto IL_01ba;
		IL_01ba:
		fdspk(attachNumber, fileName, displayName, text, p, contentId, contentLocation);
		return;
		IL_0176:
		fileName = ((array.Length != 1 && ((array[1].Length != 0) ? true : false)) ? (fileName + array[1]) : (fileName + ".dat"));
		goto IL_01ba;
	}

	internal wyfgf(jfxnb owner, Stream inputStream, int attachNumber, string fileName, string displayName, string mediaType, string charSet, string contentId, string contentLocation)
		: this(owner, inputStream)
	{
		fdspk(attachNumber, fileName, displayName, mediaType, charSet, contentId, contentLocation);
	}

	private void fdspk(int p0, string p1, string p2, string p3, string p4, string p5, string p6)
	{
		string text = null;
		string text2 = null;
		if (p1 != null && 0 == 0)
		{
			text2 = Path.GetExtension(p1);
			if (text2.Length <= 4 && p1.Length <= 12)
			{
				text = p1;
			}
		}
		jifdi.mycww(MsgPropertyTag.AttachNumber, p0);
		jifdi.onqsx(MsgPropertyTag.RecordKey, BitConverter.GetBytes(p0));
		if (!string.IsNullOrEmpty(p1) || 1 == 0)
		{
			jifdi.duwaw(MsgPropertyTag.AttachLongFilename, p1);
		}
		if (!string.IsNullOrEmpty(text2) || 1 == 0)
		{
			jifdi.duwaw(MsgPropertyTag.AttachExtension, text2);
		}
		if (!string.IsNullOrEmpty(text) || 1 == 0)
		{
			jifdi.duwaw(MsgPropertyTag.AttachFilename, text);
		}
		if (p2 != null && 0 == 0)
		{
			jifdi.duwaw(MsgPropertyTag.DisplayName, p2);
		}
		else if (p1 != null && 0 == 0)
		{
			jifdi.duwaw(MsgPropertyTag.DisplayName, p1);
		}
		if (p3 != null && 0 == 0)
		{
			jifdi.duwaw(MsgPropertyTag.AttachMimeTag, p3);
		}
		if (p4 != null && 0 == 0 && p3 != null && 0 == 0 && p3.StartsWith("text/", StringComparison.OrdinalIgnoreCase) && 0 == 0)
		{
			jifdi.duwaw(MsgPropertyTag.TextAttachmentCharset, p4);
		}
		if (p5 != null && 0 == 0)
		{
			jifdi.duwaw(MsgPropertyTag.AttachContentId, p5);
		}
		if (p6 != null && 0 == 0)
		{
			jifdi.duwaw(MsgPropertyTag.AttachContentLocation, p6);
		}
	}

	public jfxnb atbph()
	{
		if (rotir && 0 == 0)
		{
			return jomzd.tgbhs as jfxnb;
		}
		throw new InvalidOperationException("Attachment is not an embedded message.");
	}
}
