using System;
using System.Collections.Generic;
using System.Threading;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;
using RWE.SmartHome.SHC.DisplayManagerInterfaces.Events;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces.Events;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;
using SHCWrapper.Drivers;
using SHCWrapper.Misc;

namespace RWE.SmartHome.SHC.DisplayManager;

internal class DisplayManager : Task, IDisplayManager, IService
{
	private struct ActiveWorkflow
	{
		public Workflow workflow;

		public string text;
	}

	private const int DELAY_WHILE_SCROLLING_MS = 200;

	private const int DELAY_BEFORE_SCROLLING_MS = 700;

	private const int DELAY_AFTER_SCROLLING_MS = 1200;

	private const int DELAY_AFTER_UPDATE_MS = 1000;

	private const int BLINK_FREQUENCY_MS = 500;

	private const int DATE_TIME_TOGGLE_FREQUENCY_MS = 5000;

	private const string BOOTING_MESSAGE = "BOOTING";

	private const string WELCOME_MESSAGE = "WELCOME";

	private const int DELAY_AFTER_BOOTING_MESSAGE = 200;

	private readonly Dictionary<WorkflowMessage, string> messageStrings = new Dictionary<WorkflowMessage, string>
	{
		{
			WorkflowMessage.GettingTime,
			"GET TIME"
		},
		{
			WorkflowMessage.DownloadingUpdate,
			"DOWNLOAD"
		},
		{
			WorkflowMessage.UsbBusy,
			"WRITING TO USB"
		},
		{
			WorkflowMessage.UsbDone,
			"WRITING TO USB COMPLETED"
		},
		{
			WorkflowMessage.Register,
			"REGISTER"
		},
		{
			WorkflowMessage.PressButton,
			"PRESS LEFT BUTTON"
		},
		{
			WorkflowMessage.UpdatingCoprocessor,
			"COPROCESSOR UPDATE"
		},
		{
			WorkflowMessage.UpdatingSHCFirmware,
			"UPDATING"
		}
	};

	private readonly Dictionary<WorkflowError, string> errorStrings = new Dictionary<WorkflowError, string>
	{
		{
			WorkflowError.SystemStartupError,
			"F 0010"
		},
		{
			WorkflowError.NetworkAdapterNotOperational,
			"F 0031"
		},
		{
			WorkflowError.NoDhcpIpAddress,
			"F 0032"
		},
		{
			WorkflowError.NoDhcpDefaultGateway,
			"F 0033"
		},
		{
			WorkflowError.NameResolutionFailed,
			"F 0034"
		},
		{
			WorkflowError.NameResolutionFailedNetworkDown,
			"F 0034"
		},
		{
			WorkflowError.NtpUnavailable,
			"F 0035"
		},
		{
			WorkflowError.SoftwareUpdateServiceUnavailable,
			"F 00A0"
		},
		{
			WorkflowError.SoftwareUpdateServiceResponseInvalid,
			"F 00A1"
		},
		{
			WorkflowError.SoftwareDownloadServiceUnavailable,
			"F 00A2"
		},
		{
			WorkflowError.SoftwareDownloadServiceResponseInvalid,
			"F 00A3"
		},
		{
			WorkflowError.FileSystemError,
			"F 00A4"
		},
		{
			WorkflowError.DirectFlashWriteError,
			"F 00A5"
		},
		{
			WorkflowError.InvalidSerialNumber,
			"F 00A6"
		},
		{
			WorkflowError.NoDefaultCertificateFound,
			"F 00A7"
		},
		{
			WorkflowError.UsbStickLogExport_UnsupportedFileSystem,
			"F 0073"
		},
		{
			WorkflowError.UsbStickLogExport_WriteFailed,
			"F 0076"
		},
		{
			WorkflowError.UsbStickLogExport_OtherError,
			"F 0076"
		},
		{
			WorkflowError.InitializationServiceUnavailable,
			"F 0021"
		},
		{
			WorkflowError.CoprocessorUpdateFailed,
			"F 0012"
		}
	};

	private readonly IEventManager eventManager;

	private readonly INetworkingMonitor networkingMonitor;

	private readonly object mutex = new object();

