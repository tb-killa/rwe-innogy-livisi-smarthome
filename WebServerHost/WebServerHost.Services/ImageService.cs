using System.Collections.Generic;
using System.IO;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;
using SmartHome.Common.API.Entities.Data;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.Generic.LogManager;
using WebServerHost.Helpers;
using WebServerHost.Web.Extensions;

namespace WebServerHost.Services;

public class ImageService : IImageService
{
	private const string ImageMetadataNamePrefix = "imgfn";

	private static ILogger log = LogManager.Instance.GetLogger<ImageService>();

	private readonly IApplicationsSettings appSettings;

	private readonly IShcClient shcClient;

	public ImageService(IApplicationsSettings appSettings, IShcClient shcClient)
	{
		this.appSettings = appSettings;
		this.shcClient = shcClient;
	}

	public List<Image> GetAllImages()
	{
		return (from i in GetAllImagesMetadata().Select(ToImage)
			orderby i.Timestamp descending
			select i).ToList();
	}

	private Image ToImage(ImageMetaData imgData)
	{
		Image image = new Image();
		image.Id = Path.GetFileNameWithoutExtension(imgData.FileName);
		image.Metadata = CollectMetadata(imgData);
		image.Timestamp = imgData.CaptureTime;
		image.ImageUrl = GenerateImageUrl(imgData.FileName);
		image.IsLiveImage = imgData.IsTriggeredFromUi;
		return image;
	}

	private List<ImageMetaData> GetAllImagesMetadata()
	{
		List<ImageMetaData> list = new List<ImageMetaData>();
		IEnumerable<ConfigurationItem> enumerable = from s in appSettings.GetAllSettings()
			where s.Name.StartsWith("imgfn")
			select s;
		foreach (ConfigurationItem item in enumerable)
		{
			ImageMetaData imageMetaData = item.Value.FromJson<ImageMetaData>();
			if (IsUsbConnected())
			{
				if (string.Equals(Path.GetDirectoryName(imageMetaData.FileName), "Hard Disk\\data"))
				{
					list.Add(imageMetaData);
				}
			}
			else if (!string.Equals(Path.GetDirectoryName(imageMetaData.FileName), "Hard Disk\\data"))
			{
				list.Add(imageMetaData);
			}
		}
		return list;
	}

	private List<SmartHome.Common.API.Entities.Entities.Property> CollectMetadata(ImageMetaData imageMetadata)
	{
		List<SmartHome.Common.API.Entities.Entities.Property> list = new List<SmartHome.Common.API.Entities.Entities.Property>();
		list.Add(new SmartHome.Common.API.Entities.Entities.Property
		{
			Name = "cameraId",
			Value = imageMetadata.CameraId.ToString("N")
		});
		list.Add(new SmartHome.Common.API.Entities.Entities.Property
		{
			Name = "cameraName",
			Value = imageMetadata.CameraName
		});
		list.Add(new SmartHome.Common.API.Entities.Entities.Property
		{
			Name = "description",
			Value = imageMetadata.Description
		});
		return list;
	}

	private string GenerateImageUrl(string filePath)
	{
		return $"/capture/{Path.GetFileNameWithoutExtension(filePath)}.jpeg";
	}

	public List<Image> GetAllImages(string cameraId)
	{
		return (from i in (from i in GetAllImagesMetadata()
				where i.CameraId.ToString("N") == cameraId
				select i).Select(ToImage)
			orderby i.Timestamp descending
			select i).ToList();
	}

	public void DeleteImage(string id)
	{
		string imageMetadataName = string.Format("{0}{1}", "imgfn", id);
		ConfigurationItem configurationItem = appSettings.GetAllSettings().FirstOrDefault((ConfigurationItem c) => c.Name == imageMetadataName);
		if (configurationItem != null)
		{
			ActionRequest actionRequest = ShcRequestHelper.NewRequest<ActionRequest>();
			string text = configurationItem.ApplicationId.Remove(0, 5);
			actionRequest.ActionDescription = new ActionDescription
			{
				ActionType = "DeleteImage",
				Namespace = text,
				Target = new LinkBinding
				{
					LinkType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Product,
					EntityId = text
				},
				Data = new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter>
				{
					new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter
					{
						Name = "image",
						Value = new ConstantStringBinding
						{
							Value = configurationItem.Name.Remove(0, "imgfn".Length)
						}
					}
				}
			};
			BaseResponse response = shcClient.GetResponse(actionRequest);
			if (response is ErrorResponse)
			{
				throw new ApiException(ErrorHelper.GetError((ErrorResponse)response));
			}
		}
	}

	public string ResolveImageUrl(string fileName)
	{
		string imageMetadataName = string.Format("{0}{1}", "imgfn", Path.GetFileNameWithoutExtension(fileName));
		ConfigurationItem configurationItem = appSettings.GetAllSettings().FirstOrDefault((ConfigurationItem c) => c.Name == imageMetadataName);
		if (configurationItem != null)
		{
			ImageMetaData imageMetaData = configurationItem.Value.FromJson<ImageMetaData>();
			return imageMetaData.FileName;
		}
		return fileName;
	}

	public void DeleteAllImages()
	{
		List<Image> allImages = GetAllImages();
		foreach (Image item in allImages)
		{
			DeleteImage(item.Id);
		}
	}

	private bool IsUsbConnected()
	{
		if (Directory.Exists("Hard Disk\\data"))
		{
			return true;
		}
		return false;
	}
}
