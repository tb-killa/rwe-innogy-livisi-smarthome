using System;
using System.Collections.Generic;
using System.IO;
using Rebex.Security.Cryptography;
using onrkn;

namespace Rebex.IO;

public struct Checksum : IComparable
{
	private struct gsyzj
	{
		public readonly Action<SeekOrigin, long> hcmac;

		public readonly Func<byte[], int, int, int> qhreg;

		public gsyzj(Action<SeekOrigin, long> seek, Func<byte[], int, int, int> read)
		{
			hcmac = seek;
			qhreg = read;
		}
	}

	private sealed class qubiu
	{
		public eyqzi tnale;

		public void rhkse(SeekOrigin p0, long p1)
		{
			tnale.qgsek((vnfav)p0, p1);
		}
	}

	private sealed class pxdai
	{
		public Stream okqbl;

		public void agmlj(SeekOrigin p0, long p1)
		{
			okqbl.Seek(p1, p0);
		}
	}

	private readonly ChecksumAlgorithm exnba;

	private readonly byte[] bvyeh;

	private string xkhch;

	public ChecksumAlgorithm Algorithm => exnba;

	public string Value => ToString();

	public byte[] GetBytes()
	{
		return (byte[])bvyeh.Clone();
	}

	public Checksum(ChecksumAlgorithm algorithm, byte[] checksum)
	{
		bpkgq.xqnvu(algorithm, "algorithm");
		exnba = algorithm;
		bvyeh = checksum;
		xkhch = null;
	}

	public static implicit operator string(Checksum checksum)
	{
		return checksum.ToString();
	}

	public int CompareTo(object obj)
	{
		if (obj == null || false || !(obj is Checksum))
		{
			return 1;
		}
		Checksum checksum = (Checksum)obj;
		int num = exnba.CompareTo(checksum.exnba);
		if (num != 0 && 0 == 0)
		{
			return num;
		}
		byte[] array = checksum.bvyeh;
		num = bvyeh.Length.CompareTo(array.Length);
		if (num != 0 && 0 == 0)
		{
			return num;
		}
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0080;
		}
		goto IL_00a6;
		IL_00a6:
		if (num2 < bvyeh.Length)
		{
			goto IL_0080;
		}
		return num;
		IL_0080:
		num = bvyeh[num2].CompareTo(array[num2]);
		if (num != 0 && 0 == 0)
		{
			return num;
		}
		num2++;
		goto IL_00a6;
	}

	public override bool Equals(object obj)
	{
		return CompareTo(obj) == 0;
	}

	public override int GetHashCode()
	{
		uint num = 0u;
		uint num2 = 0u;
		if (num2 != 0)
		{
			goto IL_0008;
		}
		goto IL_001f;
		IL_0008:
		num = bvyeh[num2] ^ ((num << 1) | (num >> 31));
		num2++;
		goto IL_001f;
		IL_001f:
		if (num2 < bvyeh.Length)
		{
			goto IL_0008;
		}
		return (int)num;
	}

	public override string ToString()
	{
		string text = xkhch;
		if (text == null || 1 == 0)
		{
			text = (xkhch = brgjd.wlvqq(bvyeh));
		}
		return text;
	}

	private static ulong mprqx(gsyzj p0, ulong p1, ChecksumAlgorithm p2, out byte[] p3)
	{
		HashingAlgorithm hashingAlgorithm = new HashingAlgorithm((HashingAlgorithmId)p2, force: true);
		try
		{
			IHashTransform hashTransform = hashingAlgorithm.CreateTransform();
			try
			{
				byte[] array = new byte[65536];
				ulong num = p1;
				int num2;
				int num3;
				do
				{
					num2 = (int)Math.Min(num, 65536uL);
					num3 = p0.qhreg(array, 0, num2);
					if (num3 > 0)
					{
						hashTransform.Process(array, 0, num3);
						num -= (ulong)num3;
					}
				}
				while (num3 >= num2 && num != 0);
				p3 = hashTransform.GetHash();
				return p1 - num;
			}
			finally
			{
				if (hashTransform != null && 0 == 0)
				{
					hashTransform.Dispose();
				}
			}
		}
		finally
		{
			if (hashingAlgorithm != null && 0 == 0)
			{
				((IDisposable)hashingAlgorithm).Dispose();
			}
		}
	}

	internal static byte[] qezdt(eyqzi p0, ChecksumAlgorithm p1, long? p2, ulong p3, uint p4)
	{
		qubiu qubiu = new qubiu();
		qubiu.tnale = p0;
		gsyzj p5 = new gsyzj(qubiu.rhkse, qubiu.tnale.mlhqn);
		return rplyk(p5, p1, p2, p3, p4);
	}

	internal static byte[] nrrak(Stream p0, ChecksumAlgorithm p1, long? p2, ulong p3, uint p4)
	{
		pxdai pxdai = new pxdai();
		pxdai.okqbl = p0;
		gsyzj p5 = new gsyzj(pxdai.agmlj, pxdai.okqbl.Read);
		return rplyk(p5, p1, p2, p3, p4);
	}

	private static byte[] rplyk(gsyzj p0, ChecksumAlgorithm p1, long? p2, ulong p3, uint p4)
	{
		List<byte[]> list = new List<byte[]>();
		if (p2.HasValue && 0 == 0)
		{
			p0.hcmac(SeekOrigin.Begin, p2.Value);
		}
		ulong num = ((p3 == 0) ? ulong.MaxValue : p3);
		ulong num2;
		ulong num3;
		do
		{
			num2 = ((p4 == 0) ? num : Math.Min(p4, num));
			num3 = mprqx(p0, num2, p1, out var p5);
			if (num3 != 0)
			{
				list.Add(p5);
				num -= num3;
			}
		}
		while (num3 != 0 && num != 0 && num3 >= num2);
		int num4;
		byte[] array;
		int num5;
		switch (list.Count)
		{
		case 0:
			return new byte[0];
		case 1:
			return list[0];
		default:
			{
				num4 = list[0].Length;
				array = new byte[list.Count * num4];
				num5 = 0;
				if (num5 != 0)
				{
					goto IL_00ca;
				}
				goto IL_00e7;
			}
			IL_00e7:
			if (num5 < list.Count)
			{
				goto IL_00ca;
			}
			return array;
			IL_00ca:
			Array.Copy(list[num5], 0, array, num5 * num4, num4);
			num5++;
			goto IL_00e7;
		}
	}
}
