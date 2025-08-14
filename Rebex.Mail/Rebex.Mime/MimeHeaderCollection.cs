using System;
using System.Collections;
using System.Collections.Generic;
using Rebex.Mime.Headers;
using onrkn;

namespace Rebex.Mime;

public class MimeHeaderCollection : IEnumerable<MimeHeader>, IEnumerable
{
	private readonly ArrayList wfzdr;

	private long hfhzo;

	private bool ijdcv;

	internal long fehza => hfhzo;

	internal bool kldya
	{
		get
		{
			return ijdcv;
		}
		set
		{
			ijdcv = value;
			IEnumerator enumerator = wfzdr.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					MimeHeader mimeHeader = (MimeHeader)enumerator.Current;
					mimeHeader.hmmsd = value;
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
	}

	public object SyncRoot => wfzdr.SyncRoot;

	public int Count => wfzdr.Count;

	public MimeHeader this[int index]
	{
		get
		{
			return (MimeHeader)wfzdr[index];
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			if (ijdcv && 0 == 0)
			{
				throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
			}
			wfzdr[index] = value;
			hfhzo++;
		}
	}

	public MimeHeader this[string headerName]
	{
		get
		{
			uuhcl(headerName);
			int num = jreii(headerName);
			if (num < 0)
			{
				return null;
			}
			return (MimeHeader)wfzdr[num];
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			uuhcl(headerName);
			if (ijdcv && 0 == 0)
			{
				throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
			}
			int num = jreii(headerName);
			if (num >= 0)
			{
				cbhgh(headerName, num);
			}
			if (num >= 0)
			{
				wfzdr[num] = value;
			}
			else
			{
				wfzdr.Add(value);
			}
			hfhzo++;
		}
	}

	internal MimeHeaderCollection()
	{
		wfzdr = new ArrayList();
	}

	internal MimeHeader[] hafss()
	{
		return (MimeHeader[])wfzdr.ToArray(typeof(MimeHeader));
	}

	private static void uuhcl(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("name");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Header name is empty.");
		}
	}

