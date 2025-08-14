using System.Text.RegularExpressions;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Helpers;

public class ResponseData
{
	public static StringCollection AddXmlResponseTags(SerializationResponse serializationResponse)
	{
		return AddXmlResponseTags(serializationResponse.ResponseObject, serializationResponse.ResponseBody);
	}

	public static StringCollection AddXmlResponseTags(BaseResponse responseObject, StringCollection responseCollection)
	{
		string text = ResponseHelper.SerializeResponse(responseObject);
		string pattern = "<(?<rootTag>[a-zA-Z].*?)( .*?)?>";
		Match match = Regex.Match(text, pattern);
		if (match.Success)
		{
			string value = match.Groups["rootTag"].Value;
			responseCollection.InsertTop(match.Value);
			responseCollection.Add($"</{value}>");
		}
		else
		{
			Log.Error(Module.ExternalCommandDispatcher, $"Cannot identify response root tag, will return default response: {responseObject}");
			responseCollection = new StringCollection(text);
		}
		return responseCollection;
	}
}
