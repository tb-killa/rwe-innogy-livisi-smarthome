using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace Microsoft.Tools.ServiceModel;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.NetCFSvcUtil", "3.5.0.0")]
public class CFClientBase<TChannel> where TChannel : class
{
	[GeneratedCode("Microsoft.Tools.ServiceModel.NetCFSvcUtil", "3.5.0.0")]
	[DebuggerStepThrough]
	private class CFContractSerializer : XmlObjectSerializer
	{
		private CFContractSerializerInfo info;

		private XmlSerializer serializer;

		private static XmlQualifiedName artificialWrapper = new XmlQualifiedName("wrapper", "");

		public CFContractSerializer(CFContractSerializerInfo info)
		{
			this.info = info;
			if (this.info.ExtraTypes == null)
			{
				this.info.ExtraTypes = new Type[0];
			}
			createSerializer(null);
		}

		private void createSerializer(XmlQualifiedName wrapper)
		{
			if (wrapper == null && !info.IsWrapped && info.IsResponse)
			{
				wrapper = artificialWrapper;
			}
			if (info.UseEncoded)
			{
				SoapAttributeOverrides soapAttributeOverrides = new SoapAttributeOverrides();
				if (wrapper != null)
				{
					SoapAttributes soapAttributes = new SoapAttributes();
					soapAttributes.SoapType = new SoapTypeAttribute(wrapper.Name, wrapper.Namespace);
					soapAttributeOverrides.Add(info.MessageContractType, soapAttributes);
				}
				SoapReflectionImporter soapReflectionImporter = new SoapReflectionImporter(soapAttributeOverrides, info.DefaultNamespace);
				for (int i = 0; i < info.ExtraTypes.Length; i++)
				{
					soapReflectionImporter.IncludeType(info.ExtraTypes[i]);
				}
				XmlTypeMapping xmlTypeMapping = soapReflectionImporter.ImportTypeMapping(info.MessageContractType);
				serializer = new XmlSerializer(xmlTypeMapping);
				Log.Information(Module.BackendCommunication, $"Created serializer: createSerializer UseEncoded {info.MessageContractType}");
			}
			else
			{
				XmlRootAttribute xmlRootAttribute = null;
				if (wrapper != null)
				{
					xmlRootAttribute = new XmlRootAttribute();
					xmlRootAttribute.ElementName = wrapper.Name;
					xmlRootAttribute.Namespace = wrapper.Namespace;
				}
				serializer = new XmlSerializer(info.MessageContractType, null, info.ExtraTypes, xmlRootAttribute, info.DefaultNamespace);
				Log.Information(Module.BackendCommunication, $"Created serializer: createSerializer NotUseEncoded {info.MessageContractType}");
			}
		}

		public override bool IsStartObject(XmlDictionaryReader reader)
		{
			throw new NotImplementedException();
		}

		public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
		{
			if (!verifyObjectName)
			{
				throw new NotSupportedException();
			}
			if (info.IsWrapped)
			{
				if (!serializer.CanDeserialize(reader))
				{
					createSerializer(new XmlQualifiedName(XmlConvert.DecodeName(reader.LocalName), reader.NamespaceURI));
					Log.Information(Module.BackendCommunication, $"Created serializer: ReadObject IsWrapped & NotCanDeserialize {reader.LocalName} - {reader.NamespaceURI}");
				}
				return serializer.Deserialize(reader);
			}
			using MemoryStream memoryStream = new MemoryStream();
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.OmitXmlDeclaration = true;
			using XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
			xmlWriter.WriteStartElement(artificialWrapper.Name, artificialWrapper.Namespace);
			string[] array = new string[2] { "xsi", "xsd" };
			for (int i = 0; i < array.Length; i++)
			{
				string text = reader.LookupNamespace(array[i]);
				if (text != null)
				{
					xmlWriter.WriteAttributeString("xmlns", array[i], null, text);
				}
			}
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				xmlWriter.WriteNode(reader, defattr: false);
			}
			xmlWriter.WriteEndElement();
			xmlWriter.Close();
			memoryStream.Position = 0L;
			using XmlReader xmlReader = XmlReader.Create(memoryStream);
			object result = serializer.Deserialize(xmlReader);
			xmlReader.Close();
			return result;
		}

		public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
		{
			throw new NotImplementedException();
		}

		public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
		{
			throw new NotImplementedException();
		}

		public override void WriteEndObject(XmlDictionaryWriter writer)
		{
			throw new NotImplementedException();
		}

