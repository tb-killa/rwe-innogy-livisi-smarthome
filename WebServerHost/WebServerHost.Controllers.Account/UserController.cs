using System;
using System.Collections.Generic;
using System.Net;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.Authentication.Entities;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.DisplayManagerInterfaces.Events;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Entities.Account;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Extensions;
using WebServerHost.Web;
using WebServerHost.Web.Extensions;

namespace WebServerHost.Controllers.Account;

[Route("user")]
public class UserController : Controller
{
	private readonly ILocalUserManager userManager;

	private readonly IAuthorization auth;

	private readonly IRegistrationService registration;

	private readonly IEventManager eventManager;

	public UserController(ILocalUserManager userManager, IAuthorization auth, IRegistrationService registration, IEventManager eventManager)
	{
		this.userManager = userManager;
		this.auth = auth;
		this.registration = registration;
		this.eventManager = eventManager;
		eventManager.GetEvent<DisplayButtonPressedEvent>().Subscribe(OnButtonPress, null, ThreadOption.PublisherThread, null);
	}

	[HttpGet]
	[Route("")]
	public IResult Get()
	{
		SmartHome.Common.API.Entities.Entities.Account.User user = new SmartHome.Common.API.Entities.Entities.Account.User();
		user.AccountName = userManager.User.Name;
		user.Password = string.Empty;
		user.TenantId = userManager.User.Id.ToString();
		SmartHome.Common.API.Entities.Entities.Account.User user2 = user;
		if (userManager.User.Data.IsNotEmptyOrNull())
		{
			user2.Data = userManager.User.Data.FromJson<List<Property>>();
		}
		else
		{
			user2.Data = new List<Property>
			{
				new Property
				{
					Name = "latestTaCAccepted",
					Value = registration.TearmsAndConditionAccepted
				}
			};
		}
		return Ok(user2);
	}

	[Route("")]
	[HttpPut]
	public IResult Update([FromBody] SmartHome.Common.API.Entities.Entities.Account.User user)
	{
		if (userManager.User.Name != user.AccountName && userManager.User.Id != user.TenantId.ToGuid())
		{
			return NotFound();
		}
		userManager.UpdateUser(user.AccountName, userManager.User.Password, user.Data.ToJson());
		return Ok();
	}

	[Route("shcconfiguration")]
	[HttpGet]
	public IResult GetConfiguration()
	{
		return Ok(registration.GetShcConfiguration());
	}

	[HttpPut]
	[Route("{username}/password")]
	public IResult ChangePassword([FromRoute] string username, [FromBody] PasswordChange passwordChange)
	{
		LocalUser user = userManager.User;
		if (!user.Name.EqualsIgnoreCase(username))
		{
			return NotFound();
		}
		if (!registration.TearmsAndConditionAccepted)
		{
			if (DateTime.UtcNow > userManager.ResetPasswordWindow)
			{
				throw new ApiException(ErrorCode.InvalidPartner, "Password change window is not active");
			}
			if (!userManager.WasButtonPressed)
			{
				return Unauthorized();
			}
		}
		if (!PasswordHelper.Matches(passwordChange.OldPassword, user.Password))
		{
			throw new ApiException(ErrorCode.InvalidUserCredentials, "Old password missmatch");
		}
		UpdatePassword(passwordChange, user);
		return Ok();
	}

	private void UpdatePassword(PasswordChange passwordChange, LocalUser user)
	{
		string text = PasswordHelper.HashPassword(passwordChange.NewPassword);
		bool flag = text != user.Password;
		userManager.UpdateUser(user.Name, text, user.Data);
		userManager.WasButtonPressed = false;
		if (flag)
		{
			auth.InvalidateTokens();
		}
	}

	[HttpPost]
	[Route("initializeshc")]
	public IResult InitializeShc([FromBody] ShcInitialization initiaization)
	{
		registration.Register(initiaization);
		return Ok();
	}

	[HttpGet]
	[Route("{username}/password/change")]
	public IResult BeginPasswordChange()
	{
		userManager.ResetPasswordWindow = DateTime.UtcNow;
		userManager.ResetPasswordWindow = userManager.ResetPasswordWindow.AddMinutes(5.0);
		userManager.StartResetPasswordWindowTimer();
		return Ok();
	}

	[HttpGet]
	[Route("{username}/password/change/active")]
	public IResult IsResetFlowActive()
	{
		if (DateTime.UtcNow > userManager.ResetPasswordWindow)
		{
			throw new ApiException(HttpStatusCode.Unauthorized, "Password change window is not active");
		}
		List<Property> list = new List<Property>();
		list.Add(new Property
		{
			Name = "active",
			Value = userManager.WasButtonPressed
		});
		List<Property> data = list;
		return Ok(data);
	}

	[HttpPost]
	[Route("{username}/password/reset")]
	public IResult ResetPassword([FromRoute] string username, [FromBody] PasswordChange passwordChange)
	{
		if (DateTime.UtcNow > userManager.ResetPasswordWindow)
		{
			throw new ApiException(ErrorCode.InvalidPartner, "Password change window is not active");
		}
		if (!userManager.WasButtonPressed)
		{
			return Unauthorized();
		}
		LocalUser user = userManager.User;
		if (!user.Name.EqualsIgnoreCase(username))
		{
			return NotFound();
		}
		if (!PasswordHelper.Matches(passwordChange.OldPassword, userManager.DefaultPassword))
		{
			throw new ApiException(ErrorCode.InvalidUserCredentials, "Old password missmatch");
		}
		UpdatePassword(passwordChange, user);
		return Ok();
	}

	private void OnButtonPress(DisplayButtonPressedEventArgs args)
	{
		userManager.WasButtonPressed = true;
	}
}
