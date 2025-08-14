namespace SmartHome.SHC.API.Protocols.wMBus;

public enum DeviceTypeIdentification : byte
{
	Other = 0,
	Oil = 1,
	Electricity = 2,
	Gas = 3,
	Heat = 4,
	Steam = 5,
	WarmWater = 6,
	Water = 7,
	HeatCostAllocator = 8,
	CompressedAir = 9,
	CoolingLoadMeterOutlet = 10,
	CoolingLoadMeterInlet = 11,
	HeatFlowInlet = 12,
	HeatCoolingLoadMeter = 13,
	BusSystemComponent = 14,
	UnknownMedium = 15,
	HotWater = 21,
	ColdWater = 22,
	DualRegisterWaterMeter = 23,
	Pressure = 24,
	ADConverter = 25,
	ReservedValve = 33
}
