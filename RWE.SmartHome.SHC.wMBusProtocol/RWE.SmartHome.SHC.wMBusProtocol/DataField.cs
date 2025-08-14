using System;
using System.Globalization;
using System.Linq;

namespace RWE.SmartHome.SHC.wMBusProtocol;

public static class DataField
{
	public static int LengthOf(DataFieldCode code)
	{
		int result = 0;
		switch (code)
		{
		case DataFieldCode.NoData:
		case DataFieldCode.SelectionForReadout:
			result = 0;
			break;
		case DataFieldCode.Integer8Bit:
		case DataFieldCode.Bcd2Digit:
			result = 1;
			break;
		case DataFieldCode.Integer16Bit:
		case DataFieldCode.Bcd4Digit:
			result = 2;
			break;
		case DataFieldCode.Integer24Bit:
		case DataFieldCode.Bcd6Digit:
			result = 3;
			break;
		case DataFieldCode.Integer32Bit:
		case DataFieldCode.Real32Bit:
		case DataFieldCode.Bcd8Digit:
			result = 4;
			break;
		case DataFieldCode.VariableLength:
			result = -1;
			break;
		case DataFieldCode.Integer48Bit:
		case DataFieldCode.Bcd12Digit:
			result = 6;
			break;
		case DataFieldCode.Integer64Bit:
		case DataFieldCode.SpecialFunctions:
			result = 8;
			break;
		}
		return result;
	}

	public static decimal? ToNumericalValue(byte[] buffer, DataFieldCode code)
	{
		decimal? result = null;
		switch (code)
		{
		case DataFieldCode.SelectionForReadout:
		case DataFieldCode.SpecialFunctions:
			result = 0m;
			break;
		case DataFieldCode.VariableLength:
			throw new NotImplementedException();
		case DataFieldCode.Integer8Bit:
			return (sbyte)buffer[0];
		case DataFieldCode.Integer16Bit:
			return BitConverter.ToInt16(buffer, 0);
		case DataFieldCode.Integer24Bit:
		{
			byte[] array2 = new byte[1];
			byte[] first2 = array2;
			return BitConverter.ToInt32(first2.Concat(buffer).ToArray(), 0) >> 8;
		}
		case DataFieldCode.Integer32Bit:
			return BitConverter.ToInt32(buffer, 0);
		case DataFieldCode.Integer48Bit:
		{
			byte[] array = new byte[2];
			byte[] first = array;
			return BitConverter.ToInt64(first.Concat(buffer).ToArray(), 0) >> 16;
		}
		case DataFieldCode.Integer64Bit:
			return BitConverter.ToInt64(buffer, 0);
		case DataFieldCode.Real32Bit:
			result = (decimal)BitConverter.ToSingle(buffer, 0);
			break;
		case DataFieldCode.Bcd2Digit:
			return BCDConverter.ConvertFromBcd(buffer);
		case DataFieldCode.Bcd4Digit:
			return BCDConverter.ConvertFromBcd(buffer);
		case DataFieldCode.Bcd6Digit:
			return BCDConverter.ConvertFromBcd(buffer);
		case DataFieldCode.Bcd8Digit:
			return BCDConverter.ConvertFromBcd(buffer);
		case DataFieldCode.Bcd12Digit:
			return BCDConverter.ConvertFromBcd(buffer);
		}
		return result;
	}

	public static string ToStringValue(byte[] buffer, DataFieldCode code)
	{
		DataFieldCode dataFieldCode = code;
		if (dataFieldCode == DataFieldCode.VariableLength)
		{
			byte b = buffer[0];
			if (b >= 0 && b <= 191)
			{
				int num = b;
				byte[] array = new byte[num];
				Array.Copy(buffer, 1, array, 0, num);
				return BitConverter.ToString(array);
			}
			if (b >= 192 && b <= 201)
			{
				int num = b - 192;
				byte[] array2 = new byte[num];
				Array.Copy(buffer, 1, array2, 0, num);
				return BCDConverter.ConvertFromBcd(array2).ToString(CultureInfo.InvariantCulture);
			}
			if (b >= 208 && b <= 217)
			{
				int num = b - 208;
				byte[] array3 = new byte[num];
				Array.Copy(buffer, 1, array3, 0, num);
				return BCDConverter.ConvertFromBcd(array3).ToString(CultureInfo.InvariantCulture);
			}
			if (b >= 224 && b <= 240)
			{
				int num = b - 224;
				byte[] array4 = new byte[num];
				Array.Copy(buffer, 1, array4, 0, num);
				return array4.Aggregate(string.Empty, (string current, byte b2) => current + b2.ToString("X02"));
			}
			if (b == 248)
			{
				byte[] array5 = new byte[4];
				Array.Copy(buffer, 1, array5, 0, 4);
				return BitConverter.ToSingle(array5, 0).ToString(CultureInfo.InvariantCulture);
			}
			throw new NotImplementedException("unknown length byte");
		}
		throw new NotImplementedException();
	}

	public static byte[] ToArray(long value, DataFieldCode code)
	{
		byte[] array = null;
		byte[] array2 = null;
		switch (code)
		{
		case DataFieldCode.NoData:
		case DataFieldCode.SelectionForReadout:
		case DataFieldCode.VariableLength:
		case DataFieldCode.SpecialFunctions:
			throw new NotImplementedException();
		case DataFieldCode.Integer8Bit:
			array = new byte[1] { BitConverter.GetBytes((ushort)value)[0] };
			break;
		case DataFieldCode.Integer16Bit:
			array = BitConverter.GetBytes((ushort)value);
			break;
		case DataFieldCode.Integer24Bit:
			array = new byte[3];
			array2 = BitConverter.GetBytes((int)value);
			Buffer.BlockCopy(array2, 0, array, 0, array.Length);
			break;
		case DataFieldCode.Integer32Bit:
			array = BitConverter.GetBytes((int)value);
			break;
		case DataFieldCode.Integer48Bit:
			array = new byte[6];
			array2 = BitConverter.GetBytes(value);
			Buffer.BlockCopy(array2, 0, array, 0, array.Length);
			break;
		case DataFieldCode.Integer64Bit:
			array = BitConverter.GetBytes(value);
			break;
		case DataFieldCode.Real32Bit:
			throw new NotImplementedException();
		case DataFieldCode.Bcd2Digit:
			array = new byte[1];
			array2 = BCDConverter.ConvertToBcd(value);
			Buffer.BlockCopy(array2, 0, array, 0, (array2.Length < array.Length) ? array2.Length : array.Length);
			break;
		case DataFieldCode.Bcd4Digit:
			array = new byte[2];
			array2 = BCDConverter.ConvertToBcd(value);
			Buffer.BlockCopy(array2, 0, array, 0, (array2.Length < array.Length) ? array2.Length : array.Length);
			break;
		case DataFieldCode.Bcd6Digit:
			array = new byte[3];
			array2 = BCDConverter.ConvertToBcd(value);
			Buffer.BlockCopy(array2, 0, array, 0, (array2.Length < array.Length) ? array2.Length : array.Length);
			break;
		case DataFieldCode.Bcd8Digit:
			array = new byte[4];
			array2 = BCDConverter.ConvertToBcd(value);
			Buffer.BlockCopy(array2, 0, array, 0, (array2.Length < array.Length) ? array2.Length : array.Length);
			break;
		case DataFieldCode.Bcd12Digit:
			array = new byte[6];
			array2 = BCDConverter.ConvertToBcd(value);
			Buffer.BlockCopy(array2, 0, array, 0, (array2.Length < array.Length) ? array2.Length : array.Length);
			break;
		}
		return array;
	}
}
