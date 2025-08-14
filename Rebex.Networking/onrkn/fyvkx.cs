using System.Text;
using Rebex;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class fyvkx : mkuxt
{
	private string zsuha;

	private string lfoot;

	private string wcczf;

	private byte[] eydfa;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 50);
		mkuxt.excko(p0, zsuha);
		mkuxt.excko(p0, lfoot);
		mkuxt.excko(p0, wcczf);
		if (eydfa != null && 0 == 0)
		{
			mkuxt.lcbhj(p0, eydfa, p2: true);
		}
	}

	private fyvkx()
	{
	}

	public static fyvkx qzwyo(string p0, string p1, string[] p2)
	{
		fyvkx fyvkx2 = new fyvkx();
		fyvkx2.lfoot = p0;
		fyvkx2.zsuha = p1;
		fyvkx2.wcczf = "gssapi-with-mic";
		tndeg tndeg2 = new tndeg(EncodingTools.ASCII);
		mkuxt.ebmel(tndeg2, (uint)p2.Length);
		int num = 0;
		if (num != 0)
		{
			goto IL_003d;
		}
		goto IL_0079;
		IL_003d:
		string oid = p2[num];
		byte[] array = new ObjectIdentifier(oid).ToArray();
		mkuxt.ebmel(tndeg2, (uint)(array.Length + 2));
		mkuxt.agnqw(tndeg2, 6);
		mkuxt.agnqw(tndeg2, (byte)array.Length);
		mkuxt.lcbhj(tndeg2, array, p2: true);
		num++;
		goto IL_0079;
		IL_0079:
		if (num >= p2.Length)
		{
			fyvkx2.eydfa = tndeg2.ToArray();
			return fyvkx2;
		}
		goto IL_003d;
	}

	public fyvkx(string serviceName, string userName)
	{
		lfoot = serviceName;
		zsuha = userName;
		wcczf = "none";
	}

	public fyvkx(string serviceName, string userName, string password, Encoding encoding)
		: this(serviceName, userName)
	{
		wcczf = "password";
		tndeg tndeg2 = new tndeg(encoding);
		mkuxt.duaqa(tndeg2, p1: false);
		mkuxt.excko(tndeg2, password);
		eydfa = tndeg2.ToArray();
	}

	public fyvkx(string serviceName, string userName, string oldPassword, string newPassword, Encoding encoding)
		: this(serviceName, userName)
	{
		wcczf = "password";
		tndeg tndeg2 = new tndeg(encoding);
		mkuxt.duaqa(tndeg2, p1: true);
		mkuxt.excko(tndeg2, oldPassword);
		mkuxt.excko(tndeg2, newPassword);
		eydfa = tndeg2.ToArray();
	}

	public fyvkx(string serviceName, string userName, string[] submethods)
		: this(serviceName, userName)
	{
		wcczf = "keyboard-interactive";
		tndeg tndeg2 = new tndeg(EncodingTools.ASCII);
		mkuxt.excko(tndeg2, "");
		mkuxt.ijaon(tndeg2, submethods);
		eydfa = tndeg2.ToArray();
	}

	public fyvkx(string serviceName, string userName, byte[] sessionId, SshPrivateKey privateKey, string keyAlgorithmId, bool withSignature, bool padSignature)
		: this(serviceName, userName)
	{
		wcczf = "publickey";
		tndeg tndeg2 = new tndeg(EncodingTools.ASCII);
		mkuxt.duaqa(tndeg2, withSignature);
		mkuxt.excko(tndeg2, keyAlgorithmId);
		mkuxt.lcbhj(tndeg2, privateKey.cgplv(keyAlgorithmId), p2: false);
		if (withSignature && 0 == 0)
		{
			tndeg tndeg3 = new tndeg(EncodingTools.ASCII);
			mkuxt.lcbhj(tndeg3, sessionId, p2: false);
			mkuxt.agnqw(tndeg3, 50);
			mkuxt.excko(tndeg3, zsuha);
			mkuxt.excko(tndeg3, lfoot);
			mkuxt.excko(tndeg3, wcczf);
			tndeg3.Write(tndeg2.GetBuffer(), 0, (int)tndeg2.Length);
			mkuxt.lcbhj(tndeg2, privateKey.ogvve(tndeg3.ToArray(), keyAlgorithmId, padSignature), p2: false);
		}
		eydfa = tndeg2.ToArray();
	}
}
