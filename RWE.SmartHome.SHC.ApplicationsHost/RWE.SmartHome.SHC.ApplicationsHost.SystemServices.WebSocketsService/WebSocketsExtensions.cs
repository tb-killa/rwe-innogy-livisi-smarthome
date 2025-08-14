using RWE.SmartHome.SHC.WebSocketsService.Client;
using SmartHome.SHC.API.SystemServices.WebSocketsService;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.WebSocketsService;

internal static class WebSocketsExtensions
{
	internal static RWE.SmartHome.SHC.WebSocketsService.Client.WSOptions ToWSOptions(this global::SmartHome.SHC.API.SystemServices.WebSocketsService.WSOptions source)
	{
		RWE.SmartHome.SHC.WebSocketsService.Client.WSOptions wSOptions = new RWE.SmartHome.SHC.WebSocketsService.Client.WSOptions();
		wSOptions.AcceptEncoding = source.AcceptEncoding;
		wSOptions.AcceptLanguage = source.AcceptLanguage;
		wSOptions.ActivityTimeout = source.ActivityTimeout;
		wSOptions.ActivityTimerEnabled = source.ActivityTimerEnabled;
		wSOptions.CacheControl = source.CacheControl;
		wSOptions.CloseMsgTimeout = source.CloseMsgTimeout;
		wSOptions.Cookies = source.Cookies;
		wSOptions.Extensions = source.Extensions;
		wSOptions.HandshakeTimeout = source.HandshakeTimeout;
		wSOptions.MaskingEnabled = source.MaskingEnabled;
		wSOptions.MaxReceiveFrameLength = source.MaxReceiveFrameLength;
		wSOptions.MaxSendQueueSize = source.MaxSendQueueSize;
		wSOptions.Origin = source.Origin;
		wSOptions.PingRespTimeout = source.PingRespTimeout;
		wSOptions.ReceiveTimeout = source.ReceiveTimeout;
		wSOptions.SubProtocol = source.SubProtocol;
		wSOptions.UserAgent = source.UserAgent;
		return wSOptions;
	}

	internal static global::SmartHome.SHC.API.SystemServices.WebSocketsService.WebSocketState ToAPIWebSocketState(this RWE.SmartHome.SHC.WebSocketsService.Client.WebSocketState source)
	{
		return source switch
		{
			RWE.SmartHome.SHC.WebSocketsService.Client.WebSocketState.Connected => global::SmartHome.SHC.API.SystemServices.WebSocketsService.WebSocketState.Connected, 
			RWE.SmartHome.SHC.WebSocketsService.Client.WebSocketState.Connecting => global::SmartHome.SHC.API.SystemServices.WebSocketsService.WebSocketState.Connecting, 
			RWE.SmartHome.SHC.WebSocketsService.Client.WebSocketState.Disconnected => global::SmartHome.SHC.API.SystemServices.WebSocketsService.WebSocketState.Disconnected, 
			RWE.SmartHome.SHC.WebSocketsService.Client.WebSocketState.Disconnecting => global::SmartHome.SHC.API.SystemServices.WebSocketsService.WebSocketState.Disconnecting, 
			RWE.SmartHome.SHC.WebSocketsService.Client.WebSocketState.Initialized => global::SmartHome.SHC.API.SystemServices.WebSocketsService.WebSocketState.Initialized, 
			_ => global::SmartHome.SHC.API.SystemServices.WebSocketsService.WebSocketState.Disconnected, 
		};
	}
}