	private readonly List<ActiveWorkflow> activeWorkflows = new List<ActiveWorkflow>();

	private int nextActiveWorkflowToDisplay;

	private string scrollingText;

	private Workflow? scrollingWorkflow;

	private bool displayTime;

	private volatile bool stop;

	private bool button1Pressed;

	private bool button2Pressed;

	private bool devicesUnreachable;

	private bool ownerHasAlerts;

	private bool ownerHasMessages;

	private bool backendReachable = true;

	private bool shcInitialized;

	private bool shcRegistered;

	private int displayingNetworkInfoStep = -1;

	private readonly string[] networkInfoTexts = new string[3];

	private SubscriptionToken shcStartedSubscriptionToken;

	private SubscriptionToken channelConnectivitySubscriptionToken;

	private SubscriptionToken deviceUnreachableSubscriptionToken;

	private SubscriptionToken ownerMessageBoxSubscriptionToken;

	private Button button1;

	private Button button2;

	private int DisplayActiveWorkflows()
	{
		lock (mutex)
		{
			bool num = (Lcd.X3 = false);
			Lcd.X2 = num;
			Lcd.X1 = num;
			if (!ScrollText(out var delay))
			{
				if (nextActiveWorkflowToDisplay >= activeWorkflows.Count)
				{
					nextActiveWorkflowToDisplay = 0;
				}
				ActiveWorkflow activeWorkflow = activeWorkflows[nextActiveWorkflowToDisplay];
				nextActiveWorkflowToDisplay = (nextActiveWorkflowToDisplay + 1) % activeWorkflows.Count;
				return DisplayText(activeWorkflow.text, activeWorkflow.workflow);
			}
			return delay;
		}
	}

	private int DisplayNetworkInformation()
	{
		bool num = (Lcd.X3 = false);
		Lcd.X2 = num;
		Lcd.X1 = num;
		if (displayingNetworkInfoStep == -1)
		{
			networkInfoTexts[0] = $"VER: {SHCVersion.OsVersion}-{SHCVersion.ApplicationVersion}";
			networkInfoTexts[1] = "IP: " + NetworkTools.GetDeviceIp();
			networkInfoTexts[2] = "MAC: " + NetworkTools.GetDeviceMacAddress().Replace('-', ':').ToUpper();
			displayingNetworkInfoStep = 0;
			button2Pressed = false;
		}
		if (!ScrollText(out var delay))
		{
			return DisplayText(networkInfoTexts[displayingNetworkInfoStep], null);
		}
		if (scrollingText == null)
		{
			displayingNetworkInfoStep++;
			if (displayingNetworkInfoStep == networkInfoTexts.Length)
			{
				displayingNetworkInfoStep = -1;
			}
		}
		return delay;
	}

	private int DisplayText(string text, Workflow? workflow)
	{
		Lcd.Text = text;
		if (text.Length > Lcd.MAX_TEXT_LENGTH)
		{
			if (workflow.HasValue)
			{
				scrollingWorkflow = workflow;
			}
			scrollingText = text;
			return 700;
		}
		return 1000;
	}

	private bool ScrollText(out int delay)
	{
		if (scrollingText == null)
		{
			delay = 0;
			return false;
		}
		scrollingText = scrollingText.Substring(1);
		Lcd.Text = scrollingText;
		if (scrollingText.Length > Lcd.MAX_TEXT_LENGTH)
		{
			delay = 200;
		}
		else
		{
			scrollingText = null;
			scrollingWorkflow = null;
			delay = 1200;
		}
		return true;
	}

	private int DisplayDateOrTime()
	{
		if (displayTime)
		{
			bool num = (Lcd.X3 = false);
			Lcd.X2 = num;
			Lcd.X1 = num;
			Lcd.Text = ShcDateTime.Now.ToString(" HH:mm");
		}
		else
		{
			Lcd.X3 = false;
			Lcd.X1 = (Lcd.X2 = false);
			Lcd.Text = ShcDateTime.Now.ToString("dd.MM.yy");
		}
		displayTime = !displayTime;
		return 5000;
	}

