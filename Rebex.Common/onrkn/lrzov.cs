using System;
using System.IO;
using System.Runtime.InteropServices;
using Rebex.IO;

namespace onrkn;

internal class lrzov : shdfo
{
	private class cvppb : xwtwn, wpvpu
	{
		private rayhl elqeg;

		public cvppb(rayhl handle)
		{
			elqeg = handle;
		}

		public void rclzx(vgycx p0)
		{
		}

		public vgycx hvaly()
		{
			if (owrbn(elqeg.lwfpq) && 0 == 0)
			{
				return null;
			}
			do
			{
				if (elqeg.rvcgt && 0 == 0)
				{
					if (!dvfml.FindNextFile(elqeg.lwfpq, ref elqeg.jmpox) || 1 == 0)
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						if (lastWin32Error != 18)
						{
							throw wsiuh();
						}
						elqeg.hmbip();
						return null;
					}
				}
				else
				{
					elqeg.rvcgt = true;
				}
			}
			while (((elqeg.jmpox.kkcvj & 0x1000) != 0) ? true : false);
			return hlrvz(ref elqeg.jmpox, null);
		}

		public void epmxq(bool p0)
		{
			elqeg.hmbip();
		}
	}

	private class rayhl
	{
		public IntPtr lwfpq;

		public dvfml.ggvit jmpox;

		public bool rvcgt;

		public void hmbip()
		{
			waxdf(p0: true);
		}

		private void waxdf(bool p0)
		{
			if (!owrbn(lwfpq) || 1 == 0)
			{
				dvfml.FindClose(lwfpq);
			}
			if (p0 && 0 == 0)
			{
				lwfpq = IntPtr.Zero;
				GC.SuppressFinalize(this);
			}
		}

		~rayhl()
		{
			waxdf(p0: false);
		}
	}

	private class ckfmh : nhovq
	{
		private readonly IntPtr wqtys;

		public IntPtr giazl => wqtys;

		public ckfmh(IntPtr handle, hqxly access)
			: base(access)
		{
			wqtys = handle;
		}

		public override void epmxq(bool p0)
		{
			dvfml.CloseHandle(wqtys);
		}

		public override int mlhqn(byte[] p0, int p1, int p2)
		{
			dahxy.dionp(p0, p1, p2);
			vexdc();
			GCHandle gCHandle = GCHandle.Alloc(p0, GCHandleType.Pinned);
			bool flag;
			uint lpNumberOfBytesRead;
			try
			{
				IntPtr lpBuffer = new IntPtr(gCHandle.AddrOfPinnedObject().ToInt64() + p1);
				flag = dvfml.ReadFile(wqtys, lpBuffer, (uint)p2, out lpNumberOfBytesRead, IntPtr.Zero);
			}
			finally
			{
				gCHandle.Free();
			}
			if (!flag || 1 == 0)
			{
				throw wsiuh();
			}
			return (int)lpNumberOfBytesRead;
		}

		public override void eesbc(byte[] p0, int p1, int p2)
		{
			dahxy.dionp(p0, p1, p2);
			xhonm();
			GCHandle gCHandle = GCHandle.Alloc(p0, GCHandleType.Pinned);
			bool flag;
			uint lpNumberOfBytesWritten;
			try
			{
				IntPtr lpBuffer = new IntPtr(gCHandle.AddrOfPinnedObject().ToInt64() + p1);
				flag = dvfml.WriteFile(wqtys, lpBuffer, (uint)p2, out lpNumberOfBytesWritten, IntPtr.Zero);
			}
			finally
			{
				gCHandle.Free();
			}
			if (!flag || 1 == 0)
			{
				throw wsiuh();
			}
			if (p2 == (int)lpNumberOfBytesWritten)
			{
				return;
			}
			throw hrfcx(xhetu.scesf);
		}

		public override long qgsek(vnfav p0, long p1)
		{
			if (!dvfml.umllv(wqtys, p1, out var p2, p0) || 1 == 0)
			{
				throw wsiuh();
			}
			return p2;
		}

		public override void rclzx(vgycx p0)
		{
			if (p0 == null || 1 == 0)
			{
				throw new ArgumentNullException("info");
			}
			if ((p0.snggt & xilhs.krccs) != 0 && 0 == 0)
			{
				if (!dvfml.umllv(wqtys, 0L, out var p1, vnfav.vohwg) || 1 == 0)
				{
					throw wsiuh();
				}
				long vtrzr = p0.vtrzr;
				if (p1 != vtrzr && (!dvfml.umllv(wqtys, vtrzr, out var p2, vnfav.efnoq) || 1 == 0))
				{
					throw wsiuh();
				}
				if (!dvfml.SetEndOfFile(wqtys) || 1 == 0)
				{
					throw wsiuh();
				}
				if (p1 != vtrzr)
				{
					bool flag = ((p1 >= vtrzr) ? dvfml.umllv(wqtys, 0L, out p2, vnfav.okkae) : dvfml.umllv(wqtys, p1, out p2, vnfav.efnoq));
					if (!flag || 1 == 0)
					{
						throw wsiuh();
					}
				}
			}
			dvfml.toapj[] array = (((p0.snggt & xilhs.fistc) == 0) ? null : new dvfml.toapj[1] { kkaga(p0.kkxpy) });
			dvfml.toapj[] array2 = (((p0.snggt & xilhs.bhwst) == 0) ? null : new dvfml.toapj[1] { kkaga(p0.xwfgd) });
			dvfml.toapj[] array3 = (((p0.snggt & xilhs.prxbn) == 0) ? null : new dvfml.toapj[1] { kkaga(p0.shsrc) });
			if ((array != null || array2 != null || array3 != null) && (!dvfml.SetFileTime(wqtys, array, array2, array3) || 1 == 0))
			{
				throw wsiuh();
			}
		}

		public override void mreei()
		{
			if (!dvfml.FlushFileBuffers(wqtys) || 1 == 0)
			{
				throw wsiuh();
			}
		}
	}

	private static readonly DateTime rbvpj = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	private static readonly evdac fnupt = new evdac('\\', '/', ':', caseSensitive: false, supportsFileSync: true, dahxy.mnelc());

	private readonly uint dnzva;

	public lrzov()
		: this(FileShare.Read)
	{
	}

	public lrzov(FileShare share)
	{
		dnzva = (uint)(share & FileShare.ReadWrite);
	}

	private static bool owrbn(IntPtr p0)
	{
		long num = p0.ToInt64();
		if (num == 0 || num == -1)
		{
			return true;
		}
		return false;
	}

	private static dvfml.toapj kkaga(DateTime p0)
	{
		long num = p0.ToUniversalTime().Ticks - rbvpj.Ticks;
		if (num < 0)
		{
			num = 0L;
		}
		return new dvfml.toapj
		{
			gkzuh = (uint)(num >> 32),
			dyffe = (uint)(num & 0xFFFFFFFFu)
		};
	}

	private static DateTime pvcpk(dvfml.toapj p0)
	{
		long num = p0.gkzuh;
		num = (num << 32) + p0.dyffe;
		return rbvpj.AddTicks(num);
	}

	private static vgycx xpzub(ref dvfml.txocr p0, string p1)
	{
		DateTime value = pvcpk(p0.lljru);
		DateTime value2 = pvcpk(p0.esldj);
		DateTime value3 = pvcpk(p0.xpgro);
		long num = p0.qpdga;
		num = (num << 32) + p0.odbve;
		ItemType itemType;
		if ((p0.lhgzr & 0x40) != 0 && 0 == 0)
		{
			itemType = ItemType.Device;
			if (itemType != ItemType.File)
			{
				goto IL_0074;
			}
		}
		if ((p0.lhgzr & 0x10) != 0 && 0 == 0)
		{
			itemType = ItemType.Directory;
			if (itemType != ItemType.File)
			{
				goto IL_0074;
			}
		}
		itemType = ItemType.File;
		goto IL_0074;
		IL_0074:
		ixkhy value4 = (ixkhy)(0x627 & p0.lhgzr);
		return new vgycx(itemType, value4, p1, num, value, value2, value3);
	}

	private static vgycx hlrvz(ref dvfml.ggvit p0, string p1)
	{
		DateTime value = pvcpk(p0.oxyjs);
		DateTime value2 = pvcpk(p0.ibxwp);
		DateTime value3 = pvcpk(p0.bildo);
		long num = p0.kqzax;
		num = (num << 32) + p0.qbeak;
		ItemType itemType;
		if ((p0.kkcvj & 0x40) != 0 && 0 == 0)
		{
			itemType = ItemType.Device;
			if (itemType != ItemType.File)
			{
				goto IL_0074;
			}
		}
		if ((p0.kkcvj & 0x10) != 0 && 0 == 0)
		{
			itemType = ItemType.Directory;
			if (itemType != ItemType.File)
			{
				goto IL_0074;
			}
		}
		itemType = ItemType.File;
		goto IL_0074;
		IL_0074:
		if (p1 == null || 1 == 0)
		{
			p1 = p0.hhxbq;
		}
		ixkhy value4 = (ixkhy)(0x627 & p0.kkcvj);
		return new vgycx(itemType, value4, p1, num, value, value2, value3);
	}

	private string ezkjs(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		if (p0.IndexOfAny(fnupt.jqvwt()) >= 0)
		{
			throw new nfcev(fvjcl.vmbsn, "Path contains invalid characters.");
		}
		p0 = p0.Replace('/', '\\');
		return p0;
	}

	public override xwtwn ellsg(string p0, string p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		p0 = ezkjs(p0);
		if (p1 == null || false || p1.Length == 0 || 1 == 0)
		{
			p1 = "*";
		}
		if (!p0.EndsWith("\\") || 1 == 0)
		{
			p0 += '\\';
		}
		string lpFileName = p0;
		p0 += p1;
		rayhl rayhl = new rayhl();
		rayhl.lwfpq = dvfml.FindFirstFile(p0, ref rayhl.jmpox);
		if (rayhl.lwfpq == dvfml.zhchs && 0 == 0)
		{
			int num = Marshal.GetLastWin32Error();
			if ((num == 2 || num == 18) && (!dvfml.GetFileAttributesEx(lpFileName, dvfml.vlgfn.mbqzs, out var _) || 1 == 0))
			{
				Marshal.GetLastWin32Error();
				num = 3;
			}
			if (num != 2 && num != 18)
			{
				throw hrfcx((xhetu)num);
			}
			rayhl.lwfpq = IntPtr.Zero;
		}
		return new cvppb(rayhl);
	}

	public override vgycx wxxmm(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		p0 = ezkjs(p0);
		if (!dvfml.GetFileAttributesEx(p0, dvfml.vlgfn.mbqzs, out var lpFileInformation) || 1 == 0)
		{
			switch (Marshal.GetLastWin32Error())
			{
			case 2:
			case 3:
			case 32:
				return null;
			default:
				throw wsiuh();
			}
		}
		int num = p0.LastIndexOf('\\');
		string p1 = p0.Substring(num + 1);
		return xpzub(ref lpFileInformation, p1);
	}

	public override eyqzi nxqay(string p0, rdvij p1, hqxly p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		shdfo.amrzo(p1, p2);
		p0 = ezkjs(p0);
		return keeyv(p0, p1, p2);
	}

	private ckfmh keeyv(string p0, rdvij p1, hqxly p2)
	{
		uint num = 0u;
		if ((p2 & hqxly.xpmbl) != 0 && 0 == 0)
		{
			num |= 0x80000000u;
		}
		if ((p2 & hqxly.oigov) != 0 && 0 == 0)
		{
			num |= 0x40000000;
		}
		IntPtr intPtr = dvfml.CreateFile(p0, num, dnzva, IntPtr.Zero, (uint)p1, 128u, IntPtr.Zero);
		if (owrbn(intPtr) && 0 == 0)
		{
			throw wsiuh();
		}
		return new ckfmh(intPtr, p2);
	}

	public override void kosgg(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		p0 = ezkjs(p0);
		if (!dvfml.CreateDirectory(p0, IntPtr.Zero) || 1 == 0)
		{
			throw wsiuh();
		}
	}

	public override void tkipb(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		p0 = ezkjs(p0);
		if (!dvfml.RemoveDirectory(p0) || 1 == 0)
		{
			xhetu xhetu2 = (xhetu)Marshal.GetLastWin32Error();
			if (xhetu2 == xhetu.xzcxw)
			{
				xhetu2 = xhetu.uyfbm;
			}
			throw hrfcx(xhetu2);
		}
	}

	public override void xptlv(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		p0 = ezkjs(p0);
		if (dvfml.DeleteFile(p0) ? true : false)
		{
			return;
		}
		xhetu lastWin32Error = (xhetu)Marshal.GetLastWin32Error();
		if (lastWin32Error == xhetu.yyddi || lastWin32Error == (xhetu)0 || 1 == 0)
		{
			OperatingSystem oSVersion = Environment.OSVersion;
			if (oSVersion.Platform == PlatformID.WinCE && oSVersion.Version.Major <= 6 && (wxxmm(p0) == null || 1 == 0))
			{
				throw hrfcx(xhetu.dpbiz);
			}
		}
		throw hrfcx(lastWin32Error);
	}

	public override void bwzap(string p0, vgycx p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("info");
		}
		p0 = ezkjs(p0);
		if ((p1.snggt & (xilhs.krccs | xilhs.fistc | xilhs.prxbn | xilhs.bhwst)) == 0 || 1 == 0)
		{
			return;
		}
		if (!dvfml.GetFileAttributesEx(p0, dvfml.vlgfn.mbqzs, out var lpFileInformation) || 1 == 0)
		{
			throw wsiuh();
		}
		if ((lpFileInformation.lhgzr & 0x10) != 0)
		{
			return;
		}
		ckfmh ckfmh = keeyv(p0, rdvij.hujjg, hqxly.oigov);
		try
		{
			ckfmh.rclzx(p1);
		}
		finally
		{
			ckfmh.epmxq(p0: true);
		}
	}

	public override void jcpei(string p0, string p1)
	{
		edncs(p0: false, p0, p1);
	}

	public override void idbxb(string p0, string p1)
	{
		edncs(p0: true, p0, p1);
	}

	private void edncs(bool p0, string p1, string p2)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("sourcePath");
		}
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("targetPath");
		}
		p1 = ezkjs(p1);
		p2 = ezkjs(p2);
		dvfml.txocr lpFileInformation;
		if (!p0 || 1 == 0)
		{
			if (!dvfml.GetFileAttributesEx(p1, dvfml.vlgfn.mbqzs, out lpFileInformation) || 1 == 0)
			{
				throw wsiuh();
			}
			if ((lpFileInformation.lhgzr & 0x10) != 0 && 0 == 0)
			{
				throw new NotSupportedException("COPY is not supported for directories.");
			}
		}
		if (dvfml.GetFileAttributesEx(p2, dvfml.vlgfn.mbqzs, out lpFileInformation) && 0 == 0)
		{
			throw hrfcx(xhetu.nossb);
		}
		switch (Marshal.GetLastWin32Error())
		{
		default:
			throw wsiuh();
		case 2:
		case 3:
		{
			int length = p2.LastIndexOf('\\');
			string p3 = p2.Substring(0, length);
			p3 = hzedv(p3);
			if (dvfml.GetFileAttributesEx(p3, dvfml.vlgfn.mbqzs, out lpFileInformation))
			{
				if ((lpFileInformation.lhgzr & 0x10) == 0 || 1 == 0)
				{
					throw hrfcx(xhetu.tgdky);
				}
				if (p0 && 0 == 0)
				{
					if (!dvfml.MoveFile(p1, p2) || 1 == 0)
					{
						throw wsiuh();
					}
				}
				else if (!dvfml.CopyFileEx(p1, p2, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0) || 1 == 0)
				{
					throw wsiuh();
				}
				break;
			}
			throw wsiuh();
		}
		}
	}

	private string hzedv(string p0)
	{
		if (p0[p0.Length - 1] != fnupt.dhpem)
		{
			return p0;
		}
		return p0 + fnupt.xwsda;
	}

	private static Exception wsiuh()
	{
		return hrfcx((xhetu)Marshal.GetLastWin32Error());
	}

	private static Exception hrfcx(xhetu p0)
	{
		return qbdpo(p0, null);
	}

	private static Exception qbdpo(xhetu p0, Exception p1)
	{
		fvjcl p2;
		string message = govoi(p0, out p2);
		return new nfcev(p2, (int)p0, message, p1);
	}

	private static string govoi(xhetu p0, out fvjcl p1)
	{
		switch (p0)
		{
		case (xhetu)0:
			p1 = fvjcl.sfwgm;
			return "Unable to determine error.";
		case xhetu.uyfbm:
			p1 = fvjcl.jlxwg;
			break;
		case xhetu.nossb:
			p1 = fvjcl.nadim;
			break;
		case xhetu.gpiol:
			p1 = fvjcl.xwbjx;
			break;
		case xhetu.axiqp:
			p1 = fvjcl.nadim;
			break;
		case xhetu.dpbiz:
			p1 = fvjcl.ockud;
			break;
		case xhetu.stvtv:
			p1 = fvjcl.xwbjx;
			break;
		case xhetu.dhcss:
			p1 = fvjcl.latcf;
			break;
		case xhetu.ckswt:
			p1 = fvjcl.cdaaf;
			break;
		case xhetu.tgdky:
			p1 = fvjcl.nzuue;
			break;
		case xhetu.yrodb:
			p1 = fvjcl.ovavv;
			break;
		case xhetu.xzcxw:
			p1 = fvjcl.xjlzv;
			break;
		case xhetu.qlios:
			p1 = fvjcl.psrbn;
			break;
		case xhetu.scesf:
			p1 = fvjcl.hznby;
			break;
		case xhetu.fxnku:
			p1 = fvjcl.ywelb;
			break;
		case xhetu.qqmyl:
			p1 = fvjcl.vmbsn;
			break;
		case xhetu.sthjb:
			p1 = fvjcl.vmbsn;
			break;
		case xhetu.jmdii:
			p1 = fvjcl.vmbsn;
			return "Path is too long.";
		default:
			p1 = fvjcl.sfwgm;
			break;
		}
		return shdfo.dprqm(p1);
	}

	public override evdac tunjy()
	{
		return fnupt;
	}
}
