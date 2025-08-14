using System;
using System.Net;

namespace RWE.SmartHome.SHC.Core;

public static class Application
{
	private static void Main(string[] args)
	{
		try
		{
			ServicePointManager.DefaultConnectionLimit = 10;
			Console.Write("Received {0} parameters", args.Length);
			for (int i = 0; i < args.Length; i++)
			{
				Console.Write(" [" + args[i] + "]");
			}
			Console.WriteLine("");
			Console.WriteLine("SHC application started: {0} build at {1}", "Release", DateTime.Now.ToString());
			ApplicationRoot.Instance.Run();
		}
		catch (Exception ex)
		{
			Console.WriteLine("SHC application failed to start: {0}", ex.ToString());
		}
	}
}
