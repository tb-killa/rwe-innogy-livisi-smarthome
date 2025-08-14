using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.RequestValidation;

public class HomeSetupValidations : EntityValidations
{
	private const double latitudeMinValue = -69.0;

	private const double latitudeMaxValue = 71.0;

	private const double longitudeMinValue = -180.0;

	private const double longitudeMaxValue = 180.0;

	private const string HomeSetupNotExist = "HOME_SETUP_NOT_EXIST";

	private const string GeolocationIncorectFormat = "GEOLOCATION_INCORECT_FORMAT";

	private const string GeolocationIncorectRange = "GEOLOCATION_INCORECT_RANGE";

	private readonly IRepository repository;

	public HomeSetupValidations(IRepository repository)
	{
		this.repository = repository;
		InitRules();
	}

	public ValidationResult ValidateHomeSetupUpdates(IEnumerable<HomeSetup> homeSetups)
	{
		ValidationResult validationResult = new ValidationResult();
		foreach (HomeSetup homeSetup in homeSetups)
		{
			ValidationResult resultsSet = Validate(homeSetup, ValidateHomeSetupIds, ValidateSyntacticHomeSetup, ValidateGeolocation);
			validationResult.Add(resultsSet);
		}
		return validationResult;
	}

	private ValidationResult ValidateHomeSetupIds(HomeSetup homeSetup)
	{
		ValidationResult validationResult = new ValidationResult();
		Guid id = homeSetup.Id;
		if (repository.GetHomeSetup(id) == null)
		{
			validationResult.Add("HOME_SETUP_NOT_EXIST");
		}
		return validationResult;
	}

	private ValidationResult ValidateSyntacticHomeSetup(HomeSetup homeSetup)
	{
		List<Property> properties = homeSetup.Properties;
		return ValidateTypeProperties(properties);
	}

	private ValidationResult ValidateGeolocation(HomeSetup homeSetup)
	{
		ValidationResult validationResult = new ValidationResult();
		string stringValue = homeSetup.Properties.GetStringValue("GeoLocation");
		if (!string.IsNullOrEmpty(stringValue))
		{
			Match match = Regex.Match(stringValue, "^ *(?<lat>-?\\d+(\\.\\d+)?) *, *(?<long>-?\\d+(\\.\\d+)?) *$");
			if (match.Success && match.Groups["lat"].Success && match.Groups["long"].Success)
			{
				double num = Convert.ToDouble(match.Groups["lat"].Value);
				double num2 = Convert.ToDouble(match.Groups["long"].Value);
				if (num < -69.0 || num > 71.0 || num2 < -180.0 || num2 > 180.0)
				{
					validationResult.Add("GEOLOCATION_INCORECT_RANGE");
				}
			}
			else
			{
				validationResult.Add("GEOLOCATION_INCORECT_FORMAT");
			}
		}
		return validationResult;
	}

	private void InitRules()
	{
		AddValidation("GeoLocation", typeof(StringProperty), PropertyRequired.Optional);
		AddValidation("PostCode", typeof(StringProperty), PropertyRequired.Optional);
		AddValidation("Country", typeof(StringProperty), PropertyRequired.Optional);
		AddValidation("HouseholdType", typeof(StringProperty), PropertyRequired.Optional);
		AddValidation("NumberOfPersons", typeof(NumericProperty), PropertyRequired.Optional);
		AddValidation("NumberOfFloors", typeof(NumericProperty), PropertyRequired.Optional);
		AddValidation("LivingArea", typeof(NumericProperty), PropertyRequired.Optional);
	}
}
