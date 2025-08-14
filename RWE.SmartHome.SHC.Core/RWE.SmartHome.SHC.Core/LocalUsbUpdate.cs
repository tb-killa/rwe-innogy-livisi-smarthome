using System;
using System.IO;
using Ionic.Zip;

namespace RWE.SmartHome.SHC.Core;

public static class LocalUsbUpdate
{
	public static void HandleLocalCommUpdate()
	{
		HandleUIUpdate();
		HandleAddinsUpdate();
	}

	private static void HandleUIUpdate()
	{
		string text = "\\Hard Disk\\ui_update.zip";
		string text2 = "\\NandFlash\\Temp";
		string text3 = "\\NandFlash\\Temp\\WWWRoot";
		string destination = "\\NandFlash\\SHC\\WWWRoot";
		Console.WriteLine("UI Update:");
		try
		{
			if (File.Exists(text))
			{
				Console.WriteLine("Found UI update file on USB");
				CleanupTemp(text2);
				Directory.CreateDirectory(text2);
				ExtractArchive(text, text3);
				ReplaceDirectory(text3, destination);
				CleanupTemp(text2);
			}
			else
			{
				Console.WriteLine("No UI update found.");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("UI app update failed with error: {0}", ex.Message);
		}
	}

	private static void ExtractArchive(string zipFile, string baseDirectory)
	{
		using ZipFile zipFile2 = new ZipFile(zipFile);
		foreach (ZipEntry entry in zipFile2.Entries)
		{
			Console.WriteLine("- unzipping {0}", entry);
			entry.Extract(baseDirectory);
		}
	}

	private static void ReplaceDirectory(string directory, string destination)
	{
		if (Directory.Exists(destination))
		{
			Directory.Delete(destination, recursive: true);
		}
		Directory.Move(directory, destination);
	}

	private static void CleanupTemp(string uiArchiveTempPath)
	{
		if (Directory.Exists(uiArchiveTempPath))
		{
			Directory.Delete(uiArchiveTempPath, recursive: true);
		}
	}

	private static void HandleAddinsUpdate()
	{
		string path = "addin_update.zip";
		string text = Path.Combine("\\Hard Disk", path);
		string text2 = "\\NandFlash\\Temp";
		string text3 = "\\NandFlash\\Temp\\addins";
		string destination = "\\NandFlash\\SHC\\addins";
		Console.WriteLine("Addin Update:");
		try
		{
			if (File.Exists(text))
			{
				Console.WriteLine("Found addin update on USB");
				CleanupTemp(text2);
				Directory.CreateDirectory(text2);
				ExtractArchive(text, text3);
				ReplaceDirectory(text3, destination);
				CleanupTemp(text3);
			}
			else
			{
				Console.WriteLine("No addin update found");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("addin repository update failed with error: {0}", ex.Message);
		}
	}
}
