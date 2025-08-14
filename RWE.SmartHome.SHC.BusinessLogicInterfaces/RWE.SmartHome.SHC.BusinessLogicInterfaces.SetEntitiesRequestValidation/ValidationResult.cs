using System.Collections.Generic;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

public class ValidationResult
{
	public List<string> Errors { get; private set; }

	public bool Valid => Errors.Count == 0;

	public string Reason => string.Join("\n", Errors.ToArray());

	public ValidationResult()
	{
		Errors = new List<string>();
	}

	public void Add(ValidationResult resultsSet)
	{
		if (!resultsSet.Valid)
		{
			Errors.AddRange(resultsSet.Errors);
		}
	}

	public void Add(ValidationResult resultsSet, string reason)
	{
		if (!resultsSet.Valid)
		{
			Add(reason);
			Errors.AddRange(resultsSet.Errors);
		}
	}

	public void Add(bool valid, string error)
	{
		if (!valid)
		{
			Errors.Add(error);
		}
	}

	public void Add(string error)
	{
		Errors.Add(error);
	}
}
