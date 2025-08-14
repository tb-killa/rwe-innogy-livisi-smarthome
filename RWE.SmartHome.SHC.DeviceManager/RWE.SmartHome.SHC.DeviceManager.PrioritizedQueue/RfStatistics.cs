using System;
using System.Collections.Generic;
using System.Text;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public class RfStatistics
{
	private const string LoggingSource = "Statistics";

	private object syncRoot = new object();

	private readonly Dictionary<SendStatus, uint> sendStatusStatistics = new Dictionary<SendStatus, uint>();

	private long appAcks;

	private long timedOutAppAcks;

	private IEventManager eventManager;

	private IScheduler scheduler;

	private int COMM_FAILURE_TRESHOLD = 80;

	private int COMM_FAILURE_EVALUATION_PERIOD = 5;

	private List<SendStatus> communicationErrors = new List<SendStatus>
	{
		SendStatus.BUSY,
		SendStatus.TIMEOUT,
		SendStatus.SERIAL_TIMEOUT,
		SendStatus.MEDIUM_BUSY,
		SendStatus.ERROR,
		SendStatus.INCOMMING,
		SendStatus.CRC_ERROR,
		SendStatus.MODE_ERROR,
		SendStatus.DUTY_CYCLE
	};

	private long prevErrors;

	private long prevNonErrors;

	private RfCommunicationStates prevState = RfCommunicationStates.Restored;

	public RfStatistics(IEventManager eventManager, IScheduler scheduler, long reportIntervall)
	{
		this.eventManager = eventManager;
		this.scheduler = scheduler;
		this.scheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(Guid.NewGuid(), ReportStatistics, TimeSpan.FromMinutes(reportIntervall)));
		this.scheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(Guid.NewGuid(), EvaluateRfStatistics, TimeSpan.FromMinutes(COMM_FAILURE_EVALUATION_PERIOD)));
	}

	private void ReportStatistics()
	{
		Log.Information(Module.DeviceManager, "Statistics", CreateStatisticsReport());
	}

	public void AddSendResult(SendStatus sendStatus)
	{
		lock (syncRoot)
		{
			if (sendStatusStatistics.ContainsKey(sendStatus))
			{
				sendStatusStatistics[sendStatus]++;
			}
			else
			{
				sendStatusStatistics[sendStatus] = 1u;
			}
		}
	}

	public void AddAckResult(AckResult ackResult)
	{
		lock (syncRoot)
		{
			switch (ackResult)
			{
			case AckResult.Ack:
				appAcks++;
				break;
			case AckResult.Timeout:
				timedOutAppAcks++;
				break;
			}
		}
	}

	private string CreateStatisticsReport()
	{
		long num = 0L;
		long num2 = 0L;
		double num3 = 0.0;
		lock (syncRoot)
		{
			foreach (KeyValuePair<SendStatus, uint> sendStatusStatistic in sendStatusStatistics)
			{
				if (sendStatusStatistic.Key != SendStatus.ACK && sendStatusStatistic.Key != SendStatus.MULTI_CAST)
				{
					num2 += sendStatusStatistic.Value;
				}
				num += sendStatusStatistic.Value;
			}
			if (num != 0)
			{
				num3 = 100.0 / (double)num;
			}
			double num4 = 0.0;
			long num5 = appAcks + timedOutAppAcks;
			if (num5 != 0)
			{
				num4 = 100.0 / (double)num5 * (double)timedOutAppAcks;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("CoSIP-RF Statistics: Frames send: {0}, Errors: {1}({2:0.00}%), Timed out app ACKs: {3}({4:0.00}%)", num, num2, num3 * (double)num2, timedOutAppAcks, num4);
			for (int i = 0; i < 13; i++)
			{
				uint num6 = 0u;
				if (sendStatusStatistics.ContainsKey((SendStatus)i))
				{
					num6 = sendStatusStatistics[(SendStatus)i];
				}
				stringBuilder.AppendFormat(", {0}: {1}({2:0.00}%)", new object[3]
				{
					(SendStatus)i,
					num6,
					num3 * (double)num6
				});
			}
			Reset();
			return stringBuilder.ToString();
		}
	}

	private void Reset()
	{
		lock (syncRoot)
		{
			sendStatusStatistics.Clear();
			appAcks = 0L;
			timedOutAppAcks = 0L;
		}
	}

	private bool IsRFError(SendStatus sendStatus)
	{
		return communicationErrors.Contains(sendStatus);
	}

	internal void EvaluateRfStatistics()
	{
		Log.Debug(Module.DeviceManager, "Statistics", "Evaluating RF communication health. Next check will be performed at " + (DateTime.Now.ToUniversalTime() + TimeSpan.FromMinutes(5.0)).ToShortTimeString());
		long num = 0L;
		long num2 = 0L;
		lock (syncRoot)
		{
			foreach (KeyValuePair<SendStatus, uint> sendStatusStatistic in sendStatusStatistics)
			{
				if (IsRFError(sendStatusStatistic.Key))
				{
					num += sendStatusStatistic.Value;
				}
				else
				{
					num2 += sendStatusStatistic.Value;
				}
			}
		}
		long num3 = (long)Math.Ceiling((double)(num - prevErrors) / (double)(num - prevErrors + num2 - prevNonErrors) * 100.0);
		Log.Debug(Module.DeviceManager, "Statistics", "Current RF communication failure ratio: " + num3 + "%");
		UpdateCommErrorState(num3);
		prevErrors = num;
		prevNonErrors = num2;
	}

	private void UpdateCommErrorState(long failureRate)
	{
		RfCommunicationStates rfCommunicationStates = ((failureRate < COMM_FAILURE_TRESHOLD) ? RfCommunicationStates.Restored : RfCommunicationStates.Failed);
		if (prevState != rfCommunicationStates)
		{
			Log.Debug(Module.DeviceManager, "RF Communication state became: " + rfCommunicationStates);
			eventManager.GetEvent<RfCommunicationStateChangedEvent>().Publish(new RfCommunicationStateChangedEventArgs
			{
				CommunicationState = rfCommunicationStates
			});
			prevState = rfCommunicationStates;
		}
	}
}
