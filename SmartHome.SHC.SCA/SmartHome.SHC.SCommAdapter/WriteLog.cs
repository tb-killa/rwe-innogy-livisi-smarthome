using System;
using Rebex;

namespace SmartHome.SHC.SCommAdapter;

public delegate void WriteLog(LogLevel level, Type objectType, int objectId, string area, string message, byte[] buffer, int offset, int length);
