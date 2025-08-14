namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcSettings;

public enum ShcTimeZone
{
	[StringValue("Dateline Standard Time")]
	DatelineStandardTime = 0,
	[StringValue("Samoa Standard Time")]
	SamoaStandardTime = 1,
	[StringValue("Hawaiian Standard Time")]
	HawaiianStandardTime = 2,
	[StringValue("Alaskan Standard Time")]
	AlaskanStandardTime = 3,
	[StringValue("Pacific Standard Time")]
	PacificStandardTime = 4,
	[StringValue("Pacific Standard Time (Mexico)")]
	PacificStandardTimeMexico = 5,
	[StringValue("Mountain Standard Time")]
	MountainStandardTime = 6,
	[StringValue("US Mountain Standard Time")]
	USMountainStandardTime = 7,
	[StringValue("Mountain Standard Time (Mexico)")]
	MountainStandardTimeMexico = 8,
	[StringValue("Central Standard Time")]
	CentralStandardTime = 9,
	[StringValue("Canada Central Standard Time")]
	CanadaCentralStandardTime = 10,
	[StringValue("Central America Standard Time")]
	CentralAmericaStandardTime = 11,
	[StringValue("Central Standard Time (Mexico)")]
	CentralStandardTimeMexico = 12,
	[StringValue("Eastern Standard Time")]
	EasternStandardTime = 13,
	[StringValue("US Eastern Standard Time")]
	USEasternStandardTime = 14,
	[StringValue("SA Pacific Standard Time")]
	SAPacificStandardTime = 15,
	[StringValue("Venezuela Standard Time")]
	VenezuelaStandardTime = 16,
	[StringValue("Atlantic Standard Time")]
	AtlanticStandardTime = 17,
	[StringValue("SA Western Standard Time")]
	SAWesternStandardTime = 18,
	[StringValue("Pacific SA Standard Time")]
	PacificSAStandardTime = 19,
	[StringValue("Central Brazilian Standard Time")]
	CentralBrazilianStandardTime = 20,
	[StringValue("Paraguay Standard Time")]
	ParaguayStandardTime = 21,
	[StringValue("Newfoundland Standard Time")]
	NewfoundlandStandardTime = 22,
	[StringValue("E. South America Standard Time")]
	ESouthAmericaStandardTime = 23,
	[StringValue("SA Eastern Standard Time")]
	SAEasternStandardTime = 24,
	[StringValue("Greenland Standard Time")]
	GreenlandStandardTime = 25,
	[StringValue("Argentina Standard Time")]
	ArgentinaStandardTime = 26,
	[StringValue("Montevideo Standard Time")]
	MontevideoStandardTime = 27,
	[StringValue("Mid-Atlantic Standard Time")]
	MidAtlanticStandardTime = 28,
	[StringValue("Azores Standard Time")]
	AzoresStandardTime = 30,
	[StringValue("GMT Standard Time")]
	GMTStandardTime = 32,
	[StringValue("Greenwich Standard Time")]
	GreenwichStandardTime = 33,
	[StringValue("Morocco Standard Time")]
	MoroccoStandardTime = 34,
	[StringValue("UTC")]
	UTC = 35,
	[StringValue("Central Europe Standard Time")]
	CentralEuropeStandardTime = 36,
	[StringValue("Central European Standard Time")]
	CentralEuropeanStandardTime = 37,
	[StringValue("Romance Standard Time")]
	RomanceStandardTime = 38,
	[StringValue("W. Europe Standard Time")]
	WEuropeStandardTime = 39,
	[StringValue("W. Central Africa Standard Time")]
	WCentralAfricaStandardTime = 40,
	[StringValue("Namibia Standard Time")]
	NamibiaStandardTime = 41,
	[StringValue("E. Europe Standard Time")]
	EEuropeStandardTime = 42,
	[StringValue("Egypt Standard Time")]
	EgyptStandardTime = 43,
	[StringValue("FLE Standard Time")]
	FLEStandardTime = 44,
	[StringValue("GTB Standard Time")]
	GTBStandardTime = 45,
	[StringValue("South Africa Standard Time")]
	SouthAfricaStandardTime = 46,
	[StringValue("Jordan Standard Time")]
	JordanStandardTime = 48,
	[StringValue("Middle East Standard Time")]
	MiddleEastStandardTime = 49,
	[StringValue("Russian Standard Time")]
	RussianStandardTime = 51,
	[StringValue("Arab Standard Time")]
	ArabStandardTime = 52,
	[StringValue("E. Africa Standard Time")]
	EAfricaStandardTime = 53,
	[StringValue("Arabic Standard Time")]
	ArabicStandardTime = 54,
	[StringValue("Iran Standard Time")]
	IranStandardTime = 55,
	[StringValue("Georgian Standard Time")]
	GeorgianStandardTime = 56,
	[StringValue("Arabian Standard Time")]
	ArabianStandardTime = 57,
	[StringValue("Caucasus Standard Time")]
	CaucasusStandardTime = 58,
	[StringValue("Azerbaijan Standard Time")]
	AzerbaijanStandardTime = 59,
	[StringValue("Mauritius Standard Time")]
	MauritiusStandardTime = 60,
	[StringValue("Afghanistan Standard Time")]
	AfghanistanStandardTime = 61,
	[StringValue("West Asia Standard Time")]
	WestAsiaStandardTime = 63,
	[StringValue("Pakistan Standard Time")]
	PakistanStandardTime = 64,
	[StringValue("India Standard Time")]
	IndiaStandardTime = 65,
	[StringValue("Sri Lanka Standard Time")]
	SriLankaStandardTime = 66,
	[StringValue("Nepal Standard Time")]
	NepalStandardTime = 67,
	[StringValue("Central Asia Standard Time")]
	CentralAsiaStandardTime = 68,
	[StringValue("Ekaterinburg Standard Time")]
	EkaterinburgStandardTime = 69,
	[StringValue("Myanmar Standard Time")]
	MyanmarStandardTime = 70,
	[StringValue("N. Central Asia Standard Time")]
	NCentralAsiaStandardTime = 71,
	[StringValue("SE Asia Standard Time")]
	SEAsiaStandardTime = 72,
	[StringValue("North Asia Standard Time")]
	NorthAsiaStandardTime = 73,
	[StringValue("China Standard Time")]
	ChinaStandardTime = 74,
	[StringValue("Taipei Standard Time")]
	TaipeiStandardTime = 75,
	[StringValue("W. Australia Standard Time")]
	WAustraliaStandardTime = 76,
	[StringValue("North Asia East Standard Time")]
	NorthAsiaEastStandardTime = 77,
	[StringValue("Korea Standard Time")]
	KoreaStandardTime = 80,
	[StringValue("Tokyo Standard Time")]
	TokyoStandardTime = 81,
	[StringValue("AUS Central Standard Time")]
	AUSCentralStandardTime = 82,
	[StringValue("Cen. Australia Standard Time")]
	CenAustraliaStandardTime = 83,
	[StringValue("AUS Eastern Standard Time")]
	AUSEasternStandardTime = 84,
	[StringValue("E. Australia Standard Time")]
	EAustraliaStandardTime = 85,
	[StringValue("Tasmania Standard Time")]
	TasmaniaStandardTime = 86,
	[StringValue("Yakutsk Standard Time")]
	YakutskStandardTime = 87,
	[StringValue("West Pacific Standard Time")]
	WestPacificStandardTime = 88,
	[StringValue("Vladivostok Standard Time")]
	VladivostokStandardTime = 89,
	[StringValue("Central Pacific Standard Time")]
	CentralPacificStandardTime = 90,
	[StringValue("Magadan Standard Time")]
	MagadanStandardTime = 91,
	[StringValue("Fiji Standard Time")]
	FijiStandardTime = 92,
	[StringValue("New Zealand Standard Time")]
	NewZealandStandardTime = 93,
	[StringValue("Tonga Standard Time")]
	TongaStandardTime = 95
}
