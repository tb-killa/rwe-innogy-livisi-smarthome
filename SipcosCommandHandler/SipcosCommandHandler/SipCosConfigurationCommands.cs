namespace SipcosCommandHandler;

public enum SipCosConfigurationCommands : byte
{
	DeleteSelectedParameterRange = 0,
	CreateLink = 1,
	RemoveLink = 2,
	RequestList = 3,
	ConfigurationDataRequest = 4,
	StartConfiguration = 5,
	EndConfiguration = 6,
	ParameterOffset = 7,
	ParameterIndex = 8,
	RequestSGTIN = 9,
	ResponseLinkPartnerList = 10,
	ResponseConfigurationData = 11,
	ReportLinkPartnerProblem = 14,
	RequestConfigUpdate = 15,
	SetTestStatus = 128,
	GetTestStatus = 129,
	TestStatus = 130
}
