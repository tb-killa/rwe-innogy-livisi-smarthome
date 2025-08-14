using System.Linq;
using CommonFunctionality.Encryption;

namespace SerialAPI;

public class AesConditionalSwitch
{
	private byte[] aesKey;

	public AesConditionalSwitch(byte[] aesKey)
	{
		this.aesKey = aesKey;
	}

	public byte[] CalculateAesSignature(byte[] source, byte[] destination, byte[] counter, byte header, byte[] message)
	{
		byte[] array = new byte[16]
		{
			73, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0
		};
		source.CopyTo(array, 1);
		destination.CopyTo(array, 4);
		counter.CopyTo(array, 7);
		new byte[6] { 0, 0, 0, 0, 0, 5 }.CopyTo(array, 10);
		byte[] array2 = new byte[16]
		{
			counter[2],
			header,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};
		message.CopyTo(array2, 2);
		byte[] array3 = new byte[10];
		array3.CopyTo(array2, 6);
		byte[] a = Encrypt(array);
		byte[] input = XORArray(a, array2);
		byte[] array4 = Encrypt(input);
		return array4.Skip(array4.Length - 4).Take(4).ToArray();
	}

	private byte[] Encrypt(byte[] input)
	{
		Aes aes = new Aes(KeySize.Bits128, aesKey);
		byte[] output = new byte[16];
		aes.Cipher(input, out output);
		return output;
	}

	private byte[] XORArray(byte[] a, byte[] b)
	{
		byte[] array = new byte[a.Length];
		for (int i = 0; i < a.Length; i++)
		{
			array[i] = (byte)(a[i] ^ b[i]);
		}
		return array;
	}
}
