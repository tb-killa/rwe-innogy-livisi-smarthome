using System.ComponentModel;

namespace Microsoft.Practices.Mobile.ContainerModel;

[EditorBrowsable(EditorBrowsableState.Never)]
public interface IReused : IFluentInterface
{
	IOwned ReusedWithin(ReuseScope scope);
}
