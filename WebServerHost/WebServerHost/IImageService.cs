using System.Collections.Generic;
using SmartHome.Common.API.Entities.Data;

namespace WebServerHost;

public interface IImageService
{
	List<Image> GetAllImages();

	List<Image> GetAllImages(string cameraId);

	void DeleteImage(string id);

	string ResolveImageUrl(string fileName);

	void DeleteAllImages();
}
