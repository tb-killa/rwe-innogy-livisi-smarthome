using System.Collections.Generic;
using SmartHome.Common.API.Entities.Entities;

namespace WebServerHost;

public interface IProductsService
{
	List<Product> GetAvailableProducts();

	Product GetProduct(string type);

	string GetHash();

	void InstallProduct(string type);

	void ActivateProduct(string type);

	void DeactivateProduct(string type);

	void UninstallProduct(string type);
}
