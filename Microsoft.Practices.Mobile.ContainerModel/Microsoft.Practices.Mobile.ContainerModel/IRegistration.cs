using System.ComponentModel;

namespace Microsoft.Practices.Mobile.ContainerModel;

[EditorBrowsable(EditorBrowsableState.Never)]
public interface IRegistration : IReusedOwned, IReused, IOwned, IFluentInterface
{
}
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IRegistration<TService> : IRegistration, IReusedOwned, IReused, IOwned, IInitializable<TService>, IFluentInterface
{
}
