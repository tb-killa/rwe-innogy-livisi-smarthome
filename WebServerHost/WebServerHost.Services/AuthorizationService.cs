using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using Rebex.Security.Cryptography;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.Entities.Login;
using SmartHome.Common.Generic.LogManager;
using WebServerHost.Helpers;
using WebServerHost.Web.Extensions;
using WebServerHost.Web.Http;

namespace WebServerHost.Services;

internal class AuthorizationService : IAuthorization
{
	private const string TokenHeader = "{\"alg\":\"RS256\",\"typ\":\"JWT\"}";

	private const string BaererTokenStart = "Bearer ";

	private const string DefaultBasicAuthorizationHeaderValue = "Basic Y2xpZW50SWQ6Y2xpZW50UGFzcw==";

	private const int JwtCacheSize = 3;

	private static readonly string EncodedTokenHeader = ToBase64("{\"alg\":\"RS256\",\"typ\":\"JWT\"}");

	private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	private static readonly long DefaultTokenLifeTime = (long)new TimeSpan(48, 0, 0).TotalSeconds;

	private static readonly string keyPath = $"NandFlash/{SHCSerialNumber.SerialNumber()}.k";

	private static readonly string issuedTokensPersistancePath = "NandFlash/tokens.json";

	private readonly ILogger logger = LogManager.Instance.GetLogger<AuthorizationService>();

	private readonly RSAParameters key;

	private readonly string BasicAuthorizationHeaderValue;

	private ILocalUserManager userManager;

	private IConfigurationManager configManager;

	private IRegistrationService registrationService;

	private List<Token> issuedTokens = new List<Token>();

	private List<string> cachedJWTs = new List<string>(3);

	private object locker = new object();

	public AuthorizationService(ILocalUserManager userManager, IConfigurationManager configManager, IRegistrationService registrationService)
	{
		this.userManager = userManager;
		this.configManager = configManager;
		this.registrationService = registrationService;
		key = GetKey();
		BasicAuthorizationHeaderValue = GetBasicAuthHeaderValue();
		LoadIssuedTokens();
	}

	private RSAParameters GetKey()
	{
		if (File.Exists(keyPath))
		{
			RSAParameters rSAParameters = RSAKeyHelper.LoadFromFile(keyPath);
			try
			{
				RSAManaged rSAManaged = new RSAManaged();
				rSAManaged.ImportParameters(rSAParameters);
				return rSAParameters;
			}
			catch
			{
				File.Delete(keyPath);
			}
		}
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		RSAParameters rSAParameters = rSACryptoServiceProvider.ExportParameters(includePrivateParameters: true);
		RSAKeyHelper.SaveToFile(keyPath, rSAParameters);
		return rSAParameters;
	}

	private string GetBasicAuthHeaderValue()
	{
		try
		{
			string text = configManager["RWE.SmartHome.SHC.WebServiceHost"].GetString("ClientId");
			string text2 = configManager["RWE.SmartHome.SHC.WebServiceHost"].GetString("ClientSecret");
			if (text.IsNotEmptyOrNull() && text2.IsNotEmptyOrNull())
			{
				logger.Debug("Authorization: Using clientId and client secret from config");
				string arg = ToBase64($"{text}:{text2}");
				return $"Basic {arg}";
			}
			return "Basic Y2xpZW50SWQ6Y2xpZW50UGFzcw==";
		}
		catch
		{
			return "Basic Y2xpZW50SWQ6Y2xpZW50UGFzcw==";
		}
	}

	private void LoadIssuedTokens()
	{
		if (!File.Exists(issuedTokensPersistancePath))
		{
			return;
		}
		string text = string.Empty;
		using (StreamReader streamReader = File.OpenText(issuedTokensPersistancePath))
		{
			text = streamReader.ReadToEnd();
		}
		if (!text.IsNotEmptyOrNull())
		{
			return;
		}
		lock (locker)
		{
			try
			{
				issuedTokens = text.FromJson<List<Token>>();
			}
			catch (Exception exception)
			{
				logger.Debug("Could not load issued tokens, file may have been coruppted, celaning up", exception);
				File.Delete(issuedTokensPersistancePath);
			}
		}
	}

