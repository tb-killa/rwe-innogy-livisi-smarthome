using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using SmartHome.SHC.API.Configuration;

namespace RWE.SmartHome.SHC.CoreApiConverters;

public static class ErrorEntryConverter
{
	public static ErrorEntry ToCoreErrorEntry(this ConfigurationError configError, string appId)
	{
		Guid id;
		try
		{
			id = new Guid(configError.Link.Id);
		}
		catch (Exception)
		{
			id = Guid.Empty;
		}
		ErrorEntry errorEntry = new ErrorEntry();
		errorEntry.AffectedEntity = new EntityMetadata
		{
			EntityType = configError.Link.Type.ToCore(),
			Id = id
		};
		errorEntry.ErrorCode = ValidationErrorCode.AddInValidationError;
		errorEntry.ErrorParameters = new List<ErrorParameter>(2)
		{
			new ErrorParameter
			{
				Key = ErrorParameterKey.ApplicationId,
				Value = appId
			},
			new ErrorParameter
			{
				Key = ErrorParameterKey.ApplicationPayload,
				Value = configError.Description
			}
		};
		return errorEntry;
	}
}
