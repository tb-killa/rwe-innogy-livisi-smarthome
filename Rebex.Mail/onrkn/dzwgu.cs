using System;
using System.Collections.Generic;
using Rebex.OutlookMessages;

namespace onrkn;

internal static class dzwgu
{
	internal static class rtraf
	{
		public const string cjfop = "__substg1.0_";

		public const string tkkoo = "__nameid_version1.0";

		public const string faxfh = "__recip_version1.0_#";

		public const string itbxl = "__attach_version1.0_#";

		public const string zcuie = "__nameid_version1.0/__substg1.0_00020102";

		public const string dwmdk = "__nameid_version1.0/__substg1.0_00030102";

		public const string uixrp = "__nameid_version1.0/__substg1.0_00040102";

		public const string dnhul = "__properties_version1.0";
	}

	private static Dictionary<MsgPropertySet, Guid> aodqz;

	private static Dictionary<Guid, MsgPropertySet> upumi;

	public static Dictionary<MsgPropertySet, Guid> sazkf => aodqz;

	public static Dictionary<Guid, MsgPropertySet> ymgvg => upumi;

	static dzwgu()
	{
		aodqz = new Dictionary<MsgPropertySet, Guid>();
		aodqz.Add(MsgPropertySet.Mapi, new Guid("00020328-0000-0000-C000-000000000046"));
		aodqz.Add(MsgPropertySet.PublicStrings, new Guid("00020329-0000-0000-C000-000000000046"));
		aodqz.Add(MsgPropertySet.InternetHeaders, new Guid("00020386-0000-0000-C000-000000000046"));
		aodqz.Add(MsgPropertySet.Common, new Guid("00062008-0000-0000-C000-000000000046"));
		aodqz.Add(MsgPropertySet.Address, new Guid("00062004-0000-0000-C000-000000000046"));
		aodqz.Add(MsgPropertySet.Appointment, new Guid("00062002-0000-0000-C000-000000000046"));
		aodqz.Add(MsgPropertySet.Meeting, new Guid("6ED8DA90-450B-101B-98DA-00AA003F1305"));
		aodqz.Add(MsgPropertySet.Log, new Guid("0006200A-0000-0000-C000-000000000046"));
		aodqz.Add(MsgPropertySet.Messaging, new Guid("41F28F13-83F4-4114-A584-EEDB5A6B0BFF"));
		aodqz.Add(MsgPropertySet.Note, new Guid("0006200E-0000-0000-C000-000000000046"));
		aodqz.Add(MsgPropertySet.Rss, new Guid("00062041-0000-0000-C000-000000000046"));
		aodqz.Add(MsgPropertySet.Task, new Guid("00062003-0000-0000-C000-000000000046"));
		aodqz.Add(MsgPropertySet.UnifiedMessaging, new Guid("4442858E-A9E3-4E80-B900-317A210CC15B"));
		aodqz.Add(MsgPropertySet.AirSync, new Guid("71035549-0739-4DCB-9163-00F0580DBBDF"));
		aodqz.Add(MsgPropertySet.Sharing, new Guid("00062040-0000-0000-C000-000000000046"));
		aodqz.Add(MsgPropertySet.CalendarAsistant, new Guid("11000E07-B51B-40D6-AF21-CAA85EDAB1D0"));
		aodqz.Add(MsgPropertySet.Attachment, new Guid("96357f7f-59e1-47d0-99a7-46515c183b54"));
		aodqz.Add(MsgPropertySet.Report, new Guid("00062013-0000-0000-C000-000000000046"));
		aodqz.Add(MsgPropertySet.RemoteMessaging, new Guid("00062014-0000-0000-C000-000000000046"));
		upumi = new Dictionary<Guid, MsgPropertySet>();
		using Dictionary<MsgPropertySet, Guid>.KeyCollection.Enumerator enumerator = aodqz.Keys.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			MsgPropertySet current = enumerator.Current;
			upumi.Add(sazkf[current], current);
		}
	}

	public static bool dovit(xcrar p0)
	{
		if (p0 != xcrar.xmyux)
		{
			return p0 == xcrar.bkapb;
		}
		return true;
	}

	public static bool vghtx(xcrar p0)
	{
		if (p0 != xcrar.wymks)
		{
			return p0 == xcrar.kkdjd;
		}
		return true;
	}

	public static bool qpszk(xcrar p0)
	{
		if (p0 != xcrar.cjxwq)
		{
			return p0 == xcrar.dddyf;
		}
		return true;
	}

	public static bool jefwi(xcrar p0)
	{
		if (p0 != xcrar.mctvt)
		{
			return p0 == xcrar.rkomo;
		}
		return true;
	}

	public static string njsop<T>()
	{
		string name = typeof(T).Name;
		if (name == "Nullable`1" || name == "Nullable")
		{
			return Nullable.GetUnderlyingType(typeof(T)).Name;
		}
		return name;
	}

	public static bool pmrst<T>(xcrar p0, bool p1 = false)
	{
		return hlbog(njsop<T>(), p0, p1);
	}

	public static bool gesog(Type p0, xcrar p1, bool p2 = false)
	{
		string name = p0.Name;
		if (name == "Nullable`1" || name == "Nullable")
		{
			name = Nullable.GetUnderlyingType(p0).Name;
		}
		return hlbog(name, p1, p2);
	}

	private static bool hlbog(string p0, xcrar p1, bool p2)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (czzgh.wxqsw == null || 1 == 0)
			{
				czzgh.wxqsw = new Dictionary<string, int>(19)
				{
					{ "String", 0 },
					{ "String[]", 1 },
					{ "Boolean", 2 },
					{ "Int16", 3 },
					{ "Int32", 4 },
					{ "Int32[]", 5 },
					{ "Int64", 6 },
					{ "Int64[]", 7 },
					{ "DateTime", 8 },
					{ "DateTime[]", 9 },
					{ "Byte[]", 10 },
					{ "Byte[][]", 11 },
					{ "Guid", 12 },
					{ "Guid[]", 13 },
					{ "Single", 14 },
					{ "Single[]", 15 },
					{ "Double", 16 },
					{ "Double[]", 17 },
					{ "Object", 18 }
				};
			}
			if (czzgh.wxqsw.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
					return dovit(p1);
				case 1:
					return vghtx(p1);
				case 2:
					return p1 == xcrar.xnjos;
				case 3:
					return p1 == xcrar.plwre;
				case 4:
					return p1 == xcrar.rjogj;
				case 5:
					return p1 == xcrar.qkgmc;
				case 6:
					return p1 == xcrar.yzzqc;
				case 7:
					return p1 == xcrar.kxwum;
				case 8:
					return qpszk(p1);
				case 9:
					return jefwi(p1);
				case 10:
					return p1 == xcrar.yesjh;
				case 11:
					return p1 == xcrar.vbevq;
				case 12:
					return p1 == xcrar.qfwqv;
				case 13:
					return p1 == xcrar.fawvf;
				case 14:
					return p1 == xcrar.xrlus;
				case 15:
					return p1 == xcrar.zfjkc;
				case 16:
					return p1 == xcrar.ysatw;
				case 17:
					return p1 == xcrar.gibpf;
				case 18:
					return p2;
				}
			}
		}
		return false;
	}

	public static void ckeqq(MsgPropertySet p0)
	{
		switch (p0)
		{
		case MsgPropertySet.Mapi:
		case MsgPropertySet.PublicStrings:
		case MsgPropertySet.InternetHeaders:
		case MsgPropertySet.Common:
		case MsgPropertySet.Address:
		case MsgPropertySet.Appointment:
		case MsgPropertySet.Meeting:
		case MsgPropertySet.Log:
		case MsgPropertySet.Messaging:
		case MsgPropertySet.Note:
		case MsgPropertySet.Rss:
		case MsgPropertySet.Task:
		case MsgPropertySet.UnifiedMessaging:
		case MsgPropertySet.AirSync:
		case MsgPropertySet.Sharing:
		case MsgPropertySet.CalendarAsistant:
		case MsgPropertySet.Attachment:
		case MsgPropertySet.Report:
		case MsgPropertySet.RemoteMessaging:
			return;
		}
		throw hifyx.nztrs("set", p0, "Argument is out of range of valid values.");
	}

	public static bool kqeon(string p0)
	{
		if (string.IsNullOrEmpty(p0) && 0 == 0)
		{
			return false;
		}
		if (p0[0] != '/')
		{
			return p0.IndexOf('@') > 0;
		}
		return false;
	}
}
