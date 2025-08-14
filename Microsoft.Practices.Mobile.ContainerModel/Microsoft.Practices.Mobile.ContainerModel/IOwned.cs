using System.ComponentModel;

namespace Microsoft.Practices.Mobile.ContainerModel;

[EditorBrowsable(EditorBrowsableState.Never)]
public interface IOwned : IFluentInterface
{
	void OwnedBy(Owner owner);
}