	private static int DisplayBooting()
	{
		bool num = (Lcd.X3 = false);
		Lcd.X2 = num;
		Lcd.X1 = num;
		Lcd.Text = "BOOTING";
		return 200;
	}

	private static int DisplayWelcome()
	{
		bool num = (Lcd.X3 = false);
		Lcd.X2 = num;
		Lcd.X1 = num;
		Lcd.Text = "WELCOME";
		return 200;
	}

	private void RemoveActiveWorkflow(Workflow workflow)
	{
		lock (mutex)
		{
			int num = activeWorkflows.FindIndex((ActiveWorkflow item) => item.workflow == workflow);
			if (num >= 0)
			{
				activeWorkflows.RemoveAt(num);
				if (nextActiveWorkflowToDisplay > num)
				{
					nextActiveWorkflowToDisplay--;
				}
				if (scrollingWorkflow.HasValue && scrollingWorkflow.Value == workflow)
				{
					scrollingText = null;
					scrollingWorkflow = null;
				}
			}
		}
	}

	private void UpdateActiveWorkflow(Workflow workflow, string text)
	{
		lock (mutex)
		{
			ActiveWorkflow activeWorkflow = new ActiveWorkflow
			{
				workflow = workflow,
				text = text
			};
			int num = activeWorkflows.FindIndex((ActiveWorkflow item) => item.workflow == workflow);
			if (num < 0)
			{
				activeWorkflows.Add(activeWorkflow);
				nextActiveWorkflowToDisplay = activeWorkflows.Count - 1;
			}
			else
			{
				activeWorkflows[num] = activeWorkflow;
				nextActiveWorkflowToDisplay = num;
			}
		}
	}

	private void OnButtonPress(DisplayButton button)
	{
		eventManager.GetEvent<DisplayButtonPressedEvent>().Publish(new DisplayButtonPressedEventArgs
		{
			Button = button
		});
	}

	private void DeviceUnreachableChanged(AtLeastOneDeviceUnreachableChangedEventArgs args)
	{
		devicesUnreachable = args.DevicesUnreachable;
	}

	private void OwnerMessageboxChanged(OwnerMessageBoxChangedEventArgs args)
	{
		ownerHasAlerts = args.OwnerHasUnreadAlerts;
		ownerHasMessages = args.OwnerHasUnreadMessages;
	}

	public DisplayManager(IEventManager eventManager, INetworkingMonitor networkingMonitor)
	{
		base.Name = "DisplayManager";
		this.eventManager = eventManager;
		this.networkingMonitor = networkingMonitor;
	}

	public void WorkflowProceeded(Workflow workflow, WorkflowMessage message, bool forceDisplay)
	{
		if (messageStrings.ContainsKey(message))
		{
			string text = messageStrings[message];
			UpdateActiveWorkflow(workflow, text);
			if (forceDisplay)
			{
				DisplayActiveWorkflows();
				Lcd.Update();
				Thread.Sleep(500);
			}
		}
		else
		{
			RemoveActiveWorkflow(workflow);
		}
	}

	public void WorkflowDynamicMessage(Workflow workflow, string message)
	{
		UpdateActiveWorkflow(workflow, message);
	}

	public void WorkflowFinished(Workflow workflow)
	{
		RemoveActiveWorkflow(workflow);
		if (workflow == Workflow.Registration || workflow == Workflow.OwnershipReassignment)
		{
			shcRegistered = true;
		}
	}

	public void WorkflowFailed(Workflow workflow, WorkflowError workflowError)
	{
		try
		{
			if (errorStrings.ContainsKey(workflowError))
			{
				string text = errorStrings[workflowError];
				UpdateActiveWorkflow(workflow, text);
			}
			else
			{
				RemoveActiveWorkflow(workflow);
			}
		}
		catch
		{
		}
	}

	public bool UserHasRequestedFactoryReset()
	{
		return ResetManager.IsFactoryReset();
	}

