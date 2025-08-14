using System.Xml.Serialization;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.ValueConstrains;

[XmlInclude(typeof(ReadOnly))]
[XmlInclude(typeof(NumericRangeStep))]
[XmlInclude(typeof(StringEnumMatch))]
[XmlInclude(typeof(StringNotEmpty))]
[XmlInclude(typeof(NumericRange))]
[XmlInclude(typeof(Required))]
public abstract class ValueConstraint
{
	public abstract ValidationResult IsValid(object property, object repositoryProperty);
}
public abstract class ValueConstraint<T> : ValueConstraint where T : class
{
	public override ValidationResult IsValid(object property, object repositoryProperty)
	{
		return IsValid(property as T, repositoryProperty as T);
	}

	protected abstract ValidationResult IsValid(T property, T repositoryProperty);
}
