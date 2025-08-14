using System;
using System.Text;
using Rebex.Security.Cryptography;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class PasswordHelper
{
	public static string HashPassword(string plainPassword)
	{
		using SHA256Managed sHA256Managed = SHA256Managed.Create();
		byte[] inArray = sHA256Managed.ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
		return Convert.ToBase64String(inArray);
	}

	public static bool Matches(string password, string hash)
	{
		if (password != null)
		{
			return HashPassword(password) == hash;
		}
		return false;
	}
}