	private int cbhgh(string p0, int p1)
	{
		int num = 0;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0008;
		}
		goto IL_004d;
		IL_0008:
		if (num2 != p1)
		{
			MimeHeader mimeHeader = (MimeHeader)wfzdr[num2];
			if (string.Compare(mimeHeader.Name, p0, StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
			{
				wfzdr.RemoveAt(num2);
				num++;
				num2--;
			}
		}
		num2++;
		goto IL_004d;
		IL_004d:
		if (num2 < wfzdr.Count)
		{
			goto IL_0008;
		}
		return num;
	}

	internal int jreii(string p0)
	{
		uuhcl(p0);
		int num = 0;
		if (num != 0)
		{
			goto IL_0008;
		}
		goto IL_0037;
		IL_0008:
		MimeHeader mimeHeader = (MimeHeader)wfzdr[num];
		if (string.Compare(p0, mimeHeader.Name, StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
		{
			return num;
		}
		num++;
		goto IL_0037;
		IL_0037:
		if (num < wfzdr.Count)
		{
			goto IL_0008;
		}
		return -1;
	}

	internal IHeader etuur(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("name");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_001b;
		}
		goto IL_004f;
		IL_004f:
		if (num < wfzdr.Count)
		{
			goto IL_001b;
		}
		return null;
		IL_001b:
		MimeHeader mimeHeader = (MimeHeader)wfzdr[num];
		if (string.Compare(mimeHeader.Name, p0, StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
		{
			return mimeHeader.Value;
		}
		num++;
		goto IL_004f;
	}

	public IEnumerator GetEnumerator()
	{
		return wfzdr.GetEnumerator();
	}

	private IEnumerator<MimeHeader> dbggz()
	{
		return this.eaqmu<MimeHeader>().GetEnumerator();
	}

	IEnumerator<MimeHeader> IEnumerable<MimeHeader>.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in dbggz
		return this.dbggz();
	}

	public void Insert(int index, MimeHeader header)
	{
		if (header == null || 1 == 0)
		{
			throw new ArgumentNullException("header");
		}
		if (ijdcv && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		wfzdr.Insert(index, header);
		hfhzo++;
	}

	public void Insert(int index, string headerName, string headerValue)
	{
		if (headerName == null || 1 == 0)
		{
			throw new ArgumentNullException("headerName");
		}
		if (headerValue == null || 1 == 0)
		{
			throw new ArgumentNullException("headerValue");
		}
		if (ijdcv && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		wfzdr.Insert(index, new MimeHeader(headerName, headerValue));
		hfhzo++;
	}

	public int Add(MimeHeader header)
	{
		if (header == null || 1 == 0)
		{
			throw new ArgumentNullException("header");
		}
		if (ijdcv && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		int result = wfzdr.Add(header);
		hfhzo++;
		return result;
	}

	public int Add(string headerName, string headerValue)
	{
		uuhcl(headerName);
		if (headerValue == null || 1 == 0)
		{
			throw new ArgumentNullException("headerValue");
		}
		if (ijdcv && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		int result = wfzdr.Add(new MimeHeader(headerName, headerValue));
		hfhzo++;
		return result;
	}

	public int Remove(string name)
	{
		uuhcl(name);
		if (ijdcv && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		int result = cbhgh(name, -1);
		hfhzo++;
		return result;
	}

	public void RemoveAt(int index)
	{
		if (ijdcv && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		wfzdr.RemoveAt(index);
		hfhzo++;
	}

	public void Clear()
	{
		if (ijdcv && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		wfzdr.Clear();
		hfhzo++;
	}

	public void Remove(MimeHeader header)
	{
		if (header == null || 1 == 0)
		{
			throw new ArgumentNullException("header");
		}
		if (ijdcv && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		wfzdr.Remove(header);
		hfhzo++;
	}

	public string GetRaw(string headerName)
	{
		MimeHeader mimeHeader = this[headerName];
		if (mimeHeader == null || 1 == 0)
		{
			return null;
		}
		return mimeHeader.Raw;
	}

	public MimeHeader[] GetAllHeaders(string headerName)
	{
		uuhcl(headerName);
		List<MimeHeader> list = new List<MimeHeader>();
		int num = 0;
		if (num != 0)
		{
			goto IL_0012;
		}
		goto IL_0046;
		IL_0012:
		MimeHeader mimeHeader = (MimeHeader)wfzdr[num];
		if (string.Compare(mimeHeader.Name, headerName, StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
		{
			list.Add(mimeHeader);
		}
		num++;
		goto IL_0046;
		IL_0046:
		if (num < wfzdr.Count)
		{
			goto IL_0012;
		}
		return list.ToArray();
	}

	public string[] GetAllHeaderValuesRaw(string headerName)
	{
		MimeHeader[] allHeaders = GetAllHeaders(headerName);
		string[] array = new string[allHeaders.Length];
		int num = 0;
		if (num != 0)
		{
			goto IL_0017;
		}
		goto IL_0026;
		IL_0017:
		array[num] = allHeaders[num].Raw;
		num++;
		goto IL_0026;
		IL_0026:
		if (num < allHeaders.Length)
		{
			goto IL_0017;
		}
		return array;
	}

	internal void tyaam(string p0, IHeader p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("headerName");
		}
		if (ijdcv && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		if (p1 == null || 1 == 0)
		{
			Remove(p0);
			return;
		}
		int num = jreii(p0);
		MimeHeader value = ((num < 0) ? new MimeHeader(p0, p1, canonize: true) : new MimeHeader(this[num].Name, p1, canonize: false));
		if (num >= 0)
		{
			cbhgh(p0, num);
			wfzdr[num] = value;
		}
		else
		{
			wfzdr.Add(value);
		}
		hfhzo++;
	}
}
