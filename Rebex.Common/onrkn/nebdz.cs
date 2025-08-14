using System;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal abstract class nebdz : ktjcg, ICryptoTransform, IDisposable
{
	private class vbwcg : bhfih
	{
		private readonly SymmetricKeyAlgorithmId jmasb;

		public vbwcg(SymmetricKeyAlgorithmId algId)
		{
			jmasb = algId;
		}

		public gajry smvfj(byte[] p0, prjlw p1)
		{
			ICryptoTransform p2 = rrife(p0, p1.lvrxy);
			return new chhkf(bdsng.gysto(p2), p1.sopyo * 8);
		}

		public fhryo nkjyk(byte[] p0, prjlw p1)
		{
			ICryptoTransform p2 = rrife(p0, p1.lvrxy);
			return new jkyvj(bdsng.gysto(p2), p1.sopyo * 8);
		}

		private ICryptoTransform rrife(byte[] p0, int p1)
		{
			if (p1 != 16)
			{
				throw new ArgumentException("Invalid block size.", "blockSize");
			}
			SymmetricKeyAlgorithm symmetricKeyAlgorithm = new SymmetricKeyAlgorithm(jmasb);
			symmetricKeyAlgorithm.Mode = CipherMode.ECB;
			symmetricKeyAlgorithm.Padding = PaddingMode.None;
			symmetricKeyAlgorithm.BlockSize = p1 * 8;
			symmetricKeyAlgorithm.SetKey(p0);
			return symmetricKeyAlgorithm.CreateEncryptor();
		}
	}

	private const string ornxh = "GcmTransform";

	protected riucd wdccm;

	protected bool kbegk;

	protected long xzegs;

	public bool CanReuseTransform
	{
		get
		{
			spvwl();
			return true;
		}
	}

	public bool CanTransformMultipleBlocks
	{
		get
		{
			spvwl();
			return true;
		}
	}

	public int InputBlockSize
	{
		get
		{
			spvwl();
			return 16;
		}
	}

	public int OutputBlockSize
	{
		get
		{
			spvwl();
			return 16;
		}
	}

	public static bhfih pqtvi(SymmetricKeyAlgorithmId p0)
	{
		return new vbwcg(p0);
	}

	public nebdz(riucd gcm)
	{
		if (gcm == null || 1 == 0)
		{
			throw new ArgumentNullException("gcm");
		}
		wdccm = gcm;
	}

	public void yirig(byte[] p0)
	{
		spvwl();
		wdccm.bjzbe(p0);
	}

	public void seoke(byte[] p0)
	{
		spvwl();
		wdccm.pqwad(p0);
	}

	protected void shwke()
	{
		xzegs = 0L;
		wdccm.liazg();
	}

	protected void qbjpk(byte[] p0, int p1, int p2, byte[] p3, int p4, bool p5)
	{
		dahxy.valft(p0, "inputBuffer", p1, "inputOffset", p2, "inputCount");
		dahxy.valft(p3, "outputBuffer", p4, "outputOffset", p2, "inputCount");
		if (p5 && 0 == 0 && p2 % 16 != 0 && 0 == 0)
		{
			throw new ArgumentException("Invalid count.", "inputCount");
		}
		wdccm.llivr();
	}

	public abstract int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset);

	public abstract int yajzn(byte[] p0, int p1, int p2, byte[] p3, int p4);

	public virtual byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
	{
		spvwl();
		byte[] array = new byte[inputCount];
		yajzn(inputBuffer, inputOffset, inputCount, array, 0);
		return array;
	}

	public void spvwl()
	{
		if (kbegk && 0 == 0)
		{
			throw new ObjectDisposedException("GcmTransform");
		}
	}

	protected virtual void nbfwm(bool p0)
	{
		if (p0 && 0 == 0 && !kbegk)
		{
			kbegk = true;
			wdccm.Dispose();
		}
	}

	public void Dispose()
	{
		nbfwm(p0: true);
		GC.SuppressFinalize(this);
	}
}
