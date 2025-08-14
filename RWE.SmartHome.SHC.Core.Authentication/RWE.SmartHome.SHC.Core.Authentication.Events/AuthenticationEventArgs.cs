using System;
using RWE.SmartHome.SHC.Core.Authentication.Entities;

namespace RWE.SmartHome.SHC.Core.Authentication.Events;

public class AuthenticationEventArgs : EventArgs
{
	public AuthenticationEventType EventType { get; set; }

	public User User { get; set; }
}
