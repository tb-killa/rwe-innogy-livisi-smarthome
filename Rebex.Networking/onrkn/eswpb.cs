using System;
using System.Collections.Generic;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class eswpb
{
	private class ajqid : IHashTransform, IDisposable
	{
		private readonly IHashTransform tmxrr;

		private readonly int wynih;

		private int yyxzg => wynih * 8;

		public ajqid(IHashTransform inner, int hashSize)
		{
			tmxrr = inner;
			wynih = hashSize;
		}

		private void lhjne(byte[] p0, int p1, int p2)
		{
			tmxrr.Process(p0, p1, p2);
		}

		void IHashTransform.Process(byte[] p0, int p1, int p2)
		{
			//ILSpy generated this explicit interface implementation from .override directive in lhjne
			this.lhjne(p0, p1, p2);
		}

		private byte[] cifwm()
		{
			byte[] hash = tmxrr.GetHash();
			byte[] array = new byte[wynih];
			Array.Copy(hash, 0, array, 0, wynih);
			return array;
		}

		byte[] IHashTransform.GetHash()
		{
			//ILSpy generated this explicit interface implementation from .override directive in cifwm
			return this.cifwm();
		}

		private void cyzcn()
		{
			tmxrr.Reset();
		}

		void IHashTransform.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in cyzcn
			this.cyzcn();
		}

		private void csfuu()
		{
			tmxrr.Dispose();
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in csfuu
			this.csfuu();
		}
	}

	private string krjxq;

	private HashingAlgorithmId bkhpb;

	private SshMacAlgorithm lwvgw;

	private int dfgmr;

	private int uizqd;

	private bool hhpnz;

	private byte[] gohqv;

	public string hluhi
	{
		get
		{
			return krjxq;
		}
		private set
		{
			krjxq = value;
		}
	}

	public HashingAlgorithmId zsreb
	{
		get
		{
			return bkhpb;
		}
		private set
		{
			bkhpb = value;
		}
	}

	public SshMacAlgorithm hcwod
	{
		get
		{
			return lwvgw;
		}
		private set
		{
			lwvgw = value;
		}
	}

	public int qwzwb
	{
		get
		{
			return dfgmr;
		}
		private set
		{
			dfgmr = value;
		}
	}

	public int shdup
	{
		get
		{
			return uizqd;
		}
		private set
		{
			uizqd = value;
		}
	}

	public bool satyn
	{
		get
		{
			return hhpnz;
		}
		private set
		{
			hhpnz = value;
		}
	}

	public byte[] inemt
	{
		get
		{
			return gohqv;
		}
		set
		{
			gohqv = value;
		}
	}

	public static eswpb apzyo(string p0)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (awprl.ylgfe == null || 1 == 0)
			{
				awprl.ylgfe = new Dictionary<string, int>(8)
				{
					{ "hmac-md5", 0 },
					{ "hmac-md5-96", 1 },
					{ "hmac-sha1", 2 },
					{ "hmac-sha1-96", 3 },
					{ "hmac-sha2-256", 4 },
					{ "hmac-sha2-512", 5 },
					{ "hmac-sha2-256-etm@openssh.com", 6 },
					{ "hmac-sha2-512-etm@openssh.com", 7 }
				};
			}
			if (awprl.ylgfe.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
					return new eswpb(p0, HashingAlgorithmId.MD5, SshMacAlgorithm.MD5, 16, 16, etm: false);
				case 1:
					return new eswpb(p0, HashingAlgorithmId.MD5, SshMacAlgorithm.MD5, 16, 12, etm: false);
				case 2:
					return new eswpb(p0, HashingAlgorithmId.SHA1, SshMacAlgorithm.SHA1, 20, 20, etm: false);
				case 3:
					return new eswpb(p0, HashingAlgorithmId.SHA1, SshMacAlgorithm.SHA1, 20, 12, etm: false);
				case 4:
					return new eswpb(p0, HashingAlgorithmId.SHA256, SshMacAlgorithm.SHA256, 32, 32, etm: false);
				case 5:
					return new eswpb(p0, HashingAlgorithmId.SHA512, SshMacAlgorithm.SHA512, 64, 64, etm: false);
				case 6:
					return new eswpb(p0, HashingAlgorithmId.SHA256, SshMacAlgorithm.SHA256, 32, 32, etm: true);
				case 7:
					return new eswpb(p0, HashingAlgorithmId.SHA512, SshMacAlgorithm.SHA512, 64, 64, etm: true);
				}
			}
		}
		return null;
	}

	public eswpb(string id, HashingAlgorithmId alg, SshMacAlgorithm mac, int keySize, int hashSize, bool etm)
	{
		hluhi = id;
		zsreb = alg;
		hcwod = mac;
		qwzwb = keySize;
		shdup = hashSize;
		satyn = etm;
	}

	public IHashTransform ktkgi()
	{
		HashingAlgorithm hashingAlgorithm = new HashingAlgorithm(zsreb);
		hashingAlgorithm.KeyMode = HashingAlgorithmKeyMode.HMAC;
		hashingAlgorithm.SetKey(inemt);
		IHashTransform hashTransform = hashingAlgorithm.CreateTransform();
		if (qwzwb != shdup)
		{
			hashTransform = new ajqid(hashTransform, shdup);
		}
		return hashTransform;
	}
}
