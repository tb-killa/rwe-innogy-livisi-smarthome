using System;

namespace SmartHome.Common.Generic.LogManager;

public sealed class LogManager
{
	private static LogManager instance;

	public static LogManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new LogManager();
			}
			return instance;
		}
	}

	public static event LogMessageHandler OnLogMessage;

	private LogManager()
	{
	}

	public ILogger GetLogger<T>()
	{
		return GetLogger(typeof(T));
	}

	public ILogger GetLogger(Type type)
	{
		Logger logger = new Logger(type);
		logger.LogMessageEvent += LogManager.OnLogMessage;
		return logger;
	}
}
