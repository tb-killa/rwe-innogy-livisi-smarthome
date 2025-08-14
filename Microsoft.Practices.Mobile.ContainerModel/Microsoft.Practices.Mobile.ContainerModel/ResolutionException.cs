using System;
using System.Globalization;
using Microsoft.Practices.Mobile.ContainerModel.Properties;

namespace Microsoft.Practices.Mobile.ContainerModel;

[Serializable]
public class ResolutionException : Exception
{
	public ResolutionException(Type missingServiceType)
		: base(string.Format(CultureInfo.CurrentCulture, Resources.ResolutionException_MissingType, new object[1] { missingServiceType.FullName }))
	{
	}

	public ResolutionException(Type missingServiceType, string missingServiceName)
		: base(string.Format(CultureInfo.CurrentCulture, Resources.ResolutionException_MissingNamedType, new object[2] { missingServiceType.FullName, missingServiceName }))
	{
	}

	public ResolutionException(string message)
		: base(message)
	{
	}
}
