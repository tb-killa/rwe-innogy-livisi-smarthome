using System;

namespace SmartHome.Common.API.Entities.Enumerations;

[Flags]
public enum ClientPermission : ulong
{
	None = 0uL,
	CoreRead = 1uL,
	CoreWrite = 2uL,
	CoreFull = 3uL,
	Storage = 4uL,
	DataRead = 8uL,
	DataWrite = 0x10uL,
	DataFull = 0x18uL,
	Account = 0x20uL,
	AppMgmtRead = 0x40uL,
	AppMgmtWrite = 0x80uL,
	AppMgmtFull = 0xC0uL,
	WebShop = 0x100uL,
	ControllerAccess = 0x200uL,
	LogUpload = 0x400uL,
	Insights = 0x800uL,
	PresenceRead = 0x1000uL,
	PresenceWrite = 0x2000uL,
	PresenceFull = 0x3000uL,
	HomeRead = 0x4000uL,
	HomeWrite = 0x8000uL,
	HomeFull = 0xC000uL,
	MemberRead = 0x10000uL,
	MemberWrite = 0x20000uL,
	MemberFull = 0x30000uL,
	SmartCodeRead = 0x40000uL,
	SmartCodeWrite = 0x80000uL,
	SmartCodeFull = 0xC0000uL,
	All = ulong.MaxValue
}
