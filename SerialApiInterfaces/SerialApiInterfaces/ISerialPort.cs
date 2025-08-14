using System;
using System.Collections.Generic;

namespace SerialApiInterfaces;

public interface ISerialPort : IDisposable
{
	string[] SerialPorts { get; }

	event ReceiveSerialData ReceiveData;

	bool Open(string port);

	bool Open(string port, Baudrate baudrate);

	void Close();

	int Send(List<byte> data);
}
