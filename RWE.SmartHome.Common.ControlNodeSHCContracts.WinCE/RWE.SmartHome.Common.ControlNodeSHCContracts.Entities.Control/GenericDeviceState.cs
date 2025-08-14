using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;

public class GenericDeviceState : LogicalDeviceState, IEquatable<GenericDeviceState>
{
	[XmlArrayItem(ElementName = "Ppt")]
	[XmlArray(ElementName = "Ppts")]
	public List<Property> Properties { get; set; }

	public GenericDeviceState()
	{
		Properties = new List<Property>();
	}

	public override void UpdateFrom(LogicalDeviceState value, DateTime timestamp)
	{
		if (value is GenericDeviceState { Properties: not null } genericDeviceState)
		{
			Properties = genericDeviceState.Properties.Select((Property ap) => ap.Clone()).ToList();
		}
	}

	public override List<Property> GetProperties()
	{
		return Properties;
	}

	protected override LogicalDeviceState CreateClone()
	{
		return new GenericDeviceState();
	}

	protected override void TransferProperties(LogicalDeviceState clone)
	{
		base.TransferProperties(clone);
		GenericDeviceState genericDeviceState = (GenericDeviceState)clone;
		if (Properties != null)
		{
			genericDeviceState.Properties = Properties.Select((Property prop) => prop.Clone()).ToList();
		}
	}

	public bool Equals(GenericDeviceState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.LogicalDeviceId.Equals(base.LogicalDeviceId) && other.Properties.All((Property p) => Properties.Contains(p)) && Properties.All((Property p) => other.Properties.Contains(p));
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj))
		{
			return false;
		}
		if (object.ReferenceEquals(this, obj))
		{
			return true;
		}
		if ((object)obj.GetType() != typeof(GenericDeviceState))
		{
			return false;
		}
		return Equals((GenericDeviceState)obj);
	}

	public override int GetHashCode()
	{
		int result = 0;
		if (Properties != null)
		{
			Properties.ForEach(delegate(Property p)
			{
				result = (result * 397) ^ p.GetHashCode();
			});
		}
		return result;
	}

	public override string ToString()
	{
		try
		{
			if (Properties == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Property property in Properties)
			{
				if (property != null && property.Name != null)
				{
					stringBuilder.AppendFormat("{0} = {1},", new object[2]
					{
						property.Name,
						property.GetValueAsString() ?? "null"
					});
				}
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Insert(0, "[");
				stringBuilder.Replace(',', ']', stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}
		catch (Exception arg)
		{
			return $"<state could not be determined: {arg}>";
		}
	}
}
