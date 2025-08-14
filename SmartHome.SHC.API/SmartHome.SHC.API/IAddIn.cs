namespace SmartHome.SHC.API;

public interface IAddIn
{
	string ApplicationId { get; }

	void Initialize(AddInConfiguration addInConfiguration, ICoreServices environment);

	void Uninitialize(CleanupMode mode);

	void Activate(AddInConfiguration addInConfiguration);

	void Deactivate();

	void Refresh(AddInConfiguration addInConfiguration);
}
