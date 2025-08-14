namespace RWE.SmartHome.SHC.ApplicationsHost;

internal class MainAssemblyData
{
	public string FileName { get; private set; }

	public string[] TypesToLoad { get; private set; }

	public MainAssemblyData(string fileName, string[] typesToLoad)
	{
		FileName = fileName;
		TypesToLoad = typesToLoad;
	}
}
