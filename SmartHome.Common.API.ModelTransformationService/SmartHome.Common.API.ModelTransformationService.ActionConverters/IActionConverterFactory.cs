using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters;

internal interface IActionConverterFactory
{
	IActionConverter GetActionConverter(Action action);
}
