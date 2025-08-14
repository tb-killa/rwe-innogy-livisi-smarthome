using System.IO;
using WebServerHost.Web;

namespace WebServerHost.Helpers;

internal class PluginsRepo
{
	private const string PluginsRepoFile = "\\plugins\\plugins.json";

	private const string PluginsHashFile = "\\plugins\\plugins.hash";

	public static string GetPuginsRepoFilePath(WebServerConfiguration configuration)
	{
		return configuration.ServerRoot + "\\plugins\\plugins.json";
	}

	public static string GetPluginsHash(WebServerConfiguration configuration)
	{
		using StreamReader streamReader = File.OpenText(configuration.ServerRoot + "\\plugins\\plugins.hash");
		return streamReader.ReadToEnd();
	}
}
