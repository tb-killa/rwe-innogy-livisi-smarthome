using System;
using Org.Mentalis.Security.Certificates;

namespace Org.Mentalis.Security.Ssl;

public class SecurityOptions : ICloneable
{
	private SslAlgorithms m_AllowedAlgorithms;

	private Certificate m_Certificate;

	private string m_CommonName;

	private ConnectionEnd m_Entity;

	private SecurityFlags m_Flags;

	private SecureProtocol m_Protocol;

	private CertRequestEventHandler m_RequestHandler;

	private CredentialVerification m_VerificationType;

	private CertVerifyEventHandler m_Verifier;

	public SecureProtocol Protocol
	{
		get
		{
			return m_Protocol;
		}
		set
		{
			m_Protocol = value;
		}
	}

	public Certificate Certificate
	{
		get
		{
			return m_Certificate;
		}
		set
		{
			m_Certificate = value;
		}
	}

	public ConnectionEnd Entity
	{
		get
		{
			return m_Entity;
		}
		set
		{
			m_Entity = value;
		}
	}

	public CredentialVerification VerificationType
	{
		get
		{
			return m_VerificationType;
		}
		set
		{
			m_VerificationType = value;
		}
	}

	public CertVerifyEventHandler Verifier
	{
		get
		{
			return m_Verifier;
		}
		set
		{
			m_Verifier = value;
		}
	}

	public CertRequestEventHandler RequestHandler
	{
		get
		{
			return m_RequestHandler;
		}
		set
		{
			m_RequestHandler = value;
		}
	}

	public string CommonName
	{
		get
		{
			return m_CommonName;
		}
		set
		{
			m_CommonName = value;
		}
	}

	public SecurityFlags Flags
	{
		get
		{
			return m_Flags;
		}
		set
		{
			m_Flags = value;
		}
	}

	public SslAlgorithms AllowedAlgorithms
	{
		get
		{
			return m_AllowedAlgorithms;
		}
		set
		{
			m_AllowedAlgorithms = value;
		}
	}

	public SecurityOptions(SecureProtocol protocol, Certificate cert, ConnectionEnd entity, CredentialVerification verifyType, CertVerifyEventHandler verifier, string commonName, SecurityFlags flags, SslAlgorithms allowed, CertRequestEventHandler requestHandler)
	{
		Protocol = protocol;
		Certificate = cert;
		Entity = entity;
		VerificationType = verifyType;
		Verifier = verifier;
		CommonName = commonName;
		Flags = flags;
		AllowedAlgorithms = allowed;
		RequestHandler = requestHandler;
	}

	public SecurityOptions(SecureProtocol protocol, Certificate cert, ConnectionEnd entity)
		: this(protocol, cert, entity, CredentialVerification.Auto, null, null, SecurityFlags.Default, SslAlgorithms.ALL, null)
	{
	}

	public SecurityOptions(SecureProtocol protocol)
		: this(protocol, null, ConnectionEnd.Client, CredentialVerification.Auto, null, null, SecurityFlags.Default, SslAlgorithms.ALL, null)
	{
	}

	public object Clone()
	{
		return new SecurityOptions(Protocol, Certificate, Entity, VerificationType, Verifier, CommonName, Flags, AllowedAlgorithms, RequestHandler);
	}
}
