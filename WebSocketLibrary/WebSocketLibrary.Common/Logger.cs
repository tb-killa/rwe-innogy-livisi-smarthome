using System;

namespace WebSocketLibrary.Common;

public class Logger : ILogger
{
	private readonly Action<string> informationCallback;

	private readonly Action<string> errorCallback;

	public Logger(Action<string> informationCallback, Action<string> errorCallback)
	{
		this.informationCallback = informationCallback;
		this.errorCallback = errorCallback;
	}

	public void Info(string message, params object[] @params)
	{
		try
		{
			informationCallback(string.Format(message, @params));
		}
		catch (Exception ex)
		{
			Console.WriteLine("Exception logger INFO: {0}", ex.Message);
		}
	}

	public void Error(string message, params object[] @params)
	{
		try
		{
			errorCallback(string.Format(message, @params));
		}
		catch (Exception ex)
		{
			Console.WriteLine("Exception logger INFO: {0}", ex.Message);
		}
	}
}
