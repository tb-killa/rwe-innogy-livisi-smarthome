using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Configuration;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService;
using WebServerHost.Helpers;
using WebServerHost.Web;

namespace WebServerHost.Controllers.ClientAPI;

[Route("interaction")]
public class InteractionController : Controller
{
	private const RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType InteractionEntityType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Interaction;

	private readonly IInteractionConverterService interactionConverter;

	private readonly IShcClient shcClient;

	public InteractionController(IInteractionConverterService interactionConverter, IShcClient shcClient)
	{
		this.interactionConverter = interactionConverter;
		this.shcClient = shcClient;
	}

	[HttpGet]
	[Route("")]
	public IEnumerable<SmartHome.Common.API.Entities.Entities.Interaction> GetInteractions(string tkey, string tval)
	{
		List<SmartHome.Common.API.Entities.Entities.Interaction> list = GetAllInterations();
		if (!tkey.IsNullOrEmpty() && !tval.IsNullOrEmpty())
		{
			list = list.Where((SmartHome.Common.API.Entities.Entities.Interaction interaction) => interaction.Tags != null && interaction.Tags.Any((SmartHome.Common.API.Entities.Entities.Property t) => t.Name == tkey && t.Value.Equals(tval))).ToList();
		}
		return list;
	}

	[Route("{id}")]
	[HttpGet]
	public SmartHome.Common.API.Entities.Entities.Interaction GetInteraction([FromRoute] string id)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		SmartHome.Common.API.Entities.Entities.Interaction interaction = GetAllInterations().FirstOrDefault((SmartHome.Common.API.Entities.Entities.Interaction existingInteraction) => string.Equals(existingInteraction.Id, id));
		if (interaction == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Interaction not found {id}");
		}
		return interaction;
	}

	[Route("")]
	[HttpPost]
	public IResult Create([FromBody] SmartHome.Common.API.Entities.Entities.Interaction interaction)
	{
		if (interaction == null)
		{
			throw new ApiException(ErrorCode.EntityMalformedContent, $"Interaction Invalid");
		}
		if (GetAllInterations().FirstOrDefault((SmartHome.Common.API.Entities.Entities.Interaction existingInteraction) => string.Equals(existingInteraction.Id, interaction.Id)) != null)
		{
			throw new ApiException(ErrorCode.EntityAlreadyExists, $"Interaction aleready exists with id: {interaction.Id}");
		}
		return CreateInteraction(interaction);
	}

	[HttpPut]
	[Route("{id}")]
	public IResult Update([FromRoute] string id, [FromBody] SmartHome.Common.API.Entities.Entities.Interaction interaction)
	{
		if (interaction == null || id != interaction.Id)
		{
			throw new ApiException(ErrorCode.EntityMalformedContent, $"Interaction Invalid");
		}
		if (GetAllInterations().FirstOrDefault((SmartHome.Common.API.Entities.Entities.Interaction existingInteraction) => string.Equals(existingInteraction.Id, interaction.Id)) == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Interaction not found with id: {interaction.Id}");
		}
		return CreateInteraction(interaction);
	}

	[HttpDelete]
	[Route("{id}")]
	public IResult Delete([FromRoute] string id)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		if (GetAllInterations().FirstOrDefault((SmartHome.Common.API.Entities.Entities.Interaction existingInteraction) => string.Equals(existingInteraction.Id, id)) == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Interaction not found with id: {id}");
		}
		DeleteEntitiesRequest deleteEntitiesRequest = ShcRequestHelper.NewRequest<DeleteEntitiesRequest>();
		deleteEntitiesRequest.Entities.Add(new EntityMetadata
		{
			EntityType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Interaction,
			Id = id.ToGuid()
		});
		BaseResponse response = shcClient.GetResponse(deleteEntitiesRequest);
		if (response != null)
		{
			if (response is AcknowledgeResponse)
			{
				return Ok();
			}
			if (response is ErrorResponse errorResponse)
			{
				throw new ApiException(ErrorHelper.GetError(errorResponse));
			}
		}
		throw new ApiException(ErrorCode.InternalError, "Unkown Error");
	}

	[HttpGet]
	[Route("{id}/rules")]
	public List<SmartHome.Common.API.Entities.Entities.Rule> GetRules([FromRoute] string id)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		SmartHome.Common.API.Entities.Entities.Interaction interaction = GetAllInterations().FirstOrDefault((SmartHome.Common.API.Entities.Entities.Interaction existingInteraction) => string.Equals(existingInteraction.Id, id));
		if (interaction == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Interaction not found {id}");
		}
		return interaction.Rules;
	}

	[Route("{id}/rule/{ruleId}")]
	[HttpGet]
	public SmartHome.Common.API.Entities.Entities.Rule GetRule([FromRoute] string id, [FromRoute] string ruleId)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var output) || ruleId.IsNullOrEmpty() || !ruleId.GuidTryParse(out output))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		SmartHome.Common.API.Entities.Entities.Interaction interaction = GetAllInterations().FirstOrDefault((SmartHome.Common.API.Entities.Entities.Interaction existingInteraction) => string.Equals(existingInteraction.Id, id));
		if (interaction == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Interaction not found {id}");
		}
		SmartHome.Common.API.Entities.Entities.Rule rule = interaction.Rules.FirstOrDefault((SmartHome.Common.API.Entities.Entities.Rule existingRule) => string.Equals(existingRule.Id, ruleId));
		if (rule == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Rule with id: {ruleId} was not found in the interaction with id: {id}");
		}
		return rule;
	}

	private List<SmartHome.Common.API.Entities.Entities.Interaction> GetAllInterations()
	{
		List<SmartHome.Common.API.Entities.Entities.Interaction> list = new List<SmartHome.Common.API.Entities.Entities.Interaction>();
		GetEntitiesRequest getEntitiesRequest = ShcRequestHelper.NewRequest<GetEntitiesRequest>();
		getEntitiesRequest.EntityType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Interaction;
		BaseResponse response = shcClient.GetResponse(getEntitiesRequest);
		if (response != null && response is GetEntitiesResponse getEntitiesResponse)
		{
			foreach (RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Interaction interaction in getEntitiesResponse.Interactions)
			{
				list.Add(interactionConverter.FromSmartHomeInteraction(interaction));
			}
			if (response is ErrorResponse errorResponse)
			{
				throw new ApiException(ErrorHelper.GetError(errorResponse));
			}
		}
		return list;
	}

	private IResult CreateInteraction(SmartHome.Common.API.Entities.Entities.Interaction interaction)
	{
		SetEntitiesRequest setEntitiesRequest = ShcRequestHelper.NewRequest<SetEntitiesRequest>();
		setEntitiesRequest.Interactions.Add(interactionConverter.ToSmartHomeInteraction(interaction));
		BaseResponse response = shcClient.GetResponse(setEntitiesRequest);
		if (response != null)
		{
			if (response is AcknowledgeResponse)
			{
				return Ok();
			}
			if (response is ErrorResponse errorResponse)
			{
				throw new ApiException(ErrorHelper.GetError(errorResponse));
			}
		}
		throw new ApiException(ErrorCode.InternalError, "Unkown Error");
	}
}
