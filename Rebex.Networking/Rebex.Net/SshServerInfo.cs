using onrkn;

namespace Rebex.Net;

public class SshServerInfo
{
	private string[] vwwex;

	private string[] pytnb;

	private string[] bmbbf;

	private string[] pijef;

	private string[] uslrl;

	private string[] tuqhc;

	private string[] ffwkm;

	private string[] bmwpf;

	public string[] CompressionAlgorithmsClientToServer
	{
		get
		{
			return vwwex;
		}
		private set
		{
			vwwex = value;
		}
	}

	public string[] CompressionAlgorithmsServerToClient
	{
		get
		{
			return pytnb;
		}
		private set
		{
			pytnb = value;
		}
	}

	public string[] EncryptionAlgorithmsClientToServer
	{
		get
		{
			return bmbbf;
		}
		private set
		{
			bmbbf = value;
		}
	}

	public string[] EncryptionAlgorithmsServerToClient
	{
		get
		{
			return pijef;
		}
		private set
		{
			pijef = value;
		}
	}

	public string[] KeyExchangeAlgorithms
	{
		get
		{
			return uslrl;
		}
		private set
		{
			uslrl = value;
		}
	}

	public string[] MacAlgorithmsClientToServer
	{
		get
		{
			return tuqhc;
		}
		private set
		{
			tuqhc = value;
		}
	}

	public string[] MacAlgorithmsServerToClient
	{
		get
		{
			return ffwkm;
		}
		private set
		{
			ffwkm = value;
		}
	}

	public string[] ServerHostKeyAlgorithms
	{
		get
		{
			return bmwpf;
		}
		private set
		{
			bmwpf = value;
		}
	}

	internal SshServerInfo(string[] kexAlgorithms, string[] serverHostKeyAlgorithms, string[] encryptionClientToServer, string[] encryptionServerToClient, string[] macClientToServer, string[] macServerToClient, string[] compressionClientToServer, string[] compressionServerToClient)
	{
		KeyExchangeAlgorithms = kexAlgorithms;
		ServerHostKeyAlgorithms = serverHostKeyAlgorithms;
		EncryptionAlgorithmsClientToServer = encryptionClientToServer;
		EncryptionAlgorithmsServerToClient = encryptionServerToClient;
		MacAlgorithmsClientToServer = macClientToServer;
		MacAlgorithmsServerToClient = macServerToClient;
		CompressionAlgorithmsClientToServer = compressionClientToServer;
		CompressionAlgorithmsServerToClient = compressionServerToClient;
	}

	internal SshServerInfo(ckzrf tki)
	{
		CompressionAlgorithmsClientToServer = (string[])tki.cwmdd.Clone();
		CompressionAlgorithmsServerToClient = (string[])tki.jsjqy.Clone();
		EncryptionAlgorithmsClientToServer = (string[])tki.wdccr.Clone();
		EncryptionAlgorithmsServerToClient = (string[])tki.yunxz.Clone();
		KeyExchangeAlgorithms = (string[])tki.fjrpn.Clone();
		MacAlgorithmsClientToServer = (string[])tki.hfzts.Clone();
		MacAlgorithmsServerToClient = (string[])tki.ipvna.Clone();
		ServerHostKeyAlgorithms = (string[])tki.kwdyx.Clone();
	}
}
