namespace RWE.SmartHome.SHC.ApplicationsHostInterfaces;

public class LocalAddin
{
	public string AppId { get; set; }

	public string Name { get; set; }

	public string Path { get; set; }

	public string ZipTemporaryPathFolder { get; set; }

	public string ManifestTemporatyFilePath { get; set; }

	public byte[] ManifestChecksum { get; set; }

	public string DestinationFolderManifestFileName { get; set; }
}
