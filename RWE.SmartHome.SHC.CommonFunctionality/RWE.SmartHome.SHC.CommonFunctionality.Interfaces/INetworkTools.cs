using System;
using System.Net;

namespace RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

internal interface INetworkTools
{
	NetworkProblem DiagnoseNetworkProblem(string host, Exception networkException);

	string GetDeviceIp();

	string GetHostName();

	void SetHostName(string newHostName);

	string GetIpAddress(string hostName);

	string GetDeviceMacAddress();

	NetworkSpecifics GetNetworkSpecifics();

	bool ValidateIPAddress(IPAddress addrToValdiate);
}
