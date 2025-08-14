using System.IO;
using System.Text;

namespace onrkn;

internal class tndeg : MemoryStream
{
	private Encoding wmiom;

	public Encoding sdfgh
	{
		get
		{
			return wmiom;
		}
		set
		{
			wmiom = value;
		}
	}

	public tndeg(Encoding encoding)
	{
		wmiom = encoding;
	}
}
