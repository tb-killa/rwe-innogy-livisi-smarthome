namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class ValueDescription : ConfigurationItem
{
	public string Name { get; set; }

	public uint Type { get; set; }

	public bool Readable { get; set; }

	public bool Writeable { get; set; }

	public bool Persistent { get; set; }

	public NumberFormat NumberFormat { get; set; }

	public StringFormat StringFormat { get; set; }

	public HexBinaryFormat HexBinaryFormat { get; set; }

	public uint? MinLogInterval { get; set; }

	public uint? MaxLogValues { get; set; }

	public bool IsVirtual { get; set; }

	public ValueDescription()
	{
	}

	public ValueDescription(byte id, byte type, string name, bool readable, bool writeable, bool persistent)
	{
		base.Id = id;
		Type = type;
		Name = name;
		Readable = readable;
		Writeable = writeable;
		Persistent = persistent;
	}

	public override bool Equals(ConfigurationItem other)
	{
		if (other is ValueDescription valueDescription && base.Id == valueDescription.Id && Name == valueDescription.Name && Type == valueDescription.Type && Readable == valueDescription.Readable && Writeable == valueDescription.Writeable && Persistent == valueDescription.Persistent && MinLogInterval == valueDescription.MinLogInterval)
		{
			uint? maxLogValues = MaxLogValues;
			uint? maxLogValues2 = valueDescription.MaxLogValues;
			if (maxLogValues.GetValueOrDefault() == maxLogValues2.GetValueOrDefault() && maxLogValues.HasValue == maxLogValues2.HasValue && IsVirtual == valueDescription.IsVirtual && ((NumberFormat == null && valueDescription.NumberFormat == null) || (NumberFormat != null && NumberFormat.Equals(valueDescription.NumberFormat))) && ((StringFormat == null && valueDescription.StringFormat == null) || (StringFormat != null && StringFormat.Equals(valueDescription.StringFormat))))
			{
				if (HexBinaryFormat != null || valueDescription.HexBinaryFormat != null)
				{
					if (HexBinaryFormat != null)
					{
						return HexBinaryFormat.Equals(valueDescription.HexBinaryFormat);
					}
					return false;
				}
				return true;
			}
		}
		return false;
	}
}
