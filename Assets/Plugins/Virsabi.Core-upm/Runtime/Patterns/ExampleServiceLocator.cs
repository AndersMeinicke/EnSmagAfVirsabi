using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example of a service locator - every project using this pattern needs this for injecting the dependencies.
/// </summary>
public class ExampleServiceLocator : Virsabi.Patterns.ServiceLocatorBase
{
	internal ExampleServiceLocator() : base()
	{
		//in this hypothetical example project we want a service requests of interface IServiceA to be controlled by the service 'ServiceAType2' instead of 'ServiceAType1'
		//this could be an audio system for example.
		AddService(typeof(IServiceA), new ServiceAType2());
		//other projects might prefer using 'ServiceAType1' for some platform specific reason - or ServiceAType2 may be an improved version over type1, but may lag still used deprecated methods that other projects might need.

		AddService(typeof(IServiceB), new ServiceB());

		//In case the service is a MonoBehaviour, we can use the MonoServiceLocator, which finds or creates the component in the scene on first initialization/call.
		AddService(typeof(IServiceMono), new MonoServiceExample());
	}

	#region Example interface services

	//interfaces that can be defined by services
	public interface IServiceA
	{
		int a { get; }
	}

	public interface IServiceB
	{
		string b { get; }
	}

	public interface IServiceMono
	{
		void Method();
	}

	#endregion

	#region Classes that can respond to a specific interface request
	public class ServiceAType1 : IServiceA
	{
		public int a { get; private set; }
	}
	public class ServiceAType2 : IServiceA
	{
		public int a { get; private set; }
	}
	public class ServiceB : IServiceB
	{
		public string b { get; private set; }
	}

    public class MonoServiceExample : MonoBehaviour, IServiceMono
    {
        public void Method() => Debug.Log("A monobehavior service was called");
    }
    #endregion
}


