using System;
using System.Linq;
using CommonFunctionality.Encryption;

namespace SerialAPI;

public class AesChallengeResponse
{
	private const int MinMessageSize = 26;

	private const int RandomBlockSize = 6;

	private const int BlockSize = 16;

	private byte[] aesKey;

	private byte[] sessionKey;

	private byte[] originalMessage;

	private byte[] challengeBytes;

	private byte[] random6Bytes;

	private byte[] solutionBytes;

	public AesChallengeResponse(byte[] aesKey, byte[] originalMessage, byte[] challengeBytes)
	{
		this.aesKey = aesKey;
		this.originalMessage = PadArray(originalMessage, 26);
		this.challengeBytes = challengeBytes;
		sessionKey = XORArray(PadArray(challengeBytes, 16), aesKey);
		random6Bytes = Generate6RandomBytes();
	}

	public byte[] Solve()
	{
		byte[] b = originalMessage.Take(10).ToArray();
		byte[] input = AppendArray(random6Bytes, b);
		byte[] a = Encrypt(input);
		byte[] b2 = originalMessage.Skip(10).Take(16).ToArray();
		byte[] input2 = XORArray(a, b2);
		byte[] array = Encrypt(input2);
		solutionBytes = new byte[array.Length];
		array.CopyTo(solutionBytes, 0);
		return array;
	}

	public byte[] ConfirmSolution(byte[] responseBytes)
	{
		byte[] input = ProcessSolution(responseBytes);
		return Decrypt(input);
	}

	public byte[] GenerateResponseReply(byte[] responseBytes, SolutionConfirmationType confirmationType)
	{
		byte[] source = ProcessSolution(responseBytes);
		return source.Skip((confirmationType != SolutionConfirmationType.ACK) ? 4 : 0).Take(4).ToArray();
	}

	public SolutionConfirmationType InterpretResponseReply(byte[] responseReplyBytes)
	{
		if (responseReplyBytes == null)
		{
			return SolutionConfirmationType.NAK;
		}
		byte[] receivedSolution = GetSolutionBytes();
		byte[] source = ProcessSolution(receivedSolution);
		byte[] first = source.Skip(0).Take(4).ToArray();
		byte[] first2 = source.Skip(4).Take(4).ToArray();
		SolutionConfirmationType result = SolutionConfirmationType.Unknown;
		if (first.SequenceEqual(responseReplyBytes))
		{
			result = SolutionConfirmationType.ACK;
		}
		else if (first2.SequenceEqual(responseReplyBytes))
		{
			result = SolutionConfirmationType.NAK;
		}
		return result;
	}

	private byte[] Generate6RandomBytes()
	{
		byte[] array = new byte[6];
		new Random().NextBytes(array);
		return array;
	}

	private byte[] ProcessSolution(byte[] receivedSolution)
	{
		byte[] a = Decrypt(receivedSolution);
		return XORArray(a, originalMessage.Skip(10).Take(16).ToArray());
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

	private byte[] AppendArray(byte[] a, byte[] b)
	{
		byte[] array = new byte[a.Length + b.Length];
		a.CopyTo(array, 0);
		b.CopyTo(array, a.Length);
		return array;
	}

	private byte[] Encrypt(byte[] input)
	{
		Aes aes = new Aes(KeySize.Bits128, sessionKey);
		byte[] output = new byte[16];
		aes.Cipher(input, out output);
		return output;
	}

	private byte[] Decrypt(byte[] input)
	{
		Aes aes = new Aes(KeySize.Bits128, sessionKey);
		byte[] output = new byte[16];
		aes.InvCipher(input, out output);
		return output;
	}

	private byte[] PadArray(byte[] input, int minLength)
	{
		if (input.Length < minLength)
		{
			byte[] array = new byte[minLength];
			input.CopyTo(array, 0);
			return array;
		}
		return input;
	}

	private byte[] GetSolutionBytes()
	{
		if (solutionBytes == null)
		{
			solutionBytes = Solve();
		}
		return solutionBytes;
	}
}
