namespace Rebex;

public interface ILogWriterProvider
{
	ILogWriter LogWriter { get; }
}
