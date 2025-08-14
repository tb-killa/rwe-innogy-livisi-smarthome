using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Mac;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[XmlRoot(Namespace = "urn:macxsd", IsNullable = false)]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "urn:macxsd")]
public class mac
{
	private mac_option_ack_requestType mac_option_ack_requestField;

	private mac_option_ackType mac_option_ackField;

	private mac_option_fragmentType mac_option_fragmentField;

	private mac_option_channel_mapType[] mac_option_channel_mapField;

	private mac_option_wake_on_radioType mac_option_wake_on_radioField;

	private uint mac_layer_versionField;

	private byte[] frame_nonceField;

	private byte[] mac_source_addressField;

	private byte[] frame_integrity_codeField;

	private byte[] mac_destination_adressField;

	private uint content_typeField;

	public mac_option_ack_requestType mac_option_ack_request
	{
		get
		{
			return mac_option_ack_requestField;
		}
		set
		{
			mac_option_ack_requestField = value;
		}
	}

	public mac_option_ackType mac_option_ack
	{
		get
		{
			return mac_option_ackField;
		}
		set
		{
			mac_option_ackField = value;
		}
	}

	public mac_option_fragmentType mac_option_fragment
	{
		get
		{
			return mac_option_fragmentField;
		}
		set
		{
			mac_option_fragmentField = value;
		}
	}

	[XmlElement("mac_option_channel_map")]
	public mac_option_channel_mapType[] mac_option_channel_map
	{
		get
		{
			return mac_option_channel_mapField;
		}
		set
		{
			mac_option_channel_mapField = value;
		}
	}

	public mac_option_wake_on_radioType mac_option_wake_on_radio
	{
		get
		{
			return mac_option_wake_on_radioField;
		}
		set
		{
			mac_option_wake_on_radioField = value;
		}
	}

	[XmlAttribute]
	public uint mac_layer_version
	{
		get
		{
			return mac_layer_versionField;
		}
		set
		{
			mac_layer_versionField = value;
		}
	}

	[XmlAttribute(DataType = "hexBinary")]
	public byte[] frame_nonce
	{
		get
		{
			return frame_nonceField;
		}
		set
		{
			frame_nonceField = value;
		}
	}

	[XmlAttribute(DataType = "hexBinary")]
	public byte[] mac_source_address
	{
		get
		{
			return mac_source_addressField;
		}
		set
		{
			mac_source_addressField = value;
		}
	}

	[XmlAttribute(DataType = "hexBinary")]
	public byte[] frame_integrity_code
	{
		get
		{
			return frame_integrity_codeField;
		}
		set
		{
			frame_integrity_codeField = value;
		}
	}

	[XmlAttribute(DataType = "hexBinary")]
	public byte[] mac_destination_adress
	{
		get
		{
			return mac_destination_adressField;
		}
		set
		{
			mac_destination_adressField = value;
		}
	}

	[XmlAttribute]
	public uint content_type
	{
		get
		{
			return content_typeField;
		}
		set
		{
			content_typeField = value;
		}
	}
}
