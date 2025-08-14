using System;
using System.Collections.Generic;
using System.Net;
using RWE.SmartHome.SHC.CommonFunctionality;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.Generic.LogManager;
using WebServerHost.Web;
using WebServerHost.Web.Extensions;
using WebServerHost.Web.Http;

namespace WebServerHost.Handlers;

public class WebSocketHandler : RequestHandler
{
	private readonly ILogger logger = LogManager.Instance.GetLogger<WebSocketHandler>();

	private readonly string endpointBase;

	public WebSocketHandler(string endpointBase)
	{
		this.endpointBase = endpointBase;
	}

	protected override void Authorize(ShcWebRequest request)
	{
		if (base.Authorization != null)
		{
			if (!request.Parameters.TryGetValueIgnoreCase("token", out var value))
			{
				throw new ApiException(ErrorCode.InvalidTokenRequest, "No authorization token not provided");
			}
			base.Authorization.Authorize(value);
		}
	}

	public override ShcWebResponse HandleRequest(ShcWebRequest request)
	{
		try
		{
			if (request.RequestUri.StartsWith(endpointBase))
			{
				Authorize(request);
				string route = request.RequestUri.Substring(endpointBase.Length, request.RequestUri.Length - endpointBase.Length).Trim('/');
				return HandleRequest(request, route);
			}
		}
		catch (ApiException apiError)
		{
			return new RestApiErrorResponse(apiError);
		}
		return null;
	}

	private ShcWebResponse HandleRequest(ShcWebRequest request, string route)
	{
		request.GetHeaderValue("Sec-WebSocket-Version");
		request.GetHeaderValue("Sec-WebSocket-Extensions");
		string headerValue = request.GetHeaderValue("Sec-WebSocket-Key");
		string headerValue2 = request.GetHeaderValue("Upgrade");
		if (headerValue2.ToLower() == "websocket")
		{
			return new ShcWebsocketResponse(headerValue);
		}
		return new ShcErrorResponse(HttpStatusCode.BadRequest, "Not a websocket request");
	}

	public void OnNewWebSocket(WebClientConnection ws)
	{
		try
		{
			Event obj = new Event();
			obj.Type = "ControllerConnectivityChanged";
			obj.Timestamp = DateTime.Now;
			obj.Link = "System";
			obj.SequenceNumber = -1;
			obj.Namespace = "core.RWE";
			obj.Properties = new List<Property>
			{
				new Property
				{
					Name = "IsConnected",
					Value = true
				},
				new Property
				{
					Name = "SerialNumber",
					Value = SHCSerialNumber.SerialNumber()
				}
			};
			Event obj2 = obj;
			ws.SendTextOnWebSocket(obj2.ToJson());
		}
		catch (Exception exception)
		{
			logger.Error("Error sending connectivity event on websocket", exception);
		}
	}

	public void HandleWsMessage(WebClientConnection ws, string message)
	{
		if (message.Contains("hbeat") && message.Contains("ping"))
		{
			ws.SendTextOnWebSocket("{\"hbeat\":\"pong\"}");
		}
	}
}
