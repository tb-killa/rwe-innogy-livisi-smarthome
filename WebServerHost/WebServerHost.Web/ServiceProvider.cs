using System;
using System.Collections.Generic;
using System.Reflection;
using SmartHome.Common.Generic.LogManager;
using WebServerHost.Web.Extensions;

namespace WebServerHost.Web;

public class ServiceProvider
{
	private static ServiceProvider instance;

	private Dictionary<Type, object> singletonServices;

	private Dictionary<Type, Type> serives;

	private ILogger logger = LogManager.Instance.GetLogger<ServiceProvider>();

	public static ServiceProvider Services
	{
		get
		{
			if (instance == null)
			{
				instance = new ServiceProvider();
			}
			return instance;
		}
	}

	private ServiceProvider()
	{
		singletonServices = new Dictionary<Type, object>();
		serives = new Dictionary<Type, Type>();
	}

	public void Add<TService>() where TService : class
	{
		Add<TService, TService>();
	}

	public void Add<TService, TImplementation>() where TService : class where TImplementation : class, TService
	{
		serives.Add(typeof(TService), typeof(TImplementation));
	}

	public void AddSingleton<TService>(TService implementationInstance) where TService : class
	{
		AddSingleton<TService, TService>(implementationInstance);
	}

	public void AddSingleton<TService, TImplementation>(TImplementation implementrationInstance) where TService : class where TImplementation : class, TService
	{
		singletonServices.Add(typeof(TService), implementrationInstance);
	}

	public TService Get<TService>() where TService : class
	{
		return (TService)Get(typeof(TService));
	}

	public object Get(Type serviceType)
	{
		if (singletonServices.ContainsKey(serviceType))
		{
			return singletonServices[serviceType];
		}
		if (serives.ContainsKey(serviceType))
		{
			return CreateInstance(serives[serviceType]);
		}
		return serviceType.GetDafaultValue();
	}

	private object CreateInstance(Type type)
	{
		try
		{
			ConstructorInfo[] constructors = type.GetConstructors();
			if (constructors.Length == 1)
			{
				ConstructorInfo constructorInfo = constructors[0];
				ParameterInfo[] parameters = constructorInfo.GetParameters();
				object[] array = new object[parameters.Length];
				for (int i = 0; i < parameters.Length; i++)
				{
					array[i] = Get(parameters[i].ParameterType);
				}
				return constructorInfo.Invoke(array);
			}
		}
		catch (Exception exception)
		{
			logger.Warn($"Cannot create instance of type {type}", exception);
		}
		return type.GetDafaultValue();
	}
}
