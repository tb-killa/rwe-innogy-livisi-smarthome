using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.ExternalCommandDispatcher.Helpers;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher;

public class CommandHandlersManager : IService
{
	private readonly List<ICommandHandler> commandHandlers;

	private readonly List<ISerializedResponseHandler> serializedResponseHandlers;

	public CommandHandlersManager()
	{
		commandHandlers = new List<ICommandHandler>();
		serializedResponseHandlers = new List<ISerializedResponseHandler>();
	}

	public void Initialize()
	{
		foreach (ICommandHandler commandHandler in commandHandlers)
		{
			commandHandler.Initialize();
		}
		foreach (ISerializedResponseHandler serializedResponseHandler in serializedResponseHandlers)
		{
			serializedResponseHandler.Initialize();
		}
	}

	public void Uninitialize()
	{
		foreach (ICommandHandler commandHandler in commandHandlers)
		{
			commandHandler.Uninitialize();
		}
		foreach (ISerializedResponseHandler serializedResponseHandler in serializedResponseHandlers)
		{
			serializedResponseHandler.Uninitialize();
		}
	}

	public void AddCommandHandler(ICommandHandler commandHandler)
	{
		commandHandlers.Add(commandHandler);
	}

	public void AddSerializedResponseHandler(ISerializedResponseHandler serializedResponseHandler)
	{
		serializedResponseHandlers.Add(serializedResponseHandler);
	}

	public BaseResponse HandleResponders(ChannelContext context, BaseRequest request, ref Action postSendAction)
	{
		BaseResponse baseResponse = HandleBaseResponders(context, request, ref postSendAction);
		if (baseResponse != null)
		{
			return baseResponse;
		}
		return HandleSerializedResponders(context, request, ref postSendAction);
	}

	private BaseResponse HandleBaseResponders(ChannelContext context, BaseRequest request, ref Action postSendAction)
	{
		BaseResponse baseResponse = null;
		foreach (ICommandHandler commandHandler in commandHandlers)
		{
			baseResponse = commandHandler.HandleRequest(context, request, ref postSendAction);
			if (baseResponse != null)
			{
				baseResponse.CorrespondingRequestId = request.RequestId;
				break;
			}
		}
		return baseResponse;
	}

	private BaseResponse HandleSerializedResponders(ChannelContext context, BaseRequest request, ref Action postSendAction)
	{
		SerializationResponse serializationResponse = null;
		foreach (ISerializedResponseHandler serializedResponseHandler in serializedResponseHandlers)
		{
			serializationResponse = serializedResponseHandler.HandleRequest(context, request, ref postSendAction);
			if (serializationResponse != null)
			{
				break;
			}
		}
		return serializationResponse.ResponseObject;
	}

	public StringCollection HandleSerializedRespondersSerialized(ChannelContext context, BaseRequest request, ref Action postSendAction)
	{
		SerializationResponse serializationResponse = null;
		foreach (ISerializedResponseHandler serializedResponseHandler in serializedResponseHandlers)
		{
			serializationResponse = serializedResponseHandler.HandleRequest(context, request, ref postSendAction);
			if (serializationResponse != null)
			{
				break;
			}
		}
		if (serializationResponse != null)
		{
			serializationResponse.ResponseObject.CorrespondingRequestId = request.RequestId;
			return ResponseData.AddXmlResponseTags(serializationResponse);
		}
		return null;
	}
}
