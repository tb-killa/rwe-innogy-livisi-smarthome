using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using onrkn;

namespace Rebex.Security.Authentication;

public class SspiAuthentication : IDisposable
{
	internal struct faaro
	{
		public uint vuwsz;

		public ushort glexm;

		public ushort twlov;

		public uint ldvfi;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string wcvbg;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string dgifp;
	}

	private const int zpmsn = 1;

	private const int ktnkv = 2;

	private const int umsai = 10;

	private const uint ifpep = 0u;

	private const uint wajlm = 2148074252u;

	private const uint dchyg = 2148074243u;

	private const uint zppvx = 590610u;

	private const uint rhijz = 2148074245u;

	private const uint ujodz = 2148074253u;

	private const uint ejvxq = 2148074255u;

	private const uint krzjv = 2148074257u;

	private const uint mldsb = 2148074248u;

	private const uint xcwzx = 2148074254u;

	private const uint kfhcj = 3221225566u;

	private const uint zpdvr = 3221225578u;

	private const string mwlmo = "Error 0x{0:X} has occurred in SSPI interop.";

	private const uint uokrd = 0u;

	internal const string ixnja = "NTLM";

	internal const string jibut = "Kerberos";

	internal const string uofuo = "Negotiate";

	private static readonly Func<nxtme<string>> yeiaj;

	private static readonly Func<string, bool> nyfjs;

	private ezcqf uaxkh = new ezcqf();

	private nhlux ulflb = new nhlux();

	private string gkazj;

	private SspiRequirements btpyw;

	private SspiDataRepresentation oqvmn;

	private string rficx;

	private SspiCredentialUsage wrdnh;

	private static Func<string, bool> zzxlo;

	[DllImport("secur32.dll", SetLastError = true)]
	internal static extern uint AcquireCredentialsHandle(string principal, string moduleName, SspiCredentialUsage usage, IntPtr logonID, wyzct[] authData, IntPtr keyCallback, IntPtr keyArgument, IntPtr handle, out long timeStamp);

	[DllImport("secur32.dll", EntryPoint = "AcquireCredentialsHandle", SetLastError = true)]
	internal static extern uint AcquireCredentialsHandle2(string principal, string moduleName, SspiCredentialUsage usage, IntPtr logonID, IntPtr authData, IntPtr keyCallback, IntPtr keyArgument, IntPtr handle, out long timeStamp);

	[DllImport("secur32.dll", SetLastError = true)]
	internal static extern uint InitializeSecurityContext(IntPtr handle, IntPtr context, string targetName, SspiRequirements contextReq, int reserved1, SspiDataRepresentation targetDataRep, IntPtr input, int reserved2, IntPtr newContext, IntPtr output, out int contextAttr, out long timeStamp);

	[DllImport("secur32.dll", SetLastError = true)]
	internal static extern uint DecryptMessage(IntPtr context, IntPtr message, uint messageSeqNo, out int qop);

	[DllImport("secur32.dll", SetLastError = true)]
	internal static extern uint VerifySignature(IntPtr context, IntPtr message, uint messageSeqNo, out uint qop);

	[DllImport("secur32.dll", SetLastError = true)]
	internal static extern uint AcceptSecurityContext(IntPtr handle, IntPtr context, IntPtr input, SspiRequirements contextReq, SspiDataRepresentation targetDataRep, IntPtr newContext, IntPtr output, out int contextAttr, out long timeStamp);

	[DllImport("secur32.dll", SetLastError = true)]
	internal static extern uint EncryptMessage(IntPtr context, int qop, IntPtr message, uint messageSeqNo);

	[DllImport("secur32.dll", SetLastError = true)]
	internal static extern uint MakeSignature(IntPtr context, int qop, IntPtr message, uint messageSeqNo);

	[DllImport("secur32.dll", SetLastError = true)]
	internal static extern uint QueryContextAttributes(IntPtr context, uint ulAttribute, out hibmf buffer);

	[DllImport("secur32.dll", SetLastError = true)]
	internal static extern uint DeleteSecurityContext(IntPtr context);

