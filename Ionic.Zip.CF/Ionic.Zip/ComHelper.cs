using System.Runtime.InteropServices;

namespace Ionic.Zip;

[ComVisible(true)]
[Guid("ebc25cf6-9120-4283-b972-0e5520d0000F")]
public class ComHelper
{
	public bool IsZipFile(string filename)
	{
		return ZipFile.IsZipFile(filename);
	}

	public bool IsZipFileWithExtract(string filename)
	{
		return ZipFile.IsZipFile(filename, testExtract: true);
	}

	public string GetZipLibraryVersion()
	{
		return ZipFile.LibraryVersion.ToString();
	}
}
