using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using Rebex;
using Rebex.Net;
using Rebex.Security.Certificates;

namespace SmartHome.SHC.SCommAdapter;

public class WcfBinding : CustomBinding
{
	private readonly TransportBindingElement transportElement;

	private readonly BindingElementCollection elements;

	private readonly List<IWcfMessageObserver> wcfMessageObservers;

	private HttpRequestCreator requestCreator;

	public HttpRequestCreator RequestCreator
	{
		get
		{
			return requestCreator;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			requestCreator = value;
		}
	}

	public override string Scheme => transportElement.Scheme;

	internal IList<IWcfMessageObserver> MessageObservers => wcfMessageObservers;

	public WcfBinding()
		: this(MessageVersion.Soap11, new LogWriter(LogLevel.Off))
	{
	}

	public WcfBinding(MessageVersion version)
		: this(version, new LogWriter(LogLevel.Off))
	{
	}

	public WcfBinding(MessageVersion version, LogLevel logLevel)
		: this(version, new LogWriter(logLevel))
	{
	}

	public WcfBinding(MessageVersion version, LogLevel logLevel, WriteLog logHandler)
		: this(version, new HandleLogWriter(logHandler, logLevel))
	{
	}

	public WcfBinding(LogLevel logLevel)
		: this(MessageVersion.Soap11, new LogWriter(logLevel))
	{
	}

	public WcfBinding(LogLevel logLevel, WriteLog logHandler)
		: this(MessageVersion.Soap11, new HandleLogWriter(logHandler, logLevel))
	{
	}

	public WcfBinding(MessageVersion version, ILogWriter logWriter)
	{
		wcfMessageObservers = new List<IWcfMessageObserver>();
		transportElement = new WcfChannelBindingElement(this);
		elements = new BindingElementCollection();
		elements.Add(new TextMessageEncodingBindingElement(version, Encoding.UTF8));
		elements.Add(transportElement);
		requestCreator = new HttpRequestCreator
		{
			LogWriter = logWriter
		};
		requestCreator.Settings.SslAllowedSuites = TlsCipherSuite.Fast;
		requestCreator.Settings.SslPreferredHashAlgorithm = SignatureHashAlgorithm.SHA1;
	}

	public override BindingElementCollection CreateBindingElements()
	{
		return elements;
	}

	public BindingElementCollection GetElements()
	{
		return elements;
	}

	public T GetElement<T>() where T : BindingElement
	{
		return elements.OfType<T>().FirstOrDefault();
	}
}
