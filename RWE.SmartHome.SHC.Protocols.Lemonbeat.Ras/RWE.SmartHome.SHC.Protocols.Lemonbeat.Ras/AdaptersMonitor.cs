using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal class AdaptersMonitor
{
	public enum Family : uint
	{
		AF_INET = 2u,
		AF_INET6 = 23u,
		AF_UNSPEC = 0u
	}

	public enum Flags : uint
	{
		NONE = 0u,
		GAA_FLAG_SKIP_UNICAST = 1u,
		GAA_FLAG_SKIP_ANYCAST = 2u,
		GAA_FLAG_SKIP_MULTICAST = 4u,
		GAA_FLAG_SKIP_DNS_SERVER = 8u,
		GAA_FLAG_INCLUDE_PREFIX = 0x10u,
		GAA_FLAG_SKIP_FRIENDLY_NAME = 0x20u
	}

	public enum Error : uint
	{
		ERROR_SUCCESS = 0u,
		ERROR_NO_DATA = 232u,
		ERROR_BUFFER_OVERFLOW = 111u,
		ERROR_INVALID_PARAMETER = 87u
	}

	public enum If_Oper_Status : uint
	{
		IfOperStatusUp = 1u,
		IfOperStatusDown,
		IfOperStatusTesting,
		IfOperStatusUnknown,
		IfOperStatusDormant,
		IfOperStatusNotPresent,
		IfOperStatusLowerLayerDown
	}

	public enum If_Type : uint
	{
		IF_TYPE_OTHER = 1u,
		IF_TYPE_REGULAR_1822,
		IF_TYPE_HDH_1822,
		IF_TYPE_DDN_X25,
		IF_TYPE_RFC877_X25,
		IF_TYPE_ETHERNET_CSMACD,
		IF_TYPE_IS088023_CSMACD,
		IF_TYPE_ISO88024_TOKENBUS,
		IF_TYPE_ISO88025_TOKENRING,
		IF_TYPE_ISO88026_MAN,
		IF_TYPE_STARLAN,
		IF_TYPE_PROTEON_10MBIT,
		IF_TYPE_PROTEON_80MBIT,
		IF_TYPE_HYPERCHANNEL,
		IF_TYPE_FDDI,
		IF_TYPE_LAP_B,
		IF_TYPE_SDLC,
		IF_TYPE_DS1,
		IF_TYPE_E1,
		IF_TYPE_BASIC_ISDN,
		IF_TYPE_PRIMARY_ISDN,
		IF_TYPE_PROP_POINT2POINT_SERIAL,
		IF_TYPE_PPP,
		IF_TYPE_SOFTWARE_LOOPBACK,
		IF_TYPE_EON,
		IF_TYPE_ETHERNET_3MBIT,
		IF_TYPE_NSIP,
		IF_TYPE_SLIP,
		IF_TYPE_ULTRA,
		IF_TYPE_DS3,
		IF_TYPE_SIP,
		IF_TYPE_FRAMERELAY,
		IF_TYPE_RS232,
		IF_TYPE_PARA,
		IF_TYPE_ARCNET,
		IF_TYPE_ARCNET_PLUS,
		IF_TYPE_ATM,
		IF_TYPE_MIO_X25,
		IF_TYPE_SONET,
		IF_TYPE_X25_PLE,
		IF_TYPE_ISO88022_LLC,
		IF_TYPE_LOCALTALK,
		IF_TYPE_SMDS_DXI,
		IF_TYPE_FRAMERELAY_SERVICE,
		IF_TYPE_V35,
		IF_TYPE_HSSI,
		IF_TYPE_HIPPI,
		IF_TYPE_MODEM,
		IF_TYPE_AAL5,
		IF_TYPE_SONET_PATH,
		IF_TYPE_SONET_VT,
		IF_TYPE_SMDS_ICIP,
		IF_TYPE_PROP_VIRTUAL,
		IF_TYPE_PROP_MULTIPLEXOR,
		IF_TYPE_IEEE80212,
		IF_TYPE_FIBRECHANNEL,
		IF_TYPE_HIPPIINTERFACE,
		IF_TYPE_FRAMERELAY_INTERCONNECT,
		IF_TYPE_AFLANE_8023,
		IF_TYPE_AFLANE_8025,
		IF_TYPE_CCTEMUL,
		IF_TYPE_FASTETHER,
		IF_TYPE_ISDN,
		IF_TYPE_V11,
		IF_TYPE_V36,
		IF_TYPE_G703_64K,
		IF_TYPE_G703_2MB,
		IF_TYPE_QLLC,
		IF_TYPE_FASTETHER_FX,
		IF_TYPE_CHANNEL,
		IF_TYPE_IEEE80211,
		IF_TYPE_IBM370PARCHAN,
		IF_TYPE_ESCON,
		IF_TYPE_DLSW,
		IF_TYPE_ISDN_S,
		IF_TYPE_ISDN_U,
		IF_TYPE_LAP_D,
		IF_TYPE_IPSWITCH,
		IF_TYPE_RSRB,
		IF_TYPE_ATM_LOGICAL,
		IF_TYPE_DS0,
		IF_TYPE_DS0_BUNDLE,
		IF_TYPE_BSC,
		IF_TYPE_ASYNC,
		IF_TYPE_CNR,
		IF_TYPE_ISO88025R_DTR,
		IF_TYPE_EPLRS,
		IF_TYPE_ARAP,
		IF_TYPE_PROP_CNLS,
		IF_TYPE_HOSTPAD,
		IF_TYPE_TERMPAD,
		IF_TYPE_FRAMERELAY_MPI,
		IF_TYPE_X213,
		IF_TYPE_ADSL,
		IF_TYPE_RADSL,
		IF_TYPE_SDSL,
		IF_TYPE_VDSL,
		IF_TYPE_ISO88025_CRFPRINT,
		IF_TYPE_MYRINET,
		IF_TYPE_VOICE_EM,
		IF_TYPE_VOICE_FXO,
		IF_TYPE_VOICE_FXS,
		IF_TYPE_VOICE_ENCAP,
		IF_TYPE_VOICE_OVERIP,
		IF_TYPE_ATM_DXI,
		IF_TYPE_ATM_FUNI,
		IF_TYPE_ATM_IMA,
		IF_TYPE_PPPMULTILINKBUNDLE,
		IF_TYPE_IPOVER_CDLC,
		IF_TYPE_IPOVER_CLAW,
		IF_TYPE_STACKTOSTACK,
		IF_TYPE_VIRTUALIPADDRESS,
		IF_TYPE_MPC,
		IF_TYPE_IPOVER_ATM,
		IF_TYPE_ISO88025_FIBER,
		IF_TYPE_TDLC,
		IF_TYPE_GIGABITETHERNET,
		IF_TYPE_HDLC,
		IF_TYPE_LAP_F,
		IF_TYPE_V37,
		IF_TYPE_X25_MLP,
		IF_TYPE_X25_HUNTGROUP,
		IF_TYPE_TRANSPHDLC,
		IF_TYPE_INTERLEAVE,
		IF_TYPE_FAST,
		IF_TYPE_IP,
		IF_TYPE_DOCSCABLE_MACLAYER,
		IF_TYPE_DOCSCABLE_DOWNSTREAM,
		IF_TYPE_DOCSCABLE_UPSTREAM,
		IF_TYPE_A12MPPSWITCH,
		IF_TYPE_TUNNEL,
		IF_TYPE_COFFEE,
		IF_TYPE_CES,
		IF_TYPE_ATM_SUBINTERFACE,
		IF_TYPE_L2_VLAN,
		IF_TYPE_L3_IPVLAN,
		IF_TYPE_L3_IPXVLAN,
		IF_TYPE_DIGITALPOWERLINE,
		IF_TYPE_MEDIAMAILOVERIP,
		IF_TYPE_DTM,
		IF_TYPE_DCN,
		IF_TYPE_IPFORWARD,
		IF_TYPE_MSDSL,
		IF_TYPE_IEEE1394,
		IF_TYPE_RECEIVE_ONLY
	}

	public struct IP_ADAPTER_ADDRESSES
	{
		public ulong Alignment;

		public IntPtr Next;

		public IntPtr AdapterName;

		public IntPtr FirstUnicastAddress;

		public IntPtr FirstAnycastAddress;

		public IntPtr FirstMulticastAddress;

		public IntPtr FirstDnsServerAddress;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string DnsSuffix;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string FriendlyName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] PhysicalAddress;

		public uint PhysicalAddressLength;

		public uint Flags;

		public uint Mtu;

		public If_Type IfType;

		public If_Oper_Status OperStatus;

		public uint Ipv6IfIndex;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public uint[] ZoneIndices;

		public IntPtr FirstPrefix;
	}

	private const int MAX_ADAPTER_ADDRESS_LENGTH = 8;

	private const int MAX_ADAPTER_NAME_LENGTH = 256;

	private List<uint> interfaces = new List<uint>();

	private uint? lastIndex;

	[DllImport("iphlpapi.dll")]
	private static extern Error GetAdaptersAddresses(Family Family, Flags Flags, IntPtr Reserved, IntPtr pAdapterAddresses, ref uint pOutBufLen);

	public static string MarshalString(IntPtr text)
	{
		byte[] array = new byte[256];
		Marshal.Copy(text, array, 0, 256);
		return Encoding.ASCII.GetString(array, 0, 256);
	}

	public void TakeSnapshot()
	{
		interfaces.Clear();
		uint pOutBufLen = 0u;
		Error adaptersAddresses = GetAdaptersAddresses(Family.AF_INET6, Flags.NONE, IntPtr.Zero, IntPtr.Zero, ref pOutBufLen);
		if (Error.ERROR_BUFFER_OVERFLOW != adaptersAddresses)
		{
			return;
		}
		IntPtr intPtr = Marshal.AllocHGlobal((int)pOutBufLen);
		try
		{
			if (GetAdaptersAddresses(Family.AF_INET6, Flags.NONE, IntPtr.Zero, intPtr, ref pOutBufLen) == Error.ERROR_SUCCESS)
			{
				IntPtr intPtr2 = intPtr;
				while (IntPtr.Zero != intPtr2)
				{
					IP_ADAPTER_ADDRESSES iP_ADAPTER_ADDRESSES = (IP_ADAPTER_ADDRESSES)Marshal.PtrToStructure(intPtr2, typeof(IP_ADAPTER_ADDRESSES));
					interfaces.Add(iP_ADAPTER_ADDRESSES.Ipv6IfIndex);
					intPtr2 = iP_ADAPTER_ADDRESSES.Next;
				}
			}
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public uint? GetNewlyCreatedInterface()
	{
		uint pOutBufLen = 0u;
		Error adaptersAddresses = GetAdaptersAddresses(Family.AF_INET6, Flags.NONE, IntPtr.Zero, IntPtr.Zero, ref pOutBufLen);
		if (Error.ERROR_BUFFER_OVERFLOW == adaptersAddresses)
		{
			IntPtr intPtr = Marshal.AllocHGlobal((int)pOutBufLen);
			try
			{
				if (GetAdaptersAddresses(Family.AF_INET6, Flags.NONE, IntPtr.Zero, intPtr, ref pOutBufLen) == Error.ERROR_SUCCESS)
				{
					IntPtr intPtr2 = intPtr;
					while (IntPtr.Zero != intPtr2)
					{
						IP_ADAPTER_ADDRESSES iP_ADAPTER_ADDRESSES = (IP_ADAPTER_ADDRESSES)Marshal.PtrToStructure(intPtr2, typeof(IP_ADAPTER_ADDRESSES));
						if (!interfaces.Contains(iP_ADAPTER_ADDRESSES.Ipv6IfIndex))
						{
							lastIndex = iP_ADAPTER_ADDRESSES.Ipv6IfIndex;
							break;
						}
						intPtr2 = iP_ADAPTER_ADDRESSES.Next;
					}
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
		return lastIndex;
	}

	public List<KeyValuePair<string, uint>> GetAllIPv6Interfaces()
	{
		List<KeyValuePair<string, uint>> list = new List<KeyValuePair<string, uint>>();
		uint pOutBufLen = 0u;
		Error adaptersAddresses = GetAdaptersAddresses(Family.AF_INET6, Flags.NONE, IntPtr.Zero, IntPtr.Zero, ref pOutBufLen);
		if (Error.ERROR_BUFFER_OVERFLOW == adaptersAddresses)
		{
			IntPtr intPtr = Marshal.AllocHGlobal((int)pOutBufLen);
			try
			{
				if (GetAdaptersAddresses(Family.AF_INET6, Flags.NONE, IntPtr.Zero, intPtr, ref pOutBufLen) == Error.ERROR_SUCCESS)
				{
					IntPtr intPtr2 = intPtr;
					while (IntPtr.Zero != intPtr2)
					{
						IP_ADAPTER_ADDRESSES iP_ADAPTER_ADDRESSES = (IP_ADAPTER_ADDRESSES)Marshal.PtrToStructure(intPtr2, typeof(IP_ADAPTER_ADDRESSES));
						list.Add(new KeyValuePair<string, uint>(iP_ADAPTER_ADDRESSES.Description, iP_ADAPTER_ADDRESSES.Ipv6IfIndex));
						intPtr2 = iP_ADAPTER_ADDRESSES.Next;
					}
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
		return list;
	}
}
