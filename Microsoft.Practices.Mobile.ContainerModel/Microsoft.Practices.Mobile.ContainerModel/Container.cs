using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.Mobile.ContainerModel.Properties;

namespace Microsoft.Practices.Mobile.ContainerModel;

public sealed class Container : IDisposable
{
	private Dictionary<ServiceKey, ServiceEntry> services = new Dictionary<ServiceKey, ServiceEntry>();

	private Stack<WeakReference> disposables = new Stack<WeakReference>();

	private Stack<Container> childContainers = new Stack<Container>();

	private Container parent;

	public Owner DefaultOwner { get; set; }

	public ReuseScope DefaultReuse { get; set; }

	[DebuggerStepThrough]
	public Func<TService> LazyResolve<TService>()
	{
		return LazyResolve<TService>(null);
	}

	[DebuggerStepThrough]
	public Func<TArg, TService> LazyResolve<TService, TArg>()
	{
		return LazyResolve<TService, TArg>(null);
	}

	[DebuggerStepThrough]
	public Func<TArg1, TArg2, TService> LazyResolve<TService, TArg1, TArg2>()
	{
		return LazyResolve<TService, TArg1, TArg2>(null);
	}

	[DebuggerStepThrough]
	public Func<TArg1, TArg2, TArg3, TService> LazyResolve<TService, TArg1, TArg2, TArg3>()
	{
		return LazyResolve<TService, TArg1, TArg2, TArg3>(null);
	}

	[DebuggerStepThrough]
	public Func<TArg1, TArg2, TArg3, TArg4, TService> LazyResolve<TService, TArg1, TArg2, TArg3, TArg4>()
	{
		return LazyResolve<TService, TArg1, TArg2, TArg3, TArg4>(null);
	}

	[DebuggerStepThrough]
	public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TService> LazyResolve<TService, TArg1, TArg2, TArg3, TArg4, TArg5>()
	{
		return LazyResolve<TService, TArg1, TArg2, TArg3, TArg4, TArg5>(null);
	}

	[DebuggerStepThrough]
	public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TService> LazyResolve<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>()
	{
		return LazyResolve<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(null);
	}

	[DebuggerStepThrough]
	public Func<TService> LazyResolve<TService>(string name)
	{
		ThrowIfNotRegistered<TService, Func<Container, TService>>(name);
		return () => ResolveNamed<TService>(name);
	}

	[DebuggerStepThrough]
	public Func<TArg, TService> LazyResolve<TService, TArg>(string name)
	{
		ThrowIfNotRegistered<TService, Func<Container, TArg, TService>>(name);
		return (TArg arg) => ResolveNamed<TService, TArg>(name, arg);
	}

	[DebuggerStepThrough]
	public Func<TArg1, TArg2, TService> LazyResolve<TService, TArg1, TArg2>(string name)
	{
		ThrowIfNotRegistered<TService, Func<Container, TArg1, TArg2, TService>>(name);
		return (TArg1 arg1, TArg2 arg2) => ResolveNamed<TService, TArg1, TArg2>(name, arg1, arg2);
	}

	[DebuggerStepThrough]
	public Func<TArg1, TArg2, TArg3, TService> LazyResolve<TService, TArg1, TArg2, TArg3>(string name)
	{
		ThrowIfNotRegistered<TService, Func<Container, TArg1, TArg2, TArg3, TService>>(name);
		return (TArg1 arg1, TArg2 arg2, TArg3 arg3) => ResolveNamed<TService, TArg1, TArg2, TArg3>(name, arg1, arg2, arg3);
	}

	[DebuggerStepThrough]
	public Func<TArg1, TArg2, TArg3, TArg4, TService> LazyResolve<TService, TArg1, TArg2, TArg3, TArg4>(string name)
	{
		ThrowIfNotRegistered<TService, Func<Container, TArg1, TArg2, TArg3, TArg4, TService>>(name);
		return (TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4) => ResolveNamed<TService, TArg1, TArg2, TArg3, TArg4>(name, arg1, arg2, arg3, arg4);
	}

	[DebuggerStepThrough]
	public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TService> LazyResolve<TService, TArg1, TArg2, TArg3, TArg4, TArg5>(string name)
	{
		ThrowIfNotRegistered<TService, Func<Container, TArg1, TArg2, TArg3, TArg4, TArg5, TService>>(name);
		return (TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5) => ResolveNamed<TService, TArg1, TArg2, TArg3, TArg4, TArg5>(name, arg1, arg2, arg3, arg4, arg5);
	}

