using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.NotificationSender;

internal class NotificationSenderValidator : IConfigurationValidator
{
	public IEnumerable<ErrorEntry> GetConfigurationErrors(IRepository configuration, RepositoryUpdateContextData updateContextData)
	{
		List<ErrorEntry> errors = new List<ErrorEntry>();
		IEnumerable<BaseDevice> source = from d in configuration.GetBaseDevices()
			where d.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.NotificationSender
			select d;
		if (!source.Any())
		{
			errors.Add(new ErrorEntry
			{
				ErrorCode = ValidationErrorCode.NotificationSenderDeletion
			});
		}
		configuration.GetInteractions().ForEach(delegate(Interaction i)
		{
			i.Rules.SelectMany((Rule r) => r.Actions).ToList().ForEach(delegate(ActionDescription action)
			{
				if (NotificationSenderConstants.IsSendAction(action.ActionType))
				{
					errors.AddRange(ValidateRecipients(i, action));
					errors.AddRange(ValidateNotificationBody(i, action));
				}
			});
		});
		return errors;
	}

	private IEnumerable<ErrorEntry> ValidateSendLimitInterval(Interaction i, ActionDescription action)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		Parameter parameter = action.Data.FirstOrDefault((Parameter x) => x.Name == "SendLimitInterval");
		if (parameter == null)
		{
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.Interaction,
					Id = i.Id
				},
				ErrorCode = ValidationErrorCode.InvalidInteraction,
				ErrorParameters = new List<ErrorParameter>
				{
					new ErrorParameter
					{
						Key = ErrorParameterKey.ParameterName,
						Value = string.Format("{0} parameter not present", "SendLimitInterval")
					}
				}
			});
			return list;
		}
		if (!(parameter.Value is ConstantStringBinding))
		{
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.Interaction,
					Id = i.Id
				},
				ErrorCode = ValidationErrorCode.InvalidInteraction,
				ErrorParameters = new List<ErrorParameter>
				{
					new ErrorParameter
					{
						Key = ErrorParameterKey.ParameterValue,
						Value = string.Format("{0} value is not of type string", "SendLimitInterval")
					}
				}
			});
			return list;
		}
		string value = (parameter.Value as ConstantStringBinding).Value;
		if (value != "Month" && value != "Day" && value != "Week")
		{
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.Interaction,
					Id = i.Id
				},
				ErrorCode = ValidationErrorCode.InvalidInteraction,
				ErrorParameters = new List<ErrorParameter>
				{
					new ErrorParameter
					{
						Key = ErrorParameterKey.ParameterValue,
						Value = string.Format("{0} value is not valid", "SendLimitInterval")
					}
				}
			});
		}
		return list;
	}

	private IEnumerable<ErrorEntry> ValidateSendLimitCount(Interaction i, ActionDescription action)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		Parameter parameter = action.Data.FirstOrDefault((Parameter x) => x.Name == "SendLimitCount");
		if (parameter == null)
		{
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.Interaction,
					Id = i.Id
				},
				ErrorCode = ValidationErrorCode.InvalidInteraction,
				ErrorParameters = new List<ErrorParameter>
				{
					new ErrorParameter
					{
						Key = ErrorParameterKey.ParameterName,
						Value = string.Format("{0} parameter not present", "SendLimitCount")
					}
				}
			});
			return list;
		}
		if (!(parameter.Value is ConstantNumericBinding))
		{
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.Interaction,
					Id = i.Id
				},
				ErrorCode = ValidationErrorCode.InvalidInteraction,
				ErrorParameters = new List<ErrorParameter>
				{
					new ErrorParameter
					{
						Key = ErrorParameterKey.ParameterValue,
						Value = string.Format("{0} value is not numeric", "SendLimitCount")
					}
				}
			});
			return list;
		}
		if (!TryParse((parameter.Value as ConstantNumericBinding).ValueAsString, out var _))
		{
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.Interaction,
					Id = i.Id
				},
				ErrorCode = ValidationErrorCode.InvalidInteraction,
				ErrorParameters = new List<ErrorParameter>
				{
					new ErrorParameter
					{
						Key = ErrorParameterKey.ParameterValue,
						Value = string.Format("{0} value is not valid", "SendLimitCount")
					}
				}
			});
		}
		return list;
	}

	private IEnumerable<ErrorEntry> ValidateNotificationBody(Interaction i, ActionDescription action)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		Parameter parameter = action.Data.FirstOrDefault((Parameter x) => x.Name == "NotificationBody");
		if (parameter == null)
		{
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.Interaction,
					Id = i.Id
				},
				ErrorCode = ValidationErrorCode.InvalidInteraction,
				ErrorParameters = new List<ErrorParameter>
				{
					new ErrorParameter
					{
						Key = ErrorParameterKey.ParameterName,
						Value = string.Format("{0} parameter not present", "NotificationBody")
					}
				}
			});
			return list;
		}
		if (!(parameter.Value is ConstantStringBinding))
		{
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.Interaction,
					Id = i.Id
				},
				ErrorCode = ValidationErrorCode.InvalidInteraction,
				ErrorParameters = new List<ErrorParameter>
				{
					new ErrorParameter
					{
						Key = ErrorParameterKey.ParameterValue,
						Value = string.Format("{0} value is not of type string", "NotificationBody")
					}
				}
			});
			return list;
		}
		if (string.IsNullOrEmpty((parameter.Value as ConstantStringBinding).Value))
		{
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.Interaction,
					Id = i.Id
				},
				ErrorCode = ValidationErrorCode.InvalidInteraction,
				ErrorParameters = new List<ErrorParameter>
				{
					new ErrorParameter
					{
						Key = ErrorParameterKey.ParameterValue,
						Value = string.Format("{0} can not be empty", "NotificationBody")
					}
				}
			});
		}
		return list;
	}

	private List<ErrorEntry> ValidateRecipients(Interaction i, ActionDescription action)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		int recipientsCount = GetRecipientsCount(action.Data);
		if (recipientsCount > 32)
		{
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.Interaction,
					Id = i.Id
				},
				ErrorCode = ValidationErrorCode.NotificationSenderMaxRecipientsReached,
				ErrorParameters = new List<ErrorParameter>
				{
					new ErrorParameter
					{
						Key = ErrorParameterKey.LicenseState,
						Value = $"{recipientsCount}/{32}"
					}
				}
			});
		}
		if (recipientsCount < 1)
		{
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.Interaction,
					Id = i.Id
				},
				ErrorCode = ValidationErrorCode.InvalidInteraction,
				ErrorParameters = new List<ErrorParameter>
				{
					new ErrorParameter
					{
						Key = ErrorParameterKey.ParameterValue,
						Value = string.Format("Must have at least one {0}", "RecipientAccountNames")
					}
				}
			});
		}
		return list;
	}

	private int GetRecipientsCount(IEnumerable<Parameter> actionParams)
	{
		IEnumerable<ConstantStringBinding> source = from prm in actionParams
			where prm.Name == "RecipientAccountNames" || prm.Name == "CustomRecipients"
			select prm.Value as ConstantStringBinding into rcp
			where rcp != null
			select rcp;
		int recipientCount = 0;
		source.ToList().ForEach(delegate(ConstantStringBinding x)
		{
			recipientCount += x.Value.Split(',').Count();
		});
		return recipientCount;
	}

	private bool TryParse(string s, out int value)
	{
		value = 0;
		try
		{
			value = int.Parse(s);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
}
