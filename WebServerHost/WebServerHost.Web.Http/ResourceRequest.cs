using System;
using System.IO;
using System.Text.RegularExpressions;

namespace WebServerHost.Web.Http;

public class ResourceRequest
{
	private string directory;

	private string fileName;

	private string queryString;

	private WebServerConfiguration configuration;

	public string Directory
	{
		get
		{
			return directory;
		}
		set
		{
			directory = value;
		}
	}

	public string FileName
	{
		get
		{
			return fileName;
		}
		set
		{
			fileName = value;
		}
	}

	public string QueryString
	{
		get
		{
			return queryString;
		}
		set
		{
			queryString = value;
		}
	}

	public string FullPath => Path.Combine(Directory, FileName);

	public ResourceRequest(ShcWebRequest request, WebServerConfiguration configuration)
	{
		this.configuration = configuration;
		string text = Uri.UnescapeDataString(request.RequestUri);
		int num = text.LastIndexOf("/");
		FileInfo fileInfo = new FileInfo(text);
		queryString = string.Empty;
		directory = ((num == 0) ? "" : fileInfo.DirectoryName);
		fileName = fileInfo.Name;
		num = fileName.IndexOf('?');
		if (num > 0)
		{
			queryString = fileName.Substring(num + 1);
			fileName = fileName.Substring(0, num);
		}
		directory = GetLocalDir(directory);
		if (string.IsNullOrEmpty(fileName))
		{
			fileName = configuration.GetDefaultFileName(directory);
		}
	}

	private string GetLocalDir(string path)
	{
		path = path.Trim();
		Match match = Regex.Match(path, "^/?([^/]*)");
		string text = match.ToString();
		string text2 = path.Substring(match.Length);
		string virtualDirectory = configuration.GetVirtualDirectory(text);
		text2 = text2.Replace('/', Path.DirectorySeparatorChar);
		text = text.Replace('/', Path.DirectorySeparatorChar);
		string text3 = (string.IsNullOrEmpty(virtualDirectory) ? (configuration.ServerRoot + text + text2) : (virtualDirectory + text2));
		Console.WriteLine("Local dir: " + text3);
		return text3;
	}
}