	[DebuggerStepThrough]
	public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TService> LazyResolve<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(string name)
	{
		ThrowIfNotRegistered<TService, Func<Container, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TService>>(name);
		return (TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6) => ResolveNamed<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(name, arg1, arg2, arg3, arg4, arg5, arg6);
	}

	[DebuggerStepThrough]
	public IRegistration<TService> Register<TService>(Func<Container, TService> factory)
	{
		return Register(null, factory);
	}

	[DebuggerStepThrough]
	public IRegistration<TService> Register<TService, TArg>(Func<Container, TArg, TService> factory)
	{
		return Register(null, factory);
	}

	[DebuggerStepThrough]
	public IRegistration<TService> Register<TService, TArg1, TArg2>(Func<Container, TArg1, TArg2, TService> factory)
	{
		return Register(null, factory);
	}

	[DebuggerStepThrough]
	public IRegistration<TService> Register<TService, TArg1, TArg2, TArg3>(Func<Container, TArg1, TArg2, TArg3, TService> factory)
	{
		return Register(null, factory);
	}

	[DebuggerStepThrough]
	public IRegistration<TService> Register<TService, TArg1, TArg2, TArg3, TArg4>(Func<Container, TArg1, TArg2, TArg3, TArg4, TService> factory)
	{
		return Register(null, factory);
	}

	[DebuggerStepThrough]
	public IRegistration<TService> Register<TService, TArg1, TArg2, TArg3, TArg4, TArg5>(Func<Container, TArg1, TArg2, TArg3, TArg4, TArg5, TService> factory)
	{
		return Register(null, factory);
	}

