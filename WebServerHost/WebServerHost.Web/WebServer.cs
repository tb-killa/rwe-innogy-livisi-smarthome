using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using Rebex.Net;
using Rebex.Security.Certificates;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.Generic.LogManager;
using WebServerHost.Handlers;
using WebServerHost.Web.Http;

namespace WebServerHost.Web;

public class WebServer
{
	private ILogger logger = LogManager.Instance.GetLogger<WebServer>();

	private bool serverStop;

	private bool running;

	private WebServerConfiguration Configuration;

	private WebClientConnectionsManager clientManager;

	private RequestHandler webContentHandler;

	private RequestHandler[] webApplicationHandlers;

	private WebSocketHandler wsHandler;

	private TcpListener webContentListener;

	private Thread webContentThread;

	private TcpListener webAppListener;

	private Thread webAppThread;

	private TlsParameters tlsParams;

	private readonly IRegistrationService registrationService;

	public bool Running => running;

	public WebServer(WebServerConfiguration webServerConf, RequestHandler handler, List<RequestHandler> appHandlers)
	{
		Configuration = webServerConf;
		webContentHandler = handler;
		webApplicationHandlers = appHandlers.ToArray();
		wsHandler = appHandlers.OfType<WebSocketHandler>().FirstOrDefault();
		registrationService = ServiceProvider.Services.Get<IRegistrationService>();
	}

	public void Start()
	{
		try
		{
			if (File.Exists(Configuration.CertificatePath))
			{
				logger.Info("Found certificate for HTTPS, initializing security settings");
				InitializeSecurityParameters();
			}
			clientManager = new WebClientConnectionsManager(Configuration);
			webContentListener = new TcpListener(Configuration.IPAddress, Configuration.Port);
			webContentListener.Start();
			webContentThread = new Thread(ListenWebContent);
			webAppListener = new TcpListener(Configuration.IPAddress, Configuration.ApplicationPort);
			webAppListener.Start();
			webAppThread = new Thread(ListenApp);
			serverStop = false;
			webContentThread.Start();
			webAppThread.Start();
			running = true;
			logger.Info("Server Started");
		}
		catch (Exception exception)
		{
			logger.Error("Server could not start", exception);
		}
	}

	private void InitializeSecurityParameters()
	{
		Certificate cert = Certificate.LoadPfx(Configuration.CertificatePath, Configuration.CertifictePassword);
		tlsParams = new TlsParameters();
		tlsParams.Entity = TlsConnectionEnd.Server;
		tlsParams.AllowedSuites = TlsCipherSuite.Fast;
		tlsParams.PreferredHashAlgorithm = SignatureHashAlgorithm.SHA256;
		tlsParams.Version = TlsVersion.Any;
		tlsParams.Certificate = CertificateChain.BuildFrom(cert);
		tlsParams.CertificatePolicy = TlsCertificatePolicy.NoClientCertificate;
	}

	private void ListenWebContent()
	{
		while (!serverStop)
		{
			WebClientConnection webClientConnection = null;
			try
			{
				TcpClient tcpClient = webContentListener.AcceptTcpClient();
				logger.Info($"Client Connected {tcpClient.Client.RemoteEndPoint.ToString()}");
				webClientConnection = ((tlsParams == null) ? new WebClientConnection(tcpClient) : new WebClientConnection(tcpClient, tlsParams));
				clientManager.Add(webClientConnection, HandleWebContent);
			}
			catch (TlsException message)
			{
				logger.Error(message);
				if (webClientConnection != null)
				{
					clientManager.Remove(webClientConnection);
				}
			}
			catch (Exception)
			{
			}
		}
	}

	private void ListenApp()
	{
		while (!serverStop)
		{
			WebClientConnection webClientConnection = null;
			try
			{
				TcpClient tcpClient = webAppListener.AcceptTcpClient();
				logger.Info($"New client connection {tcpClient.Client.RemoteEndPoint.ToString()}");
				webClientConnection = ((tlsParams == null) ? new WebClientConnection(tcpClient) : new WebClientConnection(tcpClient, tlsParams));
				clientManager.Add(webClientConnection, HandleApp);
			}
			catch (TlsException message)
			{
				logger.Error(message);
				if (webClientConnection != null)
				{
					clientManager.Remove(webClientConnection);
				}
			}
			catch (Exception)
			{
			}
		}
	}

	private void HandleWebContent(WebClientConnection client, object request)
	{
		HandleWebRequest(client, (ShcWebRequest)request, webContentHandler);
	}

	private void HandleApp(WebClientConnection client, object request)
	{
		HandleWebRequest(client, (ShcWebRequest)request, webApplicationHandlers);
	}

