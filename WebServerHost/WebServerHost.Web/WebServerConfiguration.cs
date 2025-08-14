using System.Collections.Generic;
using System.IO;
using System.Net;
using WebServerHost.Web.Http;

namespace WebServerHost.Web;

public class WebServerConfiguration
{
	private int port = 80;

	private int applicationPort = 8080;

	private string serverRoot = string.Empty;

	private string serverName = "WebServerHost";

	private IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

	private string certificatePath = string.Empty;

	private string certificatePassword = string.Empty;

	private int keepAliveTimeout = 5;

	private int keepAliveMaxRequests = 5;

	private Dictionary<string, string> virtualDirectories = new Dictionary<string, string>();

	private List<string> defaultFiles = new List<string>();

	private List<string> specialFileTypes = new List<string>();

	public int Port
	{
		get
		{
			return port;
		}
		set
		{
			port = value;
		}
	}

	public int ApplicationPort
	{
		get
		{
			return applicationPort;
		}
		set
		{
			applicationPort = value;
		}
	}

	public IPAddress IPAddress
	{
		get
		{
			return ipAddress;
		}
		set
		{
			ipAddress = value;
		}
	}

	public string ServerRoot
	{
		get
		{
			return serverRoot;
		}
		set
		{
			serverRoot = value;
		}
	}

	public string ServerName
	{
		get
		{
			return serverName;
		}
		set
		{
			serverName = value;
		}
	}

	public string CertificatePath => certificatePath;

	public string CertifictePassword => certificatePassword;

	public int KeepAliveTimeout
	{
		get
		{
			return keepAliveTimeout;
		}
		set
		{
			keepAliveTimeout = value;
		}
	}

	public int KeepAliveMaxRequests
	{
		get
		{
			return keepAliveMaxRequests;
		}
		set
		{
			keepAliveMaxRequests = value;
		}
	}

	public void UseCertificate(string certificatePath, string certificatePassword)
	{
		this.certificatePath = certificatePath;
		this.certificatePassword = certificatePassword;
	}

	public void AddDefaultFile(string fileName)
	{
		if (!defaultFiles.Contains(fileName))
		{
			defaultFiles.Add(fileName);
		}
	}

	public string GetDefaultFileName(string localDirectory)
	{
		string result = string.Empty;
		foreach (string defaultFile in defaultFiles)
		{
			if (File.Exists(Path.Combine(localDirectory, defaultFile)))
			{
				result = defaultFile;
				break;
			}
		}
		return result;
	}

	public void AddMimeType(string fileExtension, string mimeType)
	{
		fileExtension = fileExtension.ToLower();
		if (!Common.MimeTypes.ContainsKey(fileExtension))
		{
			Common.MimeTypes.Add(fileExtension, mimeType);
		}
	}

	public string GetMimeType(string fileExtension)
	{
		fileExtension = fileExtension.ToLower();
		if (!Common.MimeTypes.ContainsKey(fileExtension))
		{
			return null;
		}
		return Common.MimeTypes[fileExtension];
	}

	public void AddVirtualDirectory(string name, string path)
	{
		if (!virtualDirectories.ContainsKey(name))
		{
			virtualDirectories.Add(name, path);
		}
	}

	public string GetVirtualDirectory(string directory)
	{
		if (!virtualDirectories.ContainsKey(directory))
		{
			return string.Empty;
		}
		return virtualDirectories[directory];
	}

	public void AddSpecialFileType(string fileExtension)
	{
		if (!specialFileTypes.Contains(fileExtension))
		{
			specialFileTypes.Add(fileExtension);
		}
	}

	public bool IsSpecialFileType(string fileName)
	{
		string item = Path.GetExtension(fileName).ToLower();
		return specialFileTypes.Contains(item);
	}
}
