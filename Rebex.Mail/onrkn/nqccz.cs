using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Rebex;
using Rebex.Mail;
using Rebex.Mime;

namespace onrkn;

internal sealed class nqccz
{
	private nqccz()
	{
	}

	public static MimeEntity[] fhzrc(MimeEntity p0, bool p1)
	{
		List<MimeEntity> list = new List<MimeEntity>();
		Encoding encoding = p0.Charset;
		if (encoding == null || 1 == 0)
		{
			encoding = EncodingTools.Default;
		}
		Stream contentStream = p0.GetContentStream();
		try
		{
			StreamReader streamReader = new StreamReader(contentStream, encoding);
			eorvm eorvm2 = new eorvm();
			StreamWriter streamWriter = new StreamWriter(eorvm2, encoding);
			streamWriter.NewLine = "\r\n";
			Regex regex = new Regex("^begin\\s+[0-9]+\\s+(\\S+)$");
			Regex regex2 = new Regex("^end$|^end\\s");
			MemoryStream memoryStream;
			string name;
			if (p1 && 0 == 0)
			{
				memoryStream = new MemoryStream();
				name = p0.Name;
			}
			else
			{
				memoryStream = null;
				name = null;
			}
			while (true)
			{
				string text = streamReader.ReadLine();
				if ((memoryStream == null || 1 == 0) && text != null && 0 == 0)
				{
					Match match = regex.Match(text);
					if (match.Success && 0 == 0 && match.Groups.Count == 2)
					{
						name = match.Groups[1].Value;
						memoryStream = new MemoryStream();
					}
					else
					{
						streamWriter.WriteLine(text);
					}
					continue;
				}
				bool flag = false;
				if (text != null && 0 == 0)
				{
					Match match2 = regex2.Match(text);
					flag = match2.Success;
				}
				if ((flag ? true : false) || text == null)
				{
					if (memoryStream != null && 0 == 0)
					{
						MimeEntity mimeEntity = new MimeEntity();
						memoryStream.Position = 0L;
						mimeEntity.SetContent(memoryStream, "application/octet-stream");
						mimeEntity.TransferEncoding = TransferEncoding.Base64;
						mimeEntity.Name = name;
						list.Add(mimeEntity);
						memoryStream = null;
					}
					else if (text == null || 1 == 0)
					{
						break;
					}
				}
				else
				{
					try
					{
						zuhbq(memoryStream, text);
					}
					catch (MailException)
					{
						return new MimeEntity[0];
					}
				}
			}
			streamWriter.Flush();
			if (list.Count > 0)
			{
				p0.SetContent(eorvm2, null, p0.ContentType.MediaType);
				if (encoding != EncodingTools.ASCII)
				{
					p0.ContentType.CharSet = encoding.WebName.ToLower(CultureInfo.InvariantCulture);
				}
			}
			return list.ToArray();
		}
		finally
		{
			contentStream.Close();
		}
	}

	private static void zuhbq(Stream p0, string p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("ln");
		}
		if (p1.Length == 0 || 1 == 0)
		{
			return;
		}
		int num = (p1[0] - 32) & 0x3F;
		if (p1.Length == 1 && (num == 0 || 1 == 0))
		{
			return;
		}
		int num2 = 1;
		if (num2 == 0)
		{
			goto IL_0052;
		}
		goto IL_0060;
		IL_0052:
		num = pajvu(p0, p1, num2, num);
		num2 += 4;
		goto IL_0060;
		IL_0060:
		if (num2 >= p1.Length)
		{
			if (num != 0 && 0 == 0)
			{
				throw new MailException(brgjd.edcru("Invalid UUEncode line '{0}'. Declared data length not encoded.", p1));
			}
			return;
		}
		goto IL_0052;
	}

	private static int pajvu(Stream p0, string p1, int p2, int p3)
	{
		if (p3 == 0 || 1 == 0)
		{
			return 0;
		}
		if (p1.Length < p2 + 4)
		{
			throw new MailException(brgjd.edcru("Invalid UUEncode line '{0}'. Data length is not a multiple of 4.", p1));
		}
		byte[] array = new byte[4];
		byte[] array2 = new byte[3];
		int num = 0;
		if (num != 0)
		{
			goto IL_004a;
		}
		goto IL_0061;
		IL_004a:
		array[num] = (byte)((p1[p2 + num] - 32) & 0x3F);
		num++;
		goto IL_0061;
		IL_0061:
		if (num >= 4)
		{
			array2[0] = (byte)((array[0] << 2) | (array[1] >> 4));
			array2[1] = (byte)((array[1] << 4) | (array[2] >> 2));
			array2[2] = (byte)((array[2] << 6) | array[3]);
			if (p3 > 3)
			{
				p0.Write(array2, 0, 3);
				return p3 - 3;
			}
			p0.Write(array2, 0, p3);
			return 0;
		}
		goto IL_004a;
	}
}
