using System;
using System.ComponentModel;
using onrkn;

namespace Rebex.Net;

[EditorBrowsable(EditorBrowsableState.Never)]
[wptwl(false)]
[Obsolete("TlsDebug event API has been deprecated and will be removed. Use LogWriter instead.", false)]
public class TlsDebugEventArgs : EventArgs
{
	private TlsDebugEventType acqac;

	private TlsDebugEventSource ucbvm;

	private TlsDebugLevel bmkst;

	private byte[] zygvi;

	public TlsDebugEventType Type => acqac;

	public TlsDebugEventGroup Group => (TlsDebugEventGroup)((int)acqac >> 16);

	public TlsDebugEventSource Source => ucbvm;

	public TlsDebugLevel Level => bmkst;

	public byte[] GetRawData()
	{
		return zygvi;
	}

	public TlsDebugEventArgs(TlsDebugEventType type, TlsDebugEventSource source, TlsDebugLevel level)
	{
		acqac = type;
		ucbvm = source;
		bmkst = level;
		zygvi = null;
	}

	internal TlsDebugEventArgs(TlsDebugEventType type, TlsDebugEventSource source, TlsDebugLevel level, qoqui message)
	{
		acqac = type;
		ucbvm = source;
		bmkst = level;
		zygvi = message.szrqi();
	}

	public TlsDebugEventArgs(TlsDebugEventType type, TlsDebugEventSource source, TlsDebugLevel level, byte[] buffer, int offset, int count)
	{
		acqac = type;
		ucbvm = source;
		bmkst = level;
		zygvi = new byte[count];
		Array.Copy(buffer, offset, zygvi, 0, count);
	}
}
