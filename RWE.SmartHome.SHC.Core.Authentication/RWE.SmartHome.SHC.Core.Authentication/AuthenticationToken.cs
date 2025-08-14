using System;
using RWE.SmartHome.SHC.Core.Authentication.Entities;

namespace RWE.SmartHome.SHC.Core.Authentication;

internal sealed class AuthenticationToken : IEquatable<AuthenticationToken>
{
	private readonly Guid token;

	private readonly User user;

	public Guid Token => token;

	public User User => user;

	public DateTime LastAccessTime { get; set; }

	public AuthenticationToken(User user)
	{
		token = Guid.NewGuid();
		this.user = user;
		LastAccessTime = DateTime.UtcNow;
	}

	public bool Equals(AuthenticationToken other)
	{
		if (other == null)
		{
			return false;
		}
		return object.Equals(token, other.token);
	}

	public override bool Equals(object obj)
	{
		if (!object.ReferenceEquals(this, obj))
		{
			return Equals(obj as AuthenticationToken);
		}
		return true;
	}

	public override int GetHashCode()
	{
		return token.GetHashCode();
	}
}