	public TokenResponse Authenticate(TokenRequestData tokenRequest)
	{
		if (tokenRequest.Username != userManager.User.Name || !PasswordHelper.Matches(tokenRequest.Password, userManager.User.Password))
		{
			throw new ApiException(ErrorCode.InvalidUserCredentials);
		}
		return IssueNewToken(Guid.NewGuid());
	}

	private TokenResponse IssueNewToken(Guid tokenIdentifier)
	{
		long lifeTime = GetLifeTime();
		long num = ToTimeStamp(DateTime.UtcNow);
		bool tearmsAndConditionAccepted = registrationService.TearmsAndConditionAccepted;
		logger.Debug($"TaC accepted {tearmsAndConditionAccepted}");
		Token token = new Token();
		token.audiance = "all";
		token.expirationTime = num + lifeTime;
		token.issueTime = num;
		token.issuedBy = SHCSerialNumber.SerialNumber();
		token.uniqueIdentifier = tokenIdentifier;
		token.subject = userManager.User.Name;
		token.userPermisins = (tearmsAndConditionAccepted ? ulong.MaxValue.ToString("X2") : "000000000061B000");
		token.device = (tearmsAndConditionAccepted ? SHCSerialNumber.SerialNumber() : null);
		Token token2 = token;
		string text = ToJWT(token2);
		ChacheJWT(text);
		AddToken(token2);
		TokenResponse tokenResponse = new TokenResponse();
		tokenResponse.AccessToken = text;
		tokenResponse.RefreshToken = token2.uniqueIdentifier.ToString("N");
		tokenResponse.ExpiresIn = lifeTime;
		return tokenResponse;
	}

	private long GetLifeTime()
	{
		long? num = configManager["RWE.SmartHome.SHC.WebServiceHost"].GetLong("AccessTokenLifeTime");
		if (!num.HasValue)
		{
			return DefaultTokenLifeTime;
		}
		return num.Value;
	}

