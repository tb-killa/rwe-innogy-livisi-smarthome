using System;
using System.Reflection;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class jhqmg : eatps, dzjkq, ijjlm, hibhk, lncnv, ibhso, IDisposable
{
	private class wetwu : imfrk
	{
		private readonly string bpffu;

		private readonly object okiry;

		public wetwu(string name, object objAlg)
		{
			if (objAlg == null || 1 == 0)
			{
				throw new ArgumentNullException("objAlg");
			}
			bpffu = name;
			okiry = objAlg;
		}

		private eatps azmat(PrivateKeyInfo p0)
		{
			jhqmg jhqmg2 = new jhqmg(bpffu, okiry);
			string value;
			if ((value = p0.KeyAlgorithm.Oid.Value) == null)
			{
				goto IL_008c;
			}
			if (!(value == "1.3.101.112") || 1 == 0)
			{
				if (!(value == "1.3.101.110") || 1 == 0)
				{
					goto IL_008c;
				}
				byte[] p1 = p0.hsjue();
				byte[] rtrhq = rwolq.tvjgt(p1).rtrhq;
				jhqmg2.weuew(rtrhq);
			}
			else
			{
				byte[] p2 = p0.kfvak();
				jhqmg2.vktkl(p2);
			}
			goto IL_00a2;
			IL_008c:
			byte[] p3 = p0.hsjue();
			jhqmg2.weuew(p3);
			goto IL_00a2;
			IL_00a2:
			return jhqmg2;
		}

		eatps imfrk.xaunu(PrivateKeyInfo p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in azmat
			return this.azmat(p0);
		}

		private eatps ubesa(PublicKeyInfo p0)
		{
			byte[] p1 = p0.ToBytes();
			jhqmg jhqmg2 = new jhqmg(bpffu, okiry);
			jhqmg2.hyuay(p1);
			return jhqmg2;
		}

		eatps imfrk.neqkn(PublicKeyInfo p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ubesa
			return this.ubesa(p0);
		}

		private eatps orygi()
		{
			jhqmg jhqmg2 = new jhqmg(bpffu, okiry);
			jhqmg2.craet();
			return jhqmg2;
		}

		eatps imfrk.poerm()
		{
			//ILSpy generated this explicit interface implementation from .override directive in orygi
			return this.orygi();
		}
	}

	private readonly string xcuje;

	private readonly HashingAlgorithmId swqxh;

	private readonly int lofyx;

	private readonly AsymmetricKeyAlgorithmId lsqzz;

	private readonly aabmg mnxxw;

	private readonly object tcqao;

	private bool egmyo;

	public string janem => xcuje;

	public int KeySize => lofyx;

	public AsymmetricKeyAlgorithmId bptsq => lsqzz;

	private jhqmg(string algorithmName, object inner)
	{
		if (inner == null || 1 == 0)
		{
			throw new ArgumentNullException("inner");
		}
		if (algorithmName == null || 1 == 0)
		{
			throw new ArgumentNullException("algorithmName");
		}
		if (!bpkgq.fmlhx(algorithmName, out algorithmName, out lsqzz, out var _, out lofyx, out var _, out var _, out swqxh) || 1 == 0)
		{
			throw new ArgumentException("Unsupported algorithm name.", "algorithmName");
		}
		xcuje = algorithmName;
		tcqao = inner;
		mnxxw = aabmg.grzle(inner.GetType());
	}

	public void Dispose()
	{
		if (!egmyo)
		{
			egmyo = true;
			if ((object)mnxxw.xqyie != null && 0 == 0)
			{
				mnxxw.xqyie.Invoke(tcqao, new object[0]);
			}
		}
	}

	private object oqame(MethodBase p0, string p1, params object[] p2)
	{
		if (egmyo && 0 == 0)
		{
			throw new ObjectDisposedException("AsymmetricAlgorithmProxy");
		}
		if ((object)p0 == null || 1 == 0)
		{
			throw new NotSupportedException(brgjd.edcru("The '{0}' method is not supported by the plugin.", p1));
		}
		return mnxxw.njjty(tcqao, p0, p2);
	}

	private void weuew(byte[] p0)
	{
		oqame(mnxxw.xlgms, "FromPrivateKey", p0);
	}

	private void vktkl(byte[] p0)
	{
		oqame(mnxxw.eytuw, "FromSeed", p0);
	}

	private void hyuay(byte[] p0)
	{
		oqame(mnxxw.aayhb, "FromPublicKey", p0);
	}

	public byte[] craet()
	{
		return (byte[])oqame(mnxxw.euota, "GetPublicKey");
	}

	public byte[] knali()
	{
		return (byte[])oqame(mnxxw.heyqp, "GetPrivateKey");
	}

	public PublicKeyInfo kptoi()
	{
		AlgorithmIdentifier algorithm = bpkgq.aykug(janem);
		byte[] publicKey = craet();
		return new PublicKeyInfo(algorithm, publicKey);
	}

	public PrivateKeyInfo jbbgs(bool p0)
	{
		AlgorithmIdentifier algorithmIdentifier = bpkgq.aykug(xcuje);
		byte[] array = knali();
		byte[] publicKey = null;
		string value;
		if ((value = algorithmIdentifier.Oid.Value) == null)
		{
			goto IL_006f;
		}
		if (!(value == "1.3.101.112") || 1 == 0)
		{
			if (!(value == "1.3.101.110"))
			{
				goto IL_006f;
			}
			array = new rwolq(array).ionjf();
			publicKey = craet();
		}
		goto IL_0096;
		IL_006f:
		if (lsqzz == AsymmetricKeyAlgorithmId.ECDsa || lsqzz == AsymmetricKeyAlgorithmId.ECDH)
		{
			array = tsnbe.hvphu(array, janem);
		}
		goto IL_0096;
		IL_0096:
		return new PrivateKeyInfo(algorithmIdentifier, array, publicKey, lsqzz);
	}

	public CspParameters iqqfj()
	{
		return null;
	}

	public bool vmedb(mrxvh p0)
	{
		return p0.hqtwc == goies.gbwxv;
	}

	public byte[] rypyi(byte[] p0, mrxvh p1)
	{
		if ((object)mnxxw.mrlet == null || 1 == 0)
		{
			throw new CryptographicException("Hash signing is not supported for this algorithm.");
		}
		if (p1.hqtwc != goies.gbwxv)
		{
			throw new CryptographicException("Padding scheme is not supported for this algorithm.");
		}
		p0 = hvati(p0, bpkgq.wrqur(p1.faqqk));
		return (byte[])oqame(mnxxw.mrlet, "SignHash", p0);
	}

	public bool cbzmp(byte[] p0, byte[] p1, mrxvh p2)
	{
		if ((object)mnxxw.nfizw == null || 1 == 0)
		{
			throw new CryptographicException("Hash signature verification is not supported for this algorithm.");
		}
		if (p2.hqtwc != goies.gbwxv)
		{
			throw new CryptographicException("Padding scheme is not supported for this algorithm.");
		}
		p0 = hvati(p0, bpkgq.wrqur(p2.faqqk));
		return (bool)oqame(mnxxw.nfizw, "VerifyHash", p0, p1);
	}

	public byte[] vxuyd(byte[] p0, HashingAlgorithmId p1)
	{
		if ((object)mnxxw.lqlbg == null || false || p1 != swqxh)
		{
			byte[] array = zkeve(p0, p1);
			return (byte[])oqame(mnxxw.mrlet, "SignHash", array);
		}
		return (byte[])oqame(mnxxw.lqlbg, "SignMessage", p0);
	}

	public bool swsbt(byte[] p0, HashingAlgorithmId p1, byte[] p2)
	{
		if ((object)mnxxw.gwhjg == null || false || p1 != swqxh)
		{
			byte[] array = zkeve(p0, p1);
			return (bool)oqame(mnxxw.nfizw, "VerifyHash", array, p2);
		}
		return (bool)oqame(mnxxw.gwhjg, "VerifyMessage", p0, p2);
	}

	public byte[] ovrid(byte[] p0)
	{
		try
		{
			return (byte[])oqame(mnxxw.sjnnz, "GetSharedSecret", p0);
		}
		catch (NotSupportedException)
		{
			return null;
		}
	}

	private byte[] zkeve(byte[] p0, HashingAlgorithmId p1)
	{
		byte[] p2 = HashingAlgorithm.ComputeHash(p1, p0);
		return hvati(p2, p1);
	}

	private byte[] hvati(byte[] p0, HashingAlgorithmId p1)
	{
		HashingAlgorithmId p2 = swqxh;
		int valueOrDefault = HashingAlgorithm.kfowy(p2).GetValueOrDefault();
		int num = ((p1 == (HashingAlgorithmId)0) ? valueOrDefault : HashingAlgorithm.kfowy(p1).GetValueOrDefault());
		if (valueOrDefault != 0 && 0 == 0 && num != 0 && 0 == 0)
		{
			if (p0.Length != num)
			{
				throw new CryptographicException("Invalid hash size for " + bpkgq.hdrmd(p1) + ".");
			}
			if (num >= 20)
			{
				if (valueOrDefault == num)
				{
					return p0;
				}
				if (valueOrDefault > num)
				{
					byte[] array = new byte[valueOrDefault];
					p0.CopyTo(array, valueOrDefault - num);
					return array;
				}
				byte[] array2 = new byte[valueOrDefault];
				Array.Copy(p0, 0, array2, 0, valueOrDefault);
				return array2;
			}
		}
		throw new CryptographicException(bpkgq.hdrmd(p1) + " is not supported for this algorithm.");
	}

	internal static imfrk eepef(string p0, object p1)
	{
		return new wetwu(p0, p1);
	}
}
