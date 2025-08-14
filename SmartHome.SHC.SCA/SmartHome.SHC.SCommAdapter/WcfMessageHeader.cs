using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Xml;

namespace SmartHome.SHC.SCommAdapter;

public class WcfMessageHeader : MessageHeader
{
	private readonly string name;

	private readonly string @namespace;

	private readonly string value;

	public override string Name => name;

	public override string Namespace => @namespace;

	public string Value => value;

	public WcfMessageHeader(string name, string @namespace, string value)
	{
		this.name = name;
		this.@namespace = @namespace;
		this.value = value;
	}

	internal static IList<WcfMessageHeader> FromMessageHeaders(MessageHeaders headers)
	{
		return headers.OfType<WcfMessageHeader>().ToList();
	}

	protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
	{
		throw new NotImplementedException();
	}
}