	private string ToJWT(Token token)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string s = stringBuilder.Append(EncodedTokenHeader).Append('.').Append(ToBase64(token.ToJson()))
			.ToString();
		RSAManaged rSAManaged = new RSAManaged();
		rSAManaged.ImportParameters(key);
		Rebex.Security.Cryptography.SHA256Managed sHA256Managed = Rebex.Security.Cryptography.SHA256Managed.Create();
		byte[] rgb = sHA256Managed.ComputeHash(Encoding.UTF8.GetBytes(s));
		byte[] inArray = rSAManaged.Encrypt(rgb);
		stringBuilder.Append('.').Append(Convert.ToBase64String(inArray));
		return stringBuilder.ToString();
	}

	private void ChacheJWT(string jwt)
	{
		if (cachedJWTs.Count == 3)
		{
			cachedJWTs.RemoveAt(2);
		}
		cachedJWTs.Insert(0, jwt);
	}

	private void AddToken(Token token)
	{
		lock (locker)
		{
			if (issuedTokens.Any((Token t) => t.uniqueIdentifier == token.uniqueIdentifier))
			{
				issuedTokens.RemoveAll((Token t) => t.uniqueIdentifier == token.uniqueIdentifier);
			}
			issuedTokens.Add(token);
			long now = ToTimeStamp(DateTime.UtcNow);
			issuedTokens.RemoveAll((Token t) => t.expirationTime < now);
			using StreamWriter streamWriter = new StreamWriter(issuedTokensPersistancePath);
			streamWriter.Write(issuedTokens.ToJson());
			streamWriter.Flush();
			streamWriter.Close();
		}
	}

	private static long ToTimeStamp(DateTime dateTime)
	{
		return (long)(dateTime - Epoch).TotalSeconds;
	}

	private static string ToBase64(string text)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		return Convert.ToBase64String(bytes);
	}

	private static string FromBase64(string base64string)
	{
		byte[] array = Convert.FromBase64String(base64string);
		return Encoding.UTF8.GetString(array, 0, array.Length);
	}

	public void Authorize(ShcWebRequest request)
	{
		string value = string.Empty;
		if (!request.Headers.TryGetValueIgnoreCase("Authorization", out value))
		{
			throw new ApiException(ErrorCode.InvalidTokenRequest, "No Bearer token provided");
		}
		if (value.StartsWith("Bearer "))
		{
			string jwt = value.Remove(0, "Bearer ".Length);
			Authorize(jwt);
			return;
		}
		throw new ApiException(ErrorCode.InvalidTokenRequest, "Invalid Token");
	}

	public void Authorize(string jwt)
	{
		try
		{
			if (cachedJWTs.Any((string t) => t == jwt))
			{
				CheckExpiration(jwt.Split('.')[1]);
			}
			else
			{
				ValidateJwt(jwt);
				ChacheJWT(jwt);
			}
		}
		catch (FormatException ex)
		{
			throw new ApiException(ErrorCode.InvalidTokenRequest, ex.Message);
		}
	}

	private void CheckExpiration(string tokenBase64)
	{
		Token token = FromBase64(tokenBase64).FromJson<Token>();
		bool flag = GetIssuedToken(token.uniqueIdentifier) != null;
		if (token.expirationTime < ToTimeStamp(DateTime.UtcNow) || !flag)
		{
			throw new ApiException(ErrorCode.TokenExpired);
		}
	}

	private Token GetIssuedToken(Guid uniqueIdentifier)
	{
		lock (locker)
		{
			return issuedTokens.FirstOrDefault((Token t) => t.uniqueIdentifier == uniqueIdentifier);
		}
	}

	private void ValidateJwt(string jwt)
	{
		string[] array = jwt.Split('.');
		if (array.Length != 3 || array[0] != EncodedTokenHeader)
		{
			throw new ApiException(ErrorCode.InvalidTokenRequest, "Invalid Token");
		}
		byte[] signature = Convert.FromBase64String(array[2]);
		byte[] bytes = Encoding.UTF8.GetBytes(string.Join(".", array, 0, 2));
		if (!VerifySingedData(bytes, signature))
		{
			throw new ApiException(ErrorCode.TokenSignatureInvalid);
		}
		CheckExpiration(array[1]);
	}

	private bool VerifySingedData(byte[] data, byte[] signature)
	{
		try
		{
			RSAManaged rSAManaged = new RSAManaged();
			rSAManaged.ImportParameters(key);
			Rebex.Security.Cryptography.SHA256Managed sHA256Managed = Rebex.Security.Cryptography.SHA256Managed.Create();
			byte[] array = sHA256Managed.ComputeHash(data);
			byte[] array2 = rSAManaged.Decrypt(signature);
			if (array.Length == array2.Length)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != array2[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
		catch (CryptographicException ex)
		{
			logger.Error(ex.Message, ex);
			return false;
		}
	}

	public TokenResponse RefreshAuthentication(TokenRequestData refereshToken)
	{
		if (refereshToken.RefreshToken == null)
		{
			throw new ApiException(ErrorCode.ShcOperationError, "Missing 'refresh_token'");
		}
		Token issuedToken = GetIssuedToken(refereshToken.RefreshToken.ToGuid());
		if (issuedToken == null)
		{
			throw new ApiException(ErrorCode.InvalidTokenRequest);
		}
		return IssueNewToken(issuedToken.uniqueIdentifier);
	}

	public void ValidateBasicAuthorization(Dictionary<string, string> requestHeaders)
	{
		if (requestHeaders.FirstOrDefault((KeyValuePair<string, string> h) => h.Key.EqualsIgnoreCase("Authorization")).Value == BasicAuthorizationHeaderValue)
		{
			return;
		}
		throw new ApiException(ErrorCode.InvalidClientCredentials);
	}

	public void InvalidateTokens()
	{
		lock (locker)
		{
			issuedTokens.Clear();
			using (StreamWriter streamWriter = new StreamWriter(issuedTokensPersistancePath))
			{
				streamWriter.Write(issuedTokens.ToJson());
				streamWriter.Flush();
				streamWriter.Close();
			}
			cachedJWTs.Clear();
		}
	}
}
