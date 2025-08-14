using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;

[XmlInclude(typeof(ValueReportActionItem))]
[XmlInclude(typeof(SetHexBinaryActionItem))]
[XmlInclude(typeof(SetCalculationActionItem))]
[XmlInclude(typeof(TimerActionItem))]
[XmlInclude(typeof(SetStringActionItem))]
[XmlInclude(typeof(SetNumberActionItem))]
public abstract class ActionItem : IEquatable<ActionItem>
{
	public abstract bool Equals(ActionItem other);

	public abstract override int GetHashCode();

	public override bool Equals(object obj)
	{
		return Equals(obj as ActionItem);
	}
}