	[DebuggerStepThrough]
	public IRegistration<TService> Register<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Func<Container, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TService> factory)
	{
		return Register(null, factory);
	}

	[DebuggerStepThrough]
	public IRegistration<TService> Register<TService>(string name, Func<Container, TService> factory)
	{
		return RegisterImpl<TService, Func<Container, TService>>(name, factory);
	}

	[DebuggerStepThrough]
	public IRegistration<TService> Register<TService, TArg>(string name, Func<Container, TArg, TService> factory)
	{
		return RegisterImpl<TService, Func<Container, TArg, TService>>(name, factory);
	}

	[DebuggerStepThrough]
	public IRegistration<TService> Register<TService, TArg1, TArg2>(string name, Func<Container, TArg1, TArg2, TService> factory)
	{
		return RegisterImpl<TService, Func<Container, TArg1, TArg2, TService>>(name, factory);
	}

	[DebuggerStepThrough]
	public IRegistration<TService> Register<TService, TArg1, TArg2, TArg3>(string name, Func<Container, TArg1, TArg2, TArg3, TService> factory)
	{
		return RegisterImpl<TService, Func<Container, TArg1, TArg2, TArg3, TService>>(name, factory);
	}

	[DebuggerStepThrough]
	public IRegistration<TService> Register<TService, TArg1, TArg2, TArg3, TArg4>(string name, Func<Container, TArg1, TArg2, TArg3, TArg4, TService> factory)
	{
		return RegisterImpl<TService, Func<Container, TArg1, TArg2, TArg3, TArg4, TService>>(name, factory);
	}

	[DebuggerStepThrough]
	public IRegistration<TService> Register<TService, TArg1, TArg2, TArg3, TArg4, TArg5>(string name, Func<Container, TArg1, TArg2, TArg3, TArg4, TArg5, TService> factory)
	{
		return RegisterImpl<TService, Func<Container, TArg1, TArg2, TArg3, TArg4, TArg5, TService>>(name, factory);
	}

	[DebuggerStepThrough]
	public IRegistration<TService> Register<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(string name, Func<Container, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TService> factory)
	{
		return RegisterImpl<TService, Func<Container, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TService>>(name, factory);
	}

	[DebuggerStepThrough]
	public TService Resolve<TService>()
	{
		return ResolveNamed<TService>(null);
	}

	[DebuggerStepThrough]
	public TService Resolve<TService, TArg>(TArg arg)
	{
		return ResolveNamed<TService, TArg>(null, arg);
	}

	[DebuggerStepThrough]
	public TService Resolve<TService, TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
	{
		return ResolveNamed<TService, TArg1, TArg2>(null, arg1, arg2);
	}

	[DebuggerStepThrough]
	public TService Resolve<TService, TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		return ResolveNamed<TService, TArg1, TArg2, TArg3>(null, arg1, arg2, arg3);
	}

	[DebuggerStepThrough]
	public TService Resolve<TService, TArg1, TArg2, TArg3, TArg4>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
	{
		return ResolveNamed<TService, TArg1, TArg2, TArg3, TArg4>(null, arg1, arg2, arg3, arg4);
	}

	[DebuggerStepThrough]
	public TService Resolve<TService, TArg1, TArg2, TArg3, TArg4, TArg5>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
	{
		return ResolveNamed<TService, TArg1, TArg2, TArg3, TArg4, TArg5>(null, arg1, arg2, arg3, arg4, arg5);
	}

	[DebuggerStepThrough]
	public TService Resolve<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
	{
		return ResolveNamed<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(null, arg1, arg2, arg3, arg4, arg5, arg6);
	}

	[DebuggerStepThrough]
	public TService ResolveNamed<TService>(string name)
	{
		return ResolveImpl<TService>(name, throwIfMissing: true);
	}

	[DebuggerStepThrough]
	public TService ResolveNamed<TService, TArg>(string name, TArg arg)
	{
		return ResolveImpl<TService, TArg>(name, throwIfMissing: true, arg);
	}

	[DebuggerStepThrough]
	public TService ResolveNamed<TService, TArg1, TArg2>(string name, TArg1 arg1, TArg2 arg2)
	{
		return ResolveImpl<TService, TArg1, TArg2>(name, throwIfMissing: true, arg1, arg2);
	}

	[DebuggerStepThrough]
	public TService ResolveNamed<TService, TArg1, TArg2, TArg3>(string name, TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		return ResolveImpl<TService, TArg1, TArg2, TArg3>(name, throwIfMissing: true, arg1, arg2, arg3);
	}

	[DebuggerStepThrough]
	public TService ResolveNamed<TService, TArg1, TArg2, TArg3, TArg4>(string name, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
	{
		return ResolveImpl<TService, TArg1, TArg2, TArg3, TArg4>(name, throwIfMissing: true, arg1, arg2, arg3, arg4);
	}

	[DebuggerStepThrough]
	public TService ResolveNamed<TService, TArg1, TArg2, TArg3, TArg4, TArg5>(string name, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
	{
		return ResolveImpl<TService, TArg1, TArg2, TArg3, TArg4, TArg5>(name, throwIfMissing: true, arg1, arg2, arg3, arg4, arg5);
	}

	[DebuggerStepThrough]
	public TService ResolveNamed<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(string name, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
	{
		return ResolveImpl<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(name, throwIfMissing: true, arg1, arg2, arg3, arg4, arg5, arg6);
	}

	[DebuggerStepThrough]
	public TService TryResolve<TService>()
	{
		return TryResolveNamed<TService>(null);
	}

	[DebuggerStepThrough]
	public TService TryResolve<TService, TArg>(TArg arg)
	{
		return TryResolveNamed<TService, TArg>(null, arg);
	}

	[DebuggerStepThrough]
	public TService TryResolve<TService, TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
	{
		return TryResolveNamed<TService, TArg1, TArg2>(null, arg1, arg2);
	}

	[DebuggerStepThrough]
	public TService TryResolve<TService, TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		return TryResolveNamed<TService, TArg1, TArg2, TArg3>(null, arg1, arg2, arg3);
	}

	[DebuggerStepThrough]
	public TService TryResolve<TService, TArg1, TArg2, TArg3, TArg4>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
	{
		return TryResolveNamed<TService, TArg1, TArg2, TArg3, TArg4>(null, arg1, arg2, arg3, arg4);
	}

	[DebuggerStepThrough]
	public TService TryResolve<TService, TArg1, TArg2, TArg3, TArg4, TArg5>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
	{
		return TryResolveNamed<TService, TArg1, TArg2, TArg3, TArg4, TArg5>(null, arg1, arg2, arg3, arg4, arg5);
	}

	[DebuggerStepThrough]
	public TService TryResolve<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
	{
		return TryResolveNamed<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(null, arg1, arg2, arg3, arg4, arg5, arg6);
	}

	[DebuggerStepThrough]
	public TService TryResolveNamed<TService>(string name)
	{
		return ResolveImpl<TService>(name, throwIfMissing: false);
	}

	[DebuggerStepThrough]
	public TService TryResolveNamed<TService, TArg>(string name, TArg arg)
	{
		return ResolveImpl<TService, TArg>(name, throwIfMissing: false, arg);
	}

	[DebuggerStepThrough]
	public TService TryResolveNamed<TService, TArg1, TArg2>(string name, TArg1 arg1, TArg2 arg2)
	{
		return ResolveImpl<TService, TArg1, TArg2>(name, throwIfMissing: false, arg1, arg2);
	}

	[DebuggerStepThrough]
	public TService TryResolveNamed<TService, TArg1, TArg2, TArg3>(string name, TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		return ResolveImpl<TService, TArg1, TArg2, TArg3>(name, throwIfMissing: false, arg1, arg2, arg3);
	}

	[DebuggerStepThrough]
	public TService TryResolveNamed<TService, TArg1, TArg2, TArg3, TArg4>(string name, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
	{
		return ResolveImpl<TService, TArg1, TArg2, TArg3, TArg4>(name, throwIfMissing: false, arg1, arg2, arg3, arg4);
	}

	[DebuggerStepThrough]
	public TService TryResolveNamed<TService, TArg1, TArg2, TArg3, TArg4, TArg5>(string name, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
	{
		return ResolveImpl<TService, TArg1, TArg2, TArg3, TArg4, TArg5>(name, throwIfMissing: false, arg1, arg2, arg3, arg4, arg5);
	}

	[DebuggerStepThrough]
	public TService TryResolveNamed<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(string name, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
	{
		return ResolveImpl<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(name, throwIfMissing: false, arg1, arg2, arg3, arg4, arg5, arg6);
	}

	public Container()
	{
		services[new ServiceKey(typeof(Func<Container, Container>), null)] = new ServiceEntry<Container, Func<Container, Container>>((Container c) => c)
		{
			Container = this,
			Instance = this,
			Owner = Owner.External,
			Reuse = ReuseScope.Container
		};
	}

	public Container CreateChildContainer()
	{
		Container container = new Container();
		container.parent = this;
		Container container2 = container;
		childContainers.Push(container2);
		return container2;
	}

	public void Dispose()
	{
		while (disposables.Count > 0)
		{
			WeakReference weakReference = disposables.Pop();
			IDisposable disposable = (IDisposable)weakReference.Target;
			if (weakReference.IsAlive)
			{
				disposable.Dispose();
			}
		}
		while (childContainers.Count > 0)
		{
			childContainers.Pop().Dispose();
		}
	}

	public void Register<TService>(TService instance)
	{
		Register(null, instance);
	}

	public void Register<TService>(string name, TService instance)
	{
		ServiceEntry<TService, Func<Container, TService>> serviceEntry = RegisterImpl<TService, Func<Container, TService>>(name, null);
		serviceEntry.ReusedWithin(ReuseScope.Hierarchy).OwnedBy(Owner.External);
		serviceEntry.InitializeInstance(instance);
	}

	private ServiceEntry<TService, TFunc> RegisterImpl<TService, TFunc>(string name, TFunc factory)
	{
		if ((object)typeof(TService) == typeof(Container))
		{
			throw new ArgumentException(Resources.Registration_CantRegisterContainer);
		}
		ServiceEntry<TService, TFunc> serviceEntry = new ServiceEntry<TService, TFunc>(factory);
		serviceEntry.Container = this;
		serviceEntry.Reuse = DefaultReuse;
		serviceEntry.Owner = DefaultOwner;
		ServiceEntry<TService, TFunc> serviceEntry2 = serviceEntry;
		ServiceKey key = new ServiceKey(typeof(TFunc), name);
		services[key] = serviceEntry2;
		return serviceEntry2;
	}

	private TService ResolveImpl<TService>(string name, bool throwIfMissing)
	{
		ServiceEntry<TService, Func<Container, TService>> entry = GetEntry<TService, Func<Container, TService>>(name, throwIfMissing);
		if (entry == null)
		{
			return default(TService);
		}
		TService val = entry.Instance;
		if (val == null)
		{
			val = entry.Factory(entry.Container);
			entry.InitializeInstance(val);
		}
		return val;
	}

	private TService ResolveImpl<TService, TArg>(string name, bool throwIfMissing, TArg arg)
	{
		ServiceEntry<TService, Func<Container, TArg, TService>> entry = GetEntry<TService, Func<Container, TArg, TService>>(name, throwIfMissing);
		if (entry == null)
		{
			return default(TService);
		}
		TService val = entry.Instance;
		if (val == null)
		{
			val = entry.Factory(entry.Container, arg);
			entry.InitializeInstance(val);
		}
		return val;
	}

	private TService ResolveImpl<TService, TArg1, TArg2>(string name, bool throwIfMissing, TArg1 arg1, TArg2 arg2)
	{
		ServiceEntry<TService, Func<Container, TArg1, TArg2, TService>> entry = GetEntry<TService, Func<Container, TArg1, TArg2, TService>>(name, throwIfMissing);
		if (entry == null)
		{
			return default(TService);
		}
		TService val = entry.Instance;
		if (val == null)
		{
			val = entry.Factory(entry.Container, arg1, arg2);
			entry.InitializeInstance(val);
		}
		return val;
	}

	private TService ResolveImpl<TService, TArg1, TArg2, TArg3>(string name, bool throwIfMissing, TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		ServiceEntry<TService, Func<Container, TArg1, TArg2, TArg3, TService>> entry = GetEntry<TService, Func<Container, TArg1, TArg2, TArg3, TService>>(name, throwIfMissing);
		if (entry == null)
		{
			return default(TService);
		}
		TService val = entry.Instance;
		if (val == null)
		{
			val = entry.Factory(entry.Container, arg1, arg2, arg3);
			entry.InitializeInstance(val);
		}
		return val;
	}

	private TService ResolveImpl<TService, TArg1, TArg2, TArg3, TArg4>(string name, bool throwIfMissing, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
	{
		ServiceEntry<TService, Func<Container, TArg1, TArg2, TArg3, TArg4, TService>> entry = GetEntry<TService, Func<Container, TArg1, TArg2, TArg3, TArg4, TService>>(name, throwIfMissing);
		if (entry == null)
		{
			return default(TService);
		}
		TService val = entry.Instance;
		if (val == null)
		{
			val = entry.Factory(entry.Container, arg1, arg2, arg3, arg4);
			entry.InitializeInstance(val);
		}
		return val;
	}

	private TService ResolveImpl<TService, TArg1, TArg2, TArg3, TArg4, TArg5>(string name, bool throwIfMissing, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
	{
		ServiceEntry<TService, Func<Container, TArg1, TArg2, TArg3, TArg4, TArg5, TService>> entry = GetEntry<TService, Func<Container, TArg1, TArg2, TArg3, TArg4, TArg5, TService>>(name, throwIfMissing);
		if (entry == null)
		{
			return default(TService);
		}
		TService val = entry.Instance;
		if (val == null)
		{
			val = entry.Factory(entry.Container, arg1, arg2, arg3, arg4, arg5);
			entry.InitializeInstance(val);
		}
		return val;
	}

	private TService ResolveImpl<TService, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(string name, bool throwIfMissing, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
	{
		ServiceEntry<TService, Func<Container, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TService>> entry = GetEntry<TService, Func<Container, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TService>>(name, throwIfMissing);
		if (entry == null)
		{
			return default(TService);
		}
		TService val = entry.Instance;
		if (val == null)
		{
			val = entry.Factory(entry.Container, arg1, arg2, arg3, arg4, arg5, arg6);
			entry.InitializeInstance(val);
		}
		return val;
	}

	internal void TrackDisposable(object instance)
	{
		disposables.Push(new WeakReference(instance));
	}

	private ServiceEntry<TService, TFunc> GetEntry<TService, TFunc>(string serviceName, bool throwIfMissing)
	{
		ServiceKey key = new ServiceKey(typeof(TFunc), serviceName);
		ServiceEntry value = null;
		Container container = this;
		while (!container.services.TryGetValue(key, out value) && container.parent != null)
		{
			container = container.parent;
		}
		if (value != null)
		{
			if (value.Reuse == ReuseScope.Container && value.Container != this)
			{
				value = ((ServiceEntry<TService, TFunc>)value).CloneFor(this);
				services[key] = value;
			}
		}
		else if (throwIfMissing)
		{
			ThrowMissing<TService>(serviceName);
		}
		return (ServiceEntry<TService, TFunc>)value;
	}

	private static TService ThrowMissing<TService>(string serviceName)
	{
		if (serviceName == null)
		{
			throw new ResolutionException(typeof(TService));
		}
		throw new ResolutionException(typeof(TService), serviceName);
	}

	private void ThrowIfNotRegistered<TService, TFunc>(string name)
	{
		GetEntry<TService, TFunc>(name, throwIfMissing: true);
	}
}
