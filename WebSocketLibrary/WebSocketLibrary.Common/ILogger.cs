namespace WebSocketLibrary.Common;

public interface ILogger
{
	void Info(string message, params object[] @params);

	void Error(string message, params object[] @params);
}
