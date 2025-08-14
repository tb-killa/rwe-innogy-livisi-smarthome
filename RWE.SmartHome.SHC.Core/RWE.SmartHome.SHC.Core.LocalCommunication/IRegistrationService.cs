namespace RWE.SmartHome.SHC.Core.LocalCommunication;

public interface IRegistrationService
{
	bool TearmsAndConditionAccepted { get; }

	bool RemotelyRegistered { get; }

	bool IsShcLocalOnly { get; }

	ShcConfiguration GetShcConfiguration();

	void Register(ShcInitialization shcInitialization);

	void ResetTaC();

	void ResetIsShcLocalOnlyFlag();

	void PublishShcStartupEvents();

	void SetIsShcLocalOnlyFlagTrue();
}
