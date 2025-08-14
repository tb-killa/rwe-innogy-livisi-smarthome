using System;
using System.ComponentModel;
using System.IO;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class ContentInfo
{
	private readonly ObjectIdentifier pgzwx;

	private readonly aipxl dbziy;

	private byte[] zszdl;

	public ObjectIdentifier ContentType => pgzwx;

	[Obsolete("This property has been deprecated. Please use ToArray() method instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public byte[] Content
	{
		get
		{
			if (zszdl == null || 1 == 0)
			{
				zszdl = dbziy.pcjxs();
			}
			return zszdl;
		}
	}

	public ContentInfo(byte[] content)
		: this(new ObjectIdentifier("1.2.840.113549.1.7.1"), content)
	{
	}

	public ContentInfo(ObjectIdentifier contentType, byte[] content)
	{
		if (contentType == null || 1 == 0)
		{
			throw new ArgumentNullException("contentType");
		}
		if (content == null || 1 == 0)
		{
			throw new ArgumentNullException("content");
		}
		pgzwx = contentType;
		dbziy = new aipxl(content);
	}

	internal ContentInfo(ObjectIdentifier contentType, aipxl content)
	{
		if (contentType == null || 1 == 0)
		{
			throw new ArgumentNullException("contentType");
		}
		if (content == null || 1 == 0)
		{
			throw new ArgumentNullException("content");
		}
		pgzwx = contentType;
		dbziy = content;
	}

	internal ContentInfo(opjbe content)
	{
		pgzwx = new ObjectIdentifier("1.2.840.113549.1.7.1");
		dbziy = new aipxl(content);
	}

	internal ContentInfo()
	{
		pgzwx = new ObjectIdentifier("1.2.840.113549.1.7.1");
		dbziy = new aipxl(new byte[0]);
	}

	public byte[] ToArray()
	{
		return dbziy.pcjxs();
	}

	public void CopyTo(Stream output)
	{
		dbziy.siums(output);
	}

	internal aviir<byte> fugju()
	{
		zszdl = null;
		return dbziy.rzqgs();
	}

	internal aipxl jphgq()
	{
		zszdl = null;
		return dbziy;
	}

	internal void gfzrv(Action<byte[], int> p0)
	{
		zszdl = null;
		dbziy.xfmlk(p0);
	}

	public Stream ToStream()
	{
		zszdl = null;
		return dbziy.ymxwm();
	}
}