	[DllImport("secur32.dll", SetLastError = true)]
	internal static extern uint FreeCredentialsHandle(IntPtr handle);

	[DllImport("secur32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	internal static extern uint EnumerateSecurityPackages(ref uint pcPackages, out IntPtr ppPackageInfo);

	[DllImport("secur32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	internal static extern uint FreeContextBuffer(IntPtr pvContextBuffer);

	static SspiAuthentication()
	{
		yeiaj = tzfjl;
		yeiaj = yeiaj.ryipk();
		if (zzxlo == null || 1 == 0)
		{
			zzxlo = lubri;
		}
		nyfjs = zzxlo;
		nyfjs = nyfjs.lwhpu();
	}

	private void aazbh(uint p0)
	{
		throw new SspiException(p0 switch
		{
			2148074243u => "The specified SSPI target is unknown or unreachable.", 
			2148074252u => "The SSPI logon attempt failed.", 
			3221225566u => "There are currently no logon servers available to service the logon request.", 
			2148074253u => brgjd.edcru("The credentials supplied to {0} security package were not recognized.", rficx), 
			2148074245u => brgjd.edcru("The {0} security package is not available.", rficx), 
			3221225578u => "Password is wrong.", 
			2148074257u => "No authority could be contacted for authentication.", 
			2148074248u => "Invalid token.", 
			2148074254u => brgjd.edcru("No credentials are available in the {0} security package.", rficx), 
			_ => brgjd.edcru("Error 0x{0:X} has occurred in SSPI interop.", p0), 
		}, (int)p0);
	}

	public SspiAuthentication(string package, SspiDataRepresentation dataRepresentation, string targetName, SspiRequirements requirements, string userName, string password, string userDomain)
		: this(package, SspiCredentialUsage.Outbound, dataRepresentation, targetName, requirements, userName, password, userDomain)
	{
	}

	public SspiAuthentication(string package, SspiCredentialUsage usage, SspiDataRepresentation dataRepresentation, string targetName, SspiRequirements requirements, string userName, string password, string userDomain)
	{
		rficx = package;
		wrdnh = usage;
		oqvmn = dataRepresentation;
		gkazj = targetName;
		btpyw = requirements;
		wyzct[] array;
		if (userName == null || false || userName.Length == 0 || 1 == 0)
		{
			array = null;
		}
		else
		{
			int num = userName.IndexOf('\\');
			if (num < 0)
			{
				num = userName.IndexOf('/');
			}
			if (num >= 0)
			{
				userDomain = userName.Substring(0, num);
				userName = userName.Substring(num + 1);
			}
			array = new wyzct[1]
			{
				new wyzct(userName, password, userDomain)
			};
		}
		long timeStamp;
		uint num2 = ((array != null) ? AcquireCredentialsHandle(null, package, usage, IntPtr.Zero, array, IntPtr.Zero, IntPtr.Zero, uaxkh.inyna(), out timeStamp) : AcquireCredentialsHandle2(null, package, usage, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, uaxkh.inyna(), out timeStamp));
		if (num2 != 0 && 0 == 0)
		{
			aazbh(num2);
		}
	}

	public void Dispose()
	{
		ulflb.Dispose();
		uaxkh.Dispose();
	}

	public byte[] GetNextMessage(byte[] challenge, out bool complete)
	{
		if (wrdnh == SspiCredentialUsage.Outbound)
		{
			return zwmks(challenge, out complete);
		}
		return kuscz(challenge, out complete);
	}

	private byte[] zwmks(byte[] p0, out bool p1)
	{
		samhn samhn = null;
		txmde txmde;
		if (p0 == null || 1 == 0)
		{
			txmde = txmde.qwrqa;
		}
		else
		{
			txmde = new txmde(1);
			txmde.gyeqw(0, 2, p0);
		}
		txmde txmde2 = txmde;
		try
		{
			samhn samhn2 = samhn;
			try
			{
				txmde txmde3 = new txmde(1);
				try
				{
					txmde3.gyeqw(0, 2, new byte[16384]);
					int contextAttr;
					long timeStamp;
					uint num = InitializeSecurityContext(context: (!ulflb.zuofh) ? ulflb.inyna() : IntPtr.Zero, handle: uaxkh.inyna(), targetName: gkazj, contextReq: btpyw, reserved1: 0, targetDataRep: oqvmn, input: txmde.rivma(), reserved2: 0, newContext: ulflb.inyna(), output: txmde3.rivma(), contextAttr: out contextAttr, timeStamp: out timeStamp);
					switch (num)
					{
					case 0u:
						p1 = true;
						break;
					case 590610u:
						p1 = false;
						break;
					default:
						p1 = true;
						aazbh(num);
						return null;
					}
					return txmde3.ogwcp(0);
				}
				finally
				{
					if (txmde3 != null && 0 == 0)
					{
						((IDisposable)txmde3).Dispose();
					}
				}
			}
			finally
			{
				if (samhn2 != null && 0 == 0)
				{
					((IDisposable)samhn2).Dispose();
				}
			}
		}
		finally
		{
			if (txmde2 != null && 0 == 0)
			{
				((IDisposable)txmde2).Dispose();
			}
		}
	}

	private byte[] kuscz(byte[] p0, out bool p1)
	{
		txmde txmde;
		if (p0 == null || 1 == 0)
		{
			txmde = txmde.qwrqa;
		}
		else
		{
			txmde = new txmde(1);
			txmde.gyeqw(0, 2, p0);
		}
		txmde txmde2 = txmde;
		try
		{
			txmde txmde3 = new txmde(1);
			try
			{
				txmde3.gyeqw(0, 2, new byte[16384]);
				int contextAttr;
				long timeStamp;
				uint num = AcceptSecurityContext(context: (!ulflb.zuofh) ? ulflb.inyna() : IntPtr.Zero, handle: uaxkh.inyna(), input: txmde.rivma(), contextReq: btpyw, targetDataRep: oqvmn, newContext: ulflb.inyna(), output: txmde3.rivma(), contextAttr: out contextAttr, timeStamp: out timeStamp);
				switch (num)
				{
				case 0u:
					p1 = true;
					break;
				case 590610u:
					p1 = false;
					break;
				default:
					p1 = true;
					aazbh(num);
					return null;
				}
				return txmde3.ogwcp(0);
			}
			finally
			{
				if (txmde3 != null && 0 == 0)
				{
					((IDisposable)txmde3).Dispose();
				}
			}
		}
		finally
		{
			if (txmde2 != null && 0 == 0)
			{
				((IDisposable)txmde2).Dispose();
			}
		}
	}

	public bool VerifySignature(byte[] signature, byte[] message)
	{
		if (!dahxy.xzevd || 1 == 0)
		{
			txmde txmde = new txmde(2);
			try
			{
				txmde.gyeqw(0, 2, signature);
				txmde.gyeqw(1, 1, message);
				uint qop;
				uint num = VerifySignature(ulflb.inyna(), txmde.rivma(), 0u, out qop);
				switch (num)
				{
				case 2148074255u:
					return false;
				case 0u:
					return true;
				default:
					aazbh(num);
					return false;
				}
			}
			finally
			{
				if (txmde != null && 0 == 0)
				{
					((IDisposable)txmde).Dispose();
				}
			}
		}
		throw new SspiException("SSPI authentication is not supported on non-Windows systems.", -1);
	}

	public byte[] MakeSignature(byte[] challenge)
	{
		if (!dahxy.xzevd || 1 == 0)
		{
			uint num = QueryContextAttributes(ulflb.inyna(), 0u, out var buffer);
			if (num != 0 && 0 == 0)
			{
				aazbh(num);
			}
			txmde txmde = new txmde(2);
			try
			{
				txmde.gyeqw(0, 1, challenge);
				txmde.gyeqw(1, 2, new byte[buffer.oqtxa]);
				num = MakeSignature(ulflb.inyna(), 0, txmde.rivma(), 0u);
				if (num != 0 && 0 == 0)
				{
					aazbh(num);
					return null;
				}
				return txmde.ogwcp(1);
			}
			finally
			{
				if (txmde != null && 0 == 0)
				{
					((IDisposable)txmde).Dispose();
				}
			}
		}
		throw new SspiException("SSPI authentication is not supported on non-Windows systems.", -1);
	}

	public byte[] Unwrap(byte[] challenge, out int qop)
	{
		if (!dahxy.xzevd || 1 == 0)
		{
			txmde txmde = new txmde(2);
			try
			{
				txmde.gyeqw(0, 1, null);
				txmde.gyeqw(1, 10, challenge);
				uint num = DecryptMessage(ulflb.inyna(), txmde.rivma(), 0u, out qop);
				if (num != 0 && 0 == 0)
				{
					aazbh(num);
				}
				return txmde.ogwcp(0);
			}
			finally
			{
				if (txmde != null && 0 == 0)
				{
					((IDisposable)txmde).Dispose();
				}
			}
		}
		throw new SspiException("SSPI authentication is not supported on non-Windows systems.", -1);
	}

	public byte[] Wrap(byte[] response, int qop, out bool complete)
	{
		if (!dahxy.xzevd || 1 == 0)
		{
			hibmf buffer;
			uint num = QueryContextAttributes(ulflb.inyna(), 0u, out buffer);
			if (num != 0 && 0 == 0)
			{
				aazbh(num);
			}
			int rqtxb = buffer.rqtxb;
			txmde txmde = new txmde(2);
			byte[] array;
			byte[] array2;
			try
			{
				txmde.gyeqw(0, 2, new byte[rqtxb]);
				txmde.gyeqw(1, 1, response);
				uint num2 = EncryptMessage(ulflb.inyna(), qop, txmde.rivma(), 0u);
				if (num2 != 0)
				{
					complete = true;
					aazbh(num2);
					return null;
				}
				complete = true;
				array = txmde.ogwcp(0);
				array2 = txmde.ogwcp(1);
			}
			finally
			{
				if (txmde != null && 0 == 0)
				{
					((IDisposable)txmde).Dispose();
				}
			}
			byte[] array3 = new byte[array.Length + array2.Length];
			array.CopyTo(array3, 0);
			array2.CopyTo(array3, array.Length);
			return array3;
		}
		throw new SspiException("SSPI authentication is not supported on non-Windows systems.", -1);
	}

	public static bool IsSupported(string package)
	{
		return nyfjs(package.hewdv());
	}

	private static nxtme<string> tzfjl()
	{
		try
		{
			return jnatp();
		}
		catch (TypeLoadException)
		{
		}
		return new string[0];
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static nxtme<string> jnatp()
	{
		uint pcPackages = 0u;
		IntPtr ppPackageInfo = IntPtr.Zero;
		try
		{
			uint num = EnumerateSecurityPackages(ref pcPackages, out ppPackageInfo);
			if (num != 0 && 0 == 0)
			{
				throw new SspiException("Error 0x{0:X} has occurred in SSPI interop.", (int)num);
			}
			Type typeFromHandle = typeof(faaro);
			int num2 = Marshal.SizeOf(typeFromHandle);
			string[] array = new string[pcPackages];
			IntPtr ptr = ppPackageInfo;
			uint num3 = 0u;
			if (num3 != 0)
			{
				goto IL_0055;
			}
			goto IL_008e;
			IL_008e:
			if (num3 >= pcPackages)
			{
				return array;
			}
			goto IL_0055;
			IL_0055:
			array[num3] = ((faaro)Marshal.PtrToStructure(ptr, typeFromHandle)).wcvbg.hewdv();
			ptr = new IntPtr(ptr.ToInt64() + num2);
			num3++;
			goto IL_008e;
		}
		finally
		{
			if (ppPackageInfo != IntPtr.Zero && 0 == 0)
			{
				uint num4 = FreeContextBuffer(ppPackageInfo);
				if (num4 != 0 && 0 == 0)
				{
					throw new SspiException("Error 0x{0:X} has occurred in SSPI interop.", (int)num4);
				}
			}
		}
	}

	private static bool lubri(string p0)
	{
		return yeiaj().arduu(p0);
	}
}
