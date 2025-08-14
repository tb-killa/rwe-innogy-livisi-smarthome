using System;
using System.IO;
using System.Xml;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API.Serializer;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.WebServiceHost;

public class RequestDeserializer
{
	private static readonly string ProbeShcRequestType = typeof(ProbeShcRequest).Name;

	private static readonly string SoftwareUpdateRequestType = typeof(SoftwareUpdateRequest).Name;

	private static readonly string LoginRequestType = typeof(LoginRequest).Name;

	private static readonly string BaseRequestType = typeof(BaseRequest).Name;

	private static readonly BaseClassSerializer<BaseRequest> RequestSerializer = new BaseClassSerializer<BaseRequest>();

	public BaseRequest GetRequestAndCheckVersion(string requestText, out string errorResponse)
	{
		using TextReader input = new StringReader(requestText);
		using XmlReader xmlReader = XmlReader.Create(input);
		xmlReader.MoveToContent();
		if (xmlReader.IsStartElement(BaseRequestType))
		{
			string attribute = xmlReader.GetAttribute("Version");
			string attribute2 = xmlReader.GetAttribute("xsi:type");
			if (attribute2 != ProbeShcRequestType && attribute2 != SoftwareUpdateRequestType && attribute2 != LoginRequestType && attribute != "3.00")
			{
				errorResponse = CreateErrorResponse(xmlReader.GetAttribute("RequestId"), attribute);
			}
		}
		errorResponse = null;
		try
		{
			return RequestSerializer.Deserialize(xmlReader);
		}
		catch (Exception ex)
		{
			errorResponse = CreateErrorResponse(string.Empty, string.Empty);
			Log.Warning(Module.RelayDriver, "Failed to deserialize request" + requestText);
			Log.Debug(Module.RelayDriver, " Error: " + ex.ToString());
		}
		return null;
	}

	private static string CreateErrorResponse(string requestId, string detectedVersion)
	{
		Guid requestId2 = Guid.Empty;
		if (!string.IsNullOrEmpty(requestId))
		{
			try
			{
				requestId2 = new Guid(requestId);
			}
			catch (FormatException)
			{
			}
		}
		return ResponseHelper.SerializeErrorResponse(requestId2, ErrorResponseType.VersionMismatchError, new StringProperty("DetectedVersion", detectedVersion), new StringProperty("ExpectedVersion", "3.00"));
	}
}
