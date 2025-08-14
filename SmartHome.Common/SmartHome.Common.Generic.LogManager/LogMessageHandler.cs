using System;

namespace SmartHome.Common.Generic.LogManager;

public delegate void LogMessageHandler(Type logType, LogLevel logLevel, string logMessage);