		public override void WriteObject(XmlDictionaryWriter writer, object graph)
		{
			if (info.IsWrapped)
			{
				serializer.Serialize(writer, graph);
				return;
			}
			using MemoryStream memoryStream = new MemoryStream();
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.OmitXmlDeclaration = true;
			using XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
			serializer.Serialize(xmlWriter, graph);
			xmlWriter.Close();
			memoryStream.Position = 0L;
			using XmlReader xmlReader = XmlReader.Create(memoryStream);
			xmlReader.Read();
			writer.WriteAttributes(xmlReader, defattr: false);
			if (!xmlReader.IsEmptyElement)
			{
				xmlReader.Read();
				while (xmlReader.NodeType != XmlNodeType.EndElement)
				{
					writer.WriteNode(xmlReader, defattr: false);
				}
			}
			xmlReader.Close();
		}
	}

	protected struct CFContractSerializerInfo
	{
		public Type MessageContractType;

		public bool IsWrapped;

		public bool IsResponse;

		public Type[] ExtraTypes;

		public string DefaultNamespace;

		public bool UseEncoded;

		public override bool Equals(object obj)
		{
			if (obj is CFContractSerializerInfo)
			{
				return (object)MessageContractType == ((CFContractSerializerInfo)obj).MessageContractType;
			}
			return false;
		}

		public override int GetHashCode()
		{
			if ((object)MessageContractType != null)
			{
				return MessageContractType.GetHashCode();
			}
			return ((ValueType)this).GetHashCode();
		}
	}

	protected class CFInvokeInfo
	{
		public string Action;

		public string ReplyAction;

		public bool IsOneWay;

		public bool RequestIsWrapped;

		public bool ResponseIsWrapped;

		public Type[] ExtraTypes;

		public bool UseEncoded;

		public override bool Equals(object obj)
		{
			if (obj != null && (object)obj.GetType() == typeof(CFInvokeInfo))
			{
				return Action == ((CFInvokeInfo)obj).Action;
			}
			return false;
		}

		public override int GetHashCode()
		{
			if (Action != null)
			{
				return Action.GetHashCode();
			}
			return base.GetHashCode();
		}
	}

	private Binding binding;

	private CustomBinding oneWayBinding;

	private IChannelFactory<IRequestChannel> _requestChannelFactory;

	private IChannelFactory<IOutputChannel> _outputChannelFactory;

	private ClientCredentials _clientCredentials;

	private EndpointAddress remoteAddress;

	private BindingParameterCollection _parameters;

	private object requestChannelFactorySyncObject;

	private object outputChannelFactorySyncObject;

	private Dictionary<string, XmlObjectSerializer> serializers = new Dictionary<string, XmlObjectSerializer>(2);

	public ClientCredentials ClientCredentials => _clientCredentials;

	protected BindingParameterCollection Parameters => _parameters;

	private IChannelFactory<IOutputChannel> OutputChannelFactory
	{
		get
		{
			if (_outputChannelFactory == null)
			{
				Monitor.Enter(outputChannelFactorySyncObject);
				try
				{
					if (_outputChannelFactory == null)
					{
						if (oneWayBinding == null)
						{
							oneWayBinding = new CustomBinding(binding);
						}
						if (oneWayBinding.CanBuildChannelFactory<IOutputChannel>(Parameters))
						{
							_outputChannelFactory = oneWayBinding.BuildChannelFactory<IOutputChannel>(Parameters);
							_outputChannelFactory.Open();
						}
					}
				}
				finally
				{
					Monitor.Exit(outputChannelFactorySyncObject);
				}
			}
			return _outputChannelFactory;
		}
	}

	private IChannelFactory<IRequestChannel> RequestChannelFactory
	{
		get
		{
			if (_requestChannelFactory == null)
			{
				Monitor.Enter(requestChannelFactorySyncObject);
				try
				{
					if (_requestChannelFactory == null && binding.CanBuildChannelFactory<IRequestChannel>(Parameters))
					{
						_requestChannelFactory = binding.BuildChannelFactory<IRequestChannel>(Parameters);
						_requestChannelFactory.Open();
					}
				}
				finally
				{
					Monitor.Exit(requestChannelFactorySyncObject);
				}
			}
			return _requestChannelFactory;
		}
	}

	public CFClientBase(Binding binding, EndpointAddress remoteAddress)
	{
		if (binding == null)
		{
			throw new ArgumentNullException("binding");
		}
		if (remoteAddress == null)
		{
			throw new ArgumentNullException("remoteAddress");
		}
		this.remoteAddress = remoteAddress;
		this.binding = binding;
		_clientCredentials = new ClientCredentials();
		_parameters = new BindingParameterCollection();
		_parameters.Add(_clientCredentials);
		outputChannelFactorySyncObject = new object();
		requestChannelFactorySyncObject = new object();
	}

	protected static void ApplyProtection(string action, ScopedMessagePartSpecification parts, bool protection)
	{
		MessagePartSpecification parts2 = ((!protection) ? MessagePartSpecification.NoParts : new MessagePartSpecification(isBodyIncluded: true));
		parts.AddParts(parts2, action);
	}

	protected static bool IsSecureMessageBinding(Binding binding)
	{
		if (typeof(BasicHttpBinding).IsInstanceOfType(binding))
		{
			return false;
		}
		if (typeof(CustomBinding).IsInstanceOfType(binding))
		{
			return ((CustomBinding)binding).Elements.Contains(typeof(AsymmetricSecurityBindingElement));
		}
		throw new NotSupportedException("Unsupported binding type.");
	}

	protected void Close()
	{
		if (RequestChannelFactory != null)
		{
			Monitor.Enter(RequestChannelFactory);
			try
			{
				RequestChannelFactory.Close();
			}
			finally
			{
				Monitor.Exit(RequestChannelFactory);
			}
		}
		if (OutputChannelFactory != null)
		{
			Monitor.Enter(OutputChannelFactory);
			try
			{
				OutputChannelFactory.Close();
			}
			finally
			{
				Monitor.Exit(OutputChannelFactory);
			}
		}
	}

	protected TRESPONSE Invoke<TREQUEST, TRESPONSE>(CFInvokeInfo info, TREQUEST request)
	{
		TRESPONSE result;
		using (Message msg = Message.CreateMessage(serializer: GetContractSerializer(new CFContractSerializerInfo
		{
			MessageContractType = typeof(TREQUEST),
			IsWrapped = info.RequestIsWrapped,
			ExtraTypes = info.ExtraTypes,
			UseEncoded = info.UseEncoded
		}), version: binding.MessageVersion, action: info.Action, body: request))
		{
			result = getResult<TRESPONSE>(getReply(msg), info);
		}
		ClientCredentials.ClientCertificate.Certificate.Reset();
		return result;
	}

	protected void Invoke<TREQUEST>(CFInvokeInfo info, TREQUEST request)
	{
		using Message msg = Message.CreateMessage(serializer: GetContractSerializer(new CFContractSerializerInfo
		{
			MessageContractType = typeof(TREQUEST),
			IsWrapped = info.RequestIsWrapped,
			ExtraTypes = info.ExtraTypes,
			UseEncoded = info.UseEncoded
		}), version: binding.MessageVersion, action: info.Action, body: request);
		if (info.IsOneWay)
		{
			if (_outputChannelFactory != null)
			{
				postOneWayMessage(msg);
			}
			else
			{
				getReply(msg);
			}
		}
		else
		{
			processReply(getReply(msg));
		}
	}

	private void postOneWayMessage(Message msg)
	{
		if (OutputChannelFactory == null)
		{
			throw new NotSupportedException();
		}
		Monitor.Enter(OutputChannelFactory);
		IOutputChannel outputChannel;
		try
		{
			outputChannel = OutputChannelFactory.CreateChannel(remoteAddress);
		}
		finally
		{
			Monitor.Exit(OutputChannelFactory);
		}
		outputChannel.Open();
		try
		{
			outputChannel.Send(msg);
		}
		finally
		{
			outputChannel.Close();
		}
	}

	private Message getReply(Message msg)
	{
		if (RequestChannelFactory == null)
		{
			throw new NotSupportedException();
		}
		Monitor.Enter(RequestChannelFactory);
		IRequestChannel requestChannel;
		try
		{
			requestChannel = RequestChannelFactory.CreateChannel(remoteAddress);
		}
		finally
		{
			Monitor.Exit(RequestChannelFactory);
		}
		requestChannel.Open();
		try
		{
			return requestChannel.Request(msg);
		}
		finally
		{
			if (requestChannel.State == CommunicationState.Opened)
			{
				requestChannel.Close();
			}
			else
			{
				Log.Warning(Module.BackendCommunication, $"Can't close request channel, state = {requestChannel.State.ToString()}");
			}
		}
	}

	private void processReply(Message reply)
	{
		if (reply.IsFault)
		{
			XmlDictionaryReader readerAtBodyContents = reply.GetReaderAtBodyContents();
			try
			{
				throw new CFFaultException(readerAtBodyContents.ReadOuterXml());
			}
			finally
			{
				readerAtBodyContents.Close();
			}
		}
	}

	protected virtual XmlObjectSerializer GetContractSerializer(CFContractSerializerInfo info)
	{
		Monitor.Enter(serializers);
		XmlObjectSerializer xmlObjectSerializer;
		try
		{
			if (serializers.ContainsKey(info.MessageContractType.Name))
			{
				xmlObjectSerializer = serializers[info.MessageContractType.Name];
			}
			else
			{
				xmlObjectSerializer = new CFContractSerializer(info);
				serializers[info.MessageContractType.Name] = xmlObjectSerializer;
			}
		}
		finally
		{
			Monitor.Exit(serializers);
		}
		return xmlObjectSerializer;
	}

	private TRESPONSE getResult<TRESPONSE>(Message reply, CFInvokeInfo info)
	{
		processReply(reply);
		TRESPONSE result = default(TRESPONSE);
		if (!reply.IsEmpty)
		{
			return reply.GetBody<TRESPONSE>(GetContractSerializer(new CFContractSerializerInfo
			{
				MessageContractType = typeof(TRESPONSE),
				IsWrapped = info.ResponseIsWrapped,
				IsResponse = true,
				ExtraTypes = info.ExtraTypes,
				UseEncoded = info.UseEncoded
			}));
		}
		return result;
	}
}