	private void HandleWebRequest(WebClientConnection client, ShcWebRequest request, params RequestHandler[] handlers)
	{
		logger.Debug($"{client.RemoteEndPoint}: new request {request.ProtocolVersion} {request.Method} {request.RequestUri}");
		ShcWebResponse shcWebResponse = null;
		if (request.Method == "OPTIONS")
		{
			shcWebResponse = HandleOptionsRequest(client);
			SendResponse(client, shcWebResponse);
		}
		else if (SuportedMethod(request))
		{
			foreach (RequestHandler requestHandler in handlers)
			{
				shcWebResponse = requestHandler.HandleRequest(request);
				if (shcWebResponse != null)
				{
					PreapareResponse(request, shcWebResponse);
					SendResponse(client, shcWebResponse);
					if (shcWebResponse is ShcWebsocketResponse)
					{
						client.UpgradeToWebSocket();
						clientManager.Add(client, HandleWebSocketMessage);
						OnNewWebsocketConnected(client);
					}
					break;
				}
			}
		}
		if (shcWebResponse == null)
		{
			logger.Warn($"Client Request {request.Method} {request.RequestUri} not handled");
			clientManager.Remove(client);
			return;
		}
		logger.Debug($"{client.RemoteEndPoint} responded to request  {shcWebResponse.StatusCode} {request.ProtocolVersion} {request.Method} {request.RequestUri}");
		if (ShouldCloseConnection(client, request))
		{
			clientManager.Remove(client);
		}
	}

	private bool ShouldCloseConnection(WebClientConnection client, ShcWebRequest request)
	{
		if (client.isWebSocket)
		{
			return false;
		}
		DateTime now = DateTime.Now;
		if (IsKeepAliveHeaderSet(request) && client.RequestsCount < Configuration.KeepAliveMaxRequests && now - client.LastActivity < TimeSpan.FromSeconds(Configuration.KeepAliveTimeout))
		{
			return false;
		}
		return true;
	}

	private bool IsKeepAliveHeaderSet(ShcWebRequest request)
	{
		string value = null;
		request.Headers.TryGetValue("connection", out value);
		return value?.EqualsIgnoreCase("keep-alive") ?? false;
	}

	private ShcWebResponse HandleOptionsRequest(WebClientConnection client)
	{
		ShcHttpWebResponse shcHttpWebResponse = new ShcHttpWebResponse(HttpStatusCode.OK, null, null, "");
		shcHttpWebResponse.Headers["Access-Control-Allow-Origin"] = "*";
		shcHttpWebResponse.Headers["Allow"] = HttpMethod.AllowedMethods;
		shcHttpWebResponse.Headers["Access-Control-Allow-Headers"] = "authorization,content-type";
		shcHttpWebResponse.Headers["Access-Control-Max-Age"] = "86400";
		shcHttpWebResponse.Headers["Expires"] = "-1";
		return shcHttpWebResponse;
	}

	private bool SuportedMethod(ShcWebRequest request)
	{
		if (HttpMethod.IsValid(request.Method))
		{
			return true;
		}
		logger.Error($"Error: {request.Method} method not supported");
		return false;
	}

	private void PreapareResponse(ShcWebRequest request, ShcWebResponse response)
	{
		response.Version = request.ProtocolVersion;
		response.SetHeader("Server", request.Headers["host"]);
		if (IsKeepAliveHeaderSet(request))
		{
			response.Headers["Keep-Alive"] = $"timeout={Configuration.KeepAliveTimeout}, max={Configuration.KeepAliveMaxRequests}";
		}
	}

	private void SendResponse(WebClientConnection client, ShcWebResponse response)
	{
		if (response is IStreamResponse)
		{
			using (IStreamResponse streamResponse = response as IStreamResponse)
			{
				byte[] headerBytes = streamResponse.GetHeaderBytes();
				SendToClient(client, headerBytes, headerBytes.Length);
				SendStream(client, streamResponse.GetStream());
				return;
			}
		}
		byte[] bytes = response.GetBytes();
		SendToClient(client, bytes, bytes.Length);
	}

	private void SendStream(WebClientConnection client, Stream stream)
	{
		byte[] array = new byte[16384];
		while (true)
		{
			int num = stream.Read(array, 0, array.Length);
			if (num > 0)
			{
				SendToClient(client, array, num);
				continue;
			}
			break;
		}
	}

	private void SendToClient(WebClientConnection client, byte[] data, int bytesTosend)
	{
		try
		{
			if (client.Connected)
			{
				client.Send(data, 0, bytesTosend, SocketFlags.None);
			}
			else
			{
				logger.Warn("Connection lost");
			}
		}
		catch (SocketException)
		{
			logger.Warn("Client disconnected while sending response");
			clientManager.Remove(client);
		}
		catch (Exception ex2)
		{
			logger.Error("Error while sending response: " + ex2.ToString());
		}
	}

	private void HandleWebSocketMessage(WebClientConnection client, object message)
	{
		try
		{
			wsHandler.HandleWsMessage(client, (string)message);
		}
		catch (Exception)
		{
			logger.Debug($"Releasing closed websocket (remote {client.RemoteEndPoint}");
			clientManager.Remove(client);
		}
	}

	public void SendOnWebsockets(string message)
	{
		clientManager.SendOnWebsockets(message);
	}

	private void OnNewWebsocketConnected(WebClientConnection client)
	{
		try
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				wsHandler.OnNewWebSocket(client);
			});
		}
		catch (Exception exception)
		{
			logger.Error("Error handling new ws connection", exception);
		}
	}

	public void Stop()
	{
		try
		{
			if (webContentThread != null)
			{
				serverStop = true;
				webContentListener.Stop();
				webAppListener.Stop();
				webContentThread.Join(1000);
				webAppThread.Join(1000);
				running = false;
				clientManager.Dispose();
				clientManager = null;
				logger.Info("Server Stopped");
			}
		}
		catch (Exception exception)
		{
			logger.Error("Error on stopping server", exception);
		}
	}
}
