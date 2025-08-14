using System;
using System.IO;
using System.Net;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using WebServerHost.Helpers;
using WebServerHost.Web;
using WebServerHost.Web.Http;

namespace WebServerHost.Handlers;

public class ResourceHandler : RequestHandler
{
	private WebServerConfiguration configuration;

	public ResourceHandler(WebServerConfiguration configuration)
	{
		this.configuration = configuration;
	}

	public override ShcWebResponse HandleRequest(ShcWebRequest request)
	{
		ShcWebResponse shcWebResponse = null;
		string text = "/resources/";
		if (request.RequestUri.StartsWith(text))
		{
			string text2 = request.RequestUri.Substring(text.Length, request.RequestUri.Length - text.Length);
			switch (text2)
			{
			case "":
				return HandleEmptyRequest(request);
			case "@bundle/custom/":
				return HandleBundleCustom(request);
			case "hash":
				return HandleHashRequest(request);
			default:
			{
				string value = string.Empty;
				request.Headers.TryGetValue("Accept".ToLower(), out value);
				if (!string.IsNullOrEmpty(value))
				{
					return HandlePluginResourceRequest(text2);
				}
				return new ShcErrorResponse(HttpStatusCode.NotFound, request.RequestContent);
			}
			}
		}
		ResourceRequest resourceRequest = new ResourceRequest(request, configuration);
		if (resourceRequest.Directory == Path.Combine(configuration.ServerRoot, "capture"))
		{
			return GetCapturedImages(resourceRequest);
		}
		if (!Directory.Exists(resourceRequest.Directory))
		{
			return GetErrorResponse(HttpStatusCode.NotFound, "Directory not found.");
		}
		if (Directory.Exists(resourceRequest.Directory) && string.IsNullOrEmpty(resourceRequest.FileName))
		{
			return GetErrorResponse(HttpStatusCode.Forbidden, "Directory does not allow contents to be listed.");
		}
		if (!File.Exists(resourceRequest.FullPath))
		{
			return GetErrorResponse(HttpStatusCode.NotFound, "File not found.");
		}
		return new ResourceResponse(resourceRequest.FullPath);
	}

	private ShcWebResponse GetCapturedImages(ResourceRequest resRequest)
	{
		IImageService imageService = ServiceProvider.Services.Get<IImageService>();
		if (imageService != null)
		{
			string text = imageService.ResolveImageUrl(resRequest.FileName);
			if (File.Exists(text))
			{
				return new ResourceResponse(text);
			}
		}
		return GetErrorResponse(HttpStatusCode.NotFound, "File not found.");
	}

	private ShcWebResponse GetErrorResponse(HttpStatusCode statusCode, string message)
	{
		return new ShcErrorResponse(statusCode, message);
	}

	private ResourceResponse HandlePluginResourceRequest(string resourceDesc)
	{
		if (resourceDesc.Contains("%20"))
		{
			resourceDesc = resourceDesc.Replace("%20", " ");
		}
		string[] array = resourceDesc.Split('/');
		string resourcePath = string.Format("{0}\\plugins\\{1}@{2}{3}\\{4}", configuration.ServerRoot, array[0], array[1], "\\resources\\default", string.Join("/", array, 3, array.Length - 3));
		return new ResourceResponse(resourcePath);
	}

	private ShcWebResponse HandleBundleCustom(ShcWebRequest request)
	{
		return new ShcPluginBundleResponse(request.Parameters, configuration.ServerRoot);
	}

	private ShcWebResponse HandleEmptyRequest(ShcWebRequest request)
	{
		try
		{
			return new ResourceResponse(PluginsRepo.GetPuginsRepoFilePath(configuration));
		}
		catch (Exception ex)
		{
			Log.Error(Module.WebServerHost, $"Error occurred while processing UI plugins reporsitory: {ex.Message}", isPersisted: true);
		}
		return new ShcRestResponse(HttpStatusCode.InternalServerError, "Plugin repo not available.");
	}

	private ShcRestResponse HandleHashRequest(ShcWebRequest request)
	{
		try
		{
			string jsonContent = $"{{\"hash\":\"{PluginsRepo.GetPluginsHash(configuration)}\"}}";
			return new ShcRestResponse(HttpStatusCode.OK, jsonContent);
		}
		catch (Exception ex)
		{
			Log.Error(Module.WebServerHost, $"Error occurred while processing UI plugins reporsitory: {ex.Message}", isPersisted: true);
		}
		return new ShcRestResponse(HttpStatusCode.InternalServerError, "Plugin repo not available.");
	}
}
