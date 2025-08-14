using System;
using System.Text;
using System.Threading;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;

namespace RWE.SmartHome.SHC.BusinessLogic;

public class ChunkWriter
{
	public delegate BackendPersistenceResult WriteChunk(byte[] chunk, int currentPackage, int nextPackage);

	private int bufferSize;

	private int bufferPosition;

	private byte[] buffer;

	private WriteChunk write;

	private int chunkIndex;

	private readonly ManualResetEvent cancellationEvent;

	public ChunkWriter(int chunkSize, WriteChunk writeMethod)
		: this(chunkSize, writeMethod, null)
	{
	}

	public ChunkWriter(int chunkSize, WriteChunk writeMethod, ManualResetEvent cancellationEvent)
	{
		chunkIndex = 1;
		bufferPosition = 0;
		bufferSize = chunkSize;
		buffer = new byte[bufferSize];
		write = writeMethod;
		this.cancellationEvent = cancellationEvent;
	}

	public BackendPersistenceResult AddUtf8String(string content)
	{
		return AddByteArray(Encoding.UTF8.GetBytes(content));
	}

	public BackendPersistenceResult AddByteArray(byte[] content)
	{
		int i = 0;
		BackendPersistenceResult result = BackendPersistenceResult.Success;
		int num;
		for (; content.Length - i > bufferSize - bufferPosition; i += num)
		{
			if (CancelRequested())
			{
				return BackendPersistenceResult.OperationCancelled;
			}
			num = bufferSize - bufferPosition;
			Array.Copy(content, i, buffer, bufferPosition, num);
			try
			{
				result = write(buffer, chunkIndex, chunkIndex + 1);
			}
			catch
			{
				return BackendPersistenceResult.ServiceAccessError;
			}
			chunkIndex++;
			bufferPosition = 0;
		}
		int num2 = content.Length - i;
		Array.Copy(content, i, buffer, bufferPosition, num2);
		bufferPosition += num2;
		return result;
	}

	public BackendPersistenceResult Flush()
	{
		if (CancelRequested())
		{
			return BackendPersistenceResult.OperationCancelled;
		}
		byte[] array = new byte[bufferPosition];
		Array.Copy(buffer, array, bufferPosition);
		BackendPersistenceResult backendPersistenceResult = BackendPersistenceResult.Success;
		try
		{
			backendPersistenceResult = write(array, chunkIndex, -1);
		}
		catch
		{
			return BackendPersistenceResult.ServiceAccessError;
		}
		chunkIndex = 1;
		bufferPosition = 0;
		return backendPersistenceResult;
	}

	private bool CancelRequested()
	{
		if (cancellationEvent != null)
		{
			return cancellationEvent.WaitOne(0, exitContext: false);
		}
		return false;
	}
}
