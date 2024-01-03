using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Virsabi.Patterns
{
	/// <summary>
	/// Extend this class to make a project specific service locator.
	/// Use AddServices to inject your service references.
	/// </summary>
	public abstract class ServiceLocatorBase : IServiceLocator
	{
		private IDictionary<object, object> services;

		protected ServiceLocatorBase()
		{
			services = new Dictionary<object, object>();
		}

		public T GetService<T>()
		{
			try
			{
				return (T)services[typeof(T)];
			}
			catch (KeyNotFoundException)
			{
				throw new UnityException("The requested service is not registered");
			}
		}

		protected void AddService(object type, object service)
		{
			services.Add(type, service);
		}
	}
}
