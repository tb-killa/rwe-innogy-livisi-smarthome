using System;
using System.ComponentModel;

namespace Microsoft.Practices.Mobile.ContainerModel;

[EditorBrowsable(EditorBrowsableState.Never)]
public interface IInitializable<TService> : IFluentInterface
{
	IReusedOwned InitializedBy(Action<Container, TService> initializer);
}
