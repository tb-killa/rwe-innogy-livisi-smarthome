using System;
using System.Security.Cryptography;

namespace Rebex.Security.Cryptography;

public class MD5SHA1 : HashAlgorithm
{
	private HashAlgorithm agcqh;

	private HashAlgorithm abmcu;

	public HashAlgorithm MD5 => agcqh;

	public HashAlgorithm SHA1 => abmcu;

	public MD5SHA1()
	{
		HashSizeValue = 288;
		agcqh = HashingAlgorithm.vtcmi(HashingAlgorithmId.MD5, p1: true);
		abmcu = HashingAlgorithm.vtcmi(HashingAlgorithmId.SHA1, p1: false);
	}

	public new static MD5SHA1 Create()
	{
		return new MD5SHA1();
	}

	public override void Initialize()
	{
		agcqh.Initialize();
		abmcu.Initialize();
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
		agcqh.TransformBlock(array, ibStart, cbSize, array, ibStart);
		abmcu.TransformBlock(array, ibStart, cbSize, array, ibStart);
	}

	protected override byte[] HashFinal()
	{
		byte[] array = new byte[36];
		agcqh.TransformFinalBlock(array, 0, 0);
		abmcu.TransformFinalBlock(array, 0, 0);
		Array.Copy(agcqh.Hash, 0, array, 0, 16);
		Array.Copy(abmcu.Hash, 0, array, 16, 20);
		return array;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing ? true : false)
		{
			agcqh.Clear();
			abmcu.Clear();
		}
	}
}