	public void Initialize()
	{
		if (deviceUnreachableSubscriptionToken == null)
		{
			deviceUnreachableSubscriptionToken = eventManager.GetEvent<AtLeastOneDeviceUnreachableChangedEvent>().Subscribe(DeviceUnreachableChanged, null, ThreadOption.PublisherThread, null);
		}
		if (ownerMessageBoxSubscriptionToken == null)
		{
			ownerMessageBoxSubscriptionToken = eventManager.GetEvent<OwnerMessageBoxChangedEvent>().Subscribe(OwnerMessageboxChanged, null, ThreadOption.PublisherThread, null);
		}
		if (channelConnectivitySubscriptionToken == null)
		{
			channelConnectivitySubscriptionToken = eventManager.GetEvent<ChannelConnectivityChangedEvent>().Subscribe(ChannelConnectivityChanged, null, ThreadOption.PublisherThread, null);
		}
		if (shcStartedSubscriptionToken == null)
		{
			shcStartedSubscriptionToken = eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(ShcStarted, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound2, ThreadOption.PublisherThread, null);
		}
		button1 = new Button(Button.LCD_BUTTON_1, GPIOManager.LEVEL.LOW);
		button1.OnButtonPress += OnButton1Press;
		button2 = new Button(Button.LCD_BUTTON_2, GPIOManager.LEVEL.LOW);
		button2.OnButtonPress += OnButton2Press;
	}

	public void Uninitialize()
	{
		if (button2 != null)
		{
			button2.OnButtonPress -= OnButton2Press;
			button2.Dispose();
			button2 = null;
		}
		if (button1 != null)
		{
			button1.OnButtonPress -= OnButton1Press;
			button1.Dispose();
			button1 = null;
		}
		if (shcStartedSubscriptionToken != null)
		{
			eventManager.GetEvent<ShcStartupCompletedEvent>().Unsubscribe(shcStartedSubscriptionToken);
		}
		if (channelConnectivitySubscriptionToken != null)
		{
			eventManager.GetEvent<ChannelConnectivityChangedEvent>().Unsubscribe(channelConnectivitySubscriptionToken);
		}
		if (ownerMessageBoxSubscriptionToken != null)
		{
			eventManager.GetEvent<OwnerMessageBoxChangedEvent>().Unsubscribe(ownerMessageBoxSubscriptionToken);
		}
	}

	private void OnButton1Press()
	{
		button1Pressed = true;
		OnButtonPress(DisplayButton.Button1);
	}

	private void OnButton2Press()
	{
		button2Pressed = true;
		OnButtonPress(DisplayButton.Button2);
	}

	private void ChannelConnectivityChanged(ChannelConnectivityChangedEventArgs obj)
	{
		backendReachable = obj.Connected;
	}

	private void ShcStarted(ShcStartupCompletedEventArgs args)
	{
		shcInitialized = true;
	}

	public override void Stop()
	{
		stop = true;
	}

	protected override void Run()
	{
		stop = false;
		bool flag = true;
		int num = 0;
		int num2 = 0;
		while (!stop || flag)
		{
			if (stop)
			{
				flag = false;
			}
			try
			{
				if (num2 <= 0)
				{
					if (devicesUnreachable)
					{
						Lcd.Antenna = !Lcd.Antenna;
					}
					else
					{
						Lcd.Antenna = false;
					}
					if (networkingMonitor.InternetAccessAllowed)
					{
						if (backendReachable)
						{
							Lcd.NoInternet = false;
						}
						else
						{
							Lcd.NoInternet = !Lcd.NoInternet;
						}
					}
					else
					{
						Lcd.NoInternet = true;
					}
					Lcd.Alerts = ownerHasAlerts;
					Lcd.Messages = ownerHasMessages;
					num2 = 500;
				}
				if (num <= 0 || button1Pressed || button2Pressed)
				{
					button1Pressed = false;
					lock (mutex)
					{
						num = ((!button2Pressed && displayingNetworkInfoStep < 0) ? ((activeWorkflows.Count > 0) ? DisplayActiveWorkflows() : (shcInitialized ? DisplayDateOrTime() : ((!shcRegistered) ? DisplayBooting() : DisplayWelcome()))) : DisplayNetworkInformation());
					}
				}
				Lcd.Update();
				int num3 = Math.Min(num2, num);
				num2 -= num3;
				num -= num3;
				if (!stop)
				{
					Thread.Sleep(num3);
				}
			}
			catch
			{
			}
		}
	}
}
