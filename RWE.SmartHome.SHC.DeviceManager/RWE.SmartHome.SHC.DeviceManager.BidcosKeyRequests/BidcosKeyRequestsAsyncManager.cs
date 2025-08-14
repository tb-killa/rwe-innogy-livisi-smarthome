using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.DeviceManager.BidcosKeyRequests;

public class BidcosKeyRequestsAsyncManager
{
	private static readonly TimeSpan LifeTimeRequest = TimeSpan.FromMinutes(5.0);

	private readonly IEventManager eventManager;

	private readonly IDictionary<SGTIN96, BidcosKeyRequestAsync> requests = new Dictionary<SGTIN96, BidcosKeyRequestAsync>();

	private readonly object sync = new object();

	public BidcosKeyRequestsAsyncManager(IEventManager eventManager)
	{
		this.eventManager = eventManager;
	}

	public void RequestKey(SGTIN96 sgtin, Action<SGTIN96, byte[]> receivedKeyCallback)
	{
		lock (sync)
		{
			CleanRequests();
			if (!requests.ContainsKey(sgtin))
			{
				BidcosKeyRequestAsync bidcosKeyRequestAsync = new BidcosKeyRequestAsync(eventManager, sgtin, receivedKeyCallback);
				requests.Add(sgtin, bidcosKeyRequestAsync);
				bidcosKeyRequestAsync.RetrieveKeyAync();
			}
		}
	}

	private void CleanRequests()
	{
		DateTime now = DateTime.Now;
		List<SGTIN96> list = (from m in requests.Values
			where m.IsRequestFinished() || now - m.GetRequestedTime() > LifeTimeRequest
			select m.GetDeviceSGTIN()).ToList();
		list.ForEach(delegate(SGTIN96 m)
		{
			requests.Remove(m);
		});
	}
}
