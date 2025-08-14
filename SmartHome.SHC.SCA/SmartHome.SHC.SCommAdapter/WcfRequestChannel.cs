using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using Rebex;
using Rebex.Net;
using Rebex.Security.Certificates;

namespace SmartHome.SHC.SCommAdapter;

internal class WcfRequestChannel : WcfChannelBase, IRequestChannel, IChannel, ICommunicationObject
{
	private const int MAX_MESSAGE_SIZE = 2097152;

	private const int EXPECTED_MESSAGE_SIZE = 32768;

	private readonly WcfBinding binding;

	private readonly BufferManager bufferManager;

	private readonly Uri via;

	private readonly BindingContext context;

	private readonly ILogWriter logger;

	public Uri Via => via;

	public WcfRequestChannel(ChannelManagerBase manager, WcfBinding binding, MessageEncoderFactory encoderFactory, EndpointAddress address, Uri via, BindingContext context)
		: base(manager, encoderFactory, address)
	{
		this.binding = binding;
		this.via = via;
		this.context = context;
		logger = binding.RequestCreator.LogWriter;
		bufferManager = BufferManager.CreateBufferManager(binding.GetElement<TransportBindingElement>()?.MaxReceivedMessageSize ?? 2097152, 32768);
		CheckCertificateRequestCreator(context);
	}

	public Message Request(Message message)
	{
		return Request(message, base.DefaultReceiveTimeout);
	}

	public IAsyncResult BeginRequest(Message message, TimeSpan timeout, AsyncCallback callback, object state)
	{
		throw new NotImplementedException();
	}

	public IAsyncResult BeginRequest(Message message, AsyncCallback callback, object state)
	{
		throw new NotImplementedException();
	}

	public Message EndRequest(IAsyncResult result)
	{
		throw new NotImplementedException();
	}

	public Message Request(Message message, TimeSpan timeout)
	{
		ThrowIfDisposedOrNotOpen();
		HttpRequest httpRequest = CreateRequest(message, timeout);
		message.Headers.Clear();
		TriggerBeforeSendingMessage(message);
		AddBodyToRequest(httpRequest, message);
		WcfMessage wcfMessage;
		try
		{
			using Stream contentStream = httpRequest.GetResponse().GetResponseStream();
			wcfMessage = new WcfMessage(binding.MessageVersion, contentStream);
			TriggerAfterSendingMessage(wcfMessage);
		}
		catch (WebException ex)
		{
			if (ex.Response == null)
			{
				throw new WebException(GetExceptionMessage(ex), ex);
			}
			Stream stream = null;
			try
			{
				stream = ex.Response.GetResponseStream();
				wcfMessage = new WcfMessage(binding.MessageVersion, stream);
			}
			catch
			{
				wcfMessage = null;
			}
			finally
			{
				stream?.Close();
			}
			if (wcfMessage == null || wcfMessage.IsEmpty || !wcfMessage.IsFault)
			{
				throw new WebException(GetExceptionMessage(ex), ex);
			}
		}
		Log(wcfMessage);
		if (wcfMessage.IsFault)
		{
			throw new CommunicationObjectFaultedException(wcfMessage.FaultReason);
		}
		return wcfMessage;
	}

	private void TriggerBeforeSendingMessage(Message message)
	{
		foreach (IWcfMessageObserver messageObserver in binding.MessageObservers)
		{
			messageObserver.BeforeSendingMessage(message);
		}
	}

	private void TriggerAfterSendingMessage(WcfMessage responseMessage)
	{
		if (binding.MessageObservers.Count <= 0)
		{
			return;
		}
		IList<WcfMessageHeader> headers = WcfMessageHeader.FromMessageHeaders(responseMessage.Headers);
		foreach (IWcfMessageObserver messageObserver in binding.MessageObservers)
		{
			messageObserver.AfterReceivedMessage(responseMessage, headers);
		}
	}

	private HttpRequest CreateRequest(Message message, TimeSpan timeout)
	{
		HttpRequest httpRequest = binding.RequestCreator.Create(via);
		httpRequest.Method = "POST";
		httpRequest.Timeout = (int)Math.Ceiling(timeout.TotalMilliseconds);
		httpRequest.Headers.Add("SOAPAction", $"\"{message.Headers.Action}\"");
		httpRequest.Headers.Add("Content-Type", "text/xml; charset=utf-8");
		return httpRequest;
	}

	private void AddBodyToRequest(HttpRequest request, Message message)
	{
		using Stream stream = request.GetRequestStream();
		ArraySegment<byte> arraySegment = default(ArraySegment<byte>);
		try
		{
			arraySegment = encoder.WriteMessage(message, 2097152, bufferManager);
			stream.Write(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
		}
		finally
		{
			if (arraySegment != default(ArraySegment<byte>))
			{
				bufferManager.ReturnBuffer(arraySegment.Array);
			}
		}
	}

	private void Log(WcfMessage responseMessage)
	{
		if (logger != null)
		{
			logger.Write(LogLevel.Debug, null, 0, "SOAP", responseMessage.ToString());
		}
	}

	private void CheckCertificateRequestCreator(BindingContext context)
	{
		BindingParameterCollection bindingParameters = context.BindingParameters;
		HttpRequestCreator requestCreator = binding.RequestCreator;
		if (requestCreator.Settings.SslClientCertificateRequestHandler == CertificateRequestHandler.NoCertificate && bindingParameters.Count > 0)
		{
			ClientCredentials clientCredentials = bindingParameters.OfType<ClientCredentials>().FirstOrDefault();
			if (clientCredentials != null)
			{
				ICertificateRequestHandler sslClientCertificateRequestHandler = CertificateRequestHandler.CreateRequestHandler(new Certificate(clientCredentials.ClientCertificate.Certificate));
				requestCreator.Settings.SslClientCertificateRequestHandler = sslClientCertificateRequestHandler;
			}
		}
	}

	private static string GetExceptionMessage(Exception ex)
	{
		string text = null;
		StringBuilder stringBuilder = new StringBuilder();
		while (ex != null)
		{
			if (text != ex.Message)
			{
				text = ex.Message;
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(text);
				}
				else
				{
					stringBuilder.AppendFormat(" --> {0}", new object[1] { text });
				}
			}
			ex = ex.InnerException;
		}
		return stringBuilder.ToString();
	}
}
