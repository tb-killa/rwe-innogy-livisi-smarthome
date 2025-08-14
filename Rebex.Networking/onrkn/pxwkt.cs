using System;
using System.Runtime.InteropServices;

namespace onrkn;

internal class pxwkt
{
	[Flags]
	public enum vxvhw
	{
		vtpxr = 0,
		zcnvn = 1,
		krqkz = 2,
		qtoym = 4,
		saver = 0x10,
		wvvlp = 0x20,
		arlkn = 0x40
	}

	[Flags]
	public enum uimfo
	{
		vsshw = 0,
		kdzby = 1,
		rmxvc = 2,
		gzske = 4,
		icqrv = 8
	}

	public enum woide
	{
		spsov = 0,
		wiqjw = 32768,
		vewva = 512,
		epfwv = 8
	}

	public enum wfvjt
	{
		rzrie = 0,
		viqkc = 16,
		cgwia = 17,
		loves = 32,
		qhwyv = 33,
		obprh = 34,
		ltbvs = 35,
		aqgjt = 36,
		kfojz = 37,
		csmia = 38,
		bhnbl = 39,
		ehihg = 40,
		qsspy = 41,
		oecul = 42,
		zbnfy = 43,
		fpefw = 44,
		wtvpc = 64,
		bhchk = 65,
		hjaga = 66,
		okqhg = 128,
		jpnfg = 129
	}

	[Flags]
	public enum ldbtl
	{
		szsdt = 0,
		cchsg = 1,
		bdspc = 2,
		rfsai = 4,
		mhuba = 8
	}

	public struct vyqyo
	{
		public int wdjzn;

		public uimfo dsvwl;

		public ldbtl qnfzr;

		public woide lojav;

		public int apsvy;

		public int nwgia;

		public Guid htfes;

		public IntPtr xlkvi;

		public uint bitok;

		public int ghqfl;

		public uint eqtsb;

		public uint yiaps;

		public uint sfrqh;
	}

	private const uint soduy = 0u;

	public const int lenml = 1;

	public const int vbwrw = 0;

	public static readonly Guid yhvjh = new Guid("436EF144-B4FB-4863-A041-8F905A62C572");

	[DllImport("CellCore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern int ConnMgrMapURL(string url, ref Guid ppGuid, int pdwIndex);

	[DllImport("CellCore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern int ConnMgrEstablishConnection(ref vyqyo pConnInfo, ref IntPtr phConnection);

	[DllImport("CellCore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern int ConnMgrEstablishConnectionSync(ref vyqyo pConnInfo, ref IntPtr phConnection, uint dwTimeout, ref wfvjt pdwStatus);

	[DllImport("CellCore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern int ConnMgrReleaseConnection(IntPtr phConnection, int cache);

	[DllImport("CellCore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern int ConnMgrConnectionStatus(IntPtr phConnection, ref wfvjt status);

	[DllImport("wininet.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool InternetGetConnectedState(out vxvhw lpdwFlags, int dwReserved);

	public static bool awbrj(int p0)
	{
		return (long)p0 != 0;
	}
}
