using System.ComponentModel;

namespace Microsoft.Practices.Mobile.ContainerModel;

[EditorBrowsable(EditorBrowsableState.Never)]
public interface IReusedOwned : IReused, IOwned, IFluentInterface
{
}
