using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;

public class GetSerializedEntitiesHandler : ISerializedResponseHandler, IBaseCommandHandler
{
	private readonly IEntityPersistence configurationPersistence;

	private readonly IRepository repository;

	public GetSerializedEntitiesHandler(IEntityPersistence entityPersistence, IRepository repository)
	{
		configurationPersistence = entityPersistence;
		this.repository = repository;
	}

	public SerializationResponse HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction)
	{
		if (request is GetEntitiesRequest request2)
		{
			return GetConfiguration(request2);
		}
		return null;
	}

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}

	private SerializationResponse GetConfiguration(GetEntitiesRequest request)
	{
		StringCollection stringCollection = new StringCollection();
		EntityType entityType = request.EntityType;
		stringCollection.AddCollection(GetSerializedEntity<Location>(entityType));
		stringCollection.AddCollection(GetSerializedEntity<LogicalDevice>(entityType));
		stringCollection.AddCollection(GetSerializedEntity<BaseDevice>(entityType));
		stringCollection.AddCollection(GetSerializedEntity<Interaction>(entityType));
		stringCollection.AddCollection(GetSerializedEntity<HomeSetup>(entityType));
		stringCollection.AddCollection(GetSerializedEntity<Home>(entityType));
		stringCollection.AddCollection(GetSerializedEntity<Member>(entityType));
		SerializationResponse serializationResponse = new SerializationResponse();
		serializationResponse.ResponseObject = new GetEntitiesResponse
		{
			ConfigurationVersion = repository.RepositoryVersion
		};
		serializationResponse.ResponseBody = stringCollection;
		return serializationResponse;
	}

	private StringCollection GetSerializedEntity<T>(EntityType entityType) where T : Entity
	{
		if (!IsEntityTypeCompatible<T>(entityType))
		{
			return configurationPersistence.GetEmptyList<T>();
		}
		return configurationPersistence.LoadSerialized<T>();
	}

	private bool IsEntityTypeCompatible<T>(EntityType entityType)
	{
		Type typeFromHandle = typeof(T);
		switch (entityType)
		{
		case EntityType.BaseDevice:
			return (object)typeFromHandle == typeof(BaseDevice);
		case EntityType.LogicalDevice:
			return (object)typeFromHandle == typeof(LogicalDevice);
		case EntityType.Location:
			return (object)typeFromHandle == typeof(Location);
		case EntityType.Interaction:
			return (object)typeFromHandle == typeof(Interaction);
		case EntityType.HomeSetup:
			return (object)typeFromHandle == typeof(HomeSetup);
		case EntityType.Home:
			return false;
		case EntityType.Member:
			return false;
		case EntityType.Configuration:
			if ((object)typeFromHandle != typeof(Home))
			{
				return (object)typeFromHandle != typeof(Member);
			}
			return false;
		default:
			return false;
		}
	}
}
