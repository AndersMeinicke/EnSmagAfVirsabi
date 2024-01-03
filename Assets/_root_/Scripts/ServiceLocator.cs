using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Virsabi;
using Virsabi.Patterns;

public class ServiceLocator : ServiceLocatorBase
{
    private static ServiceLocator _instance;
    public static ServiceLocator Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ServiceLocator();

            return _instance;
        }
    }

    internal ServiceLocator() : base()
    {
        //Inject our AzureTableStorageConnection as our Database Connection
        AddService(typeof(IDBConnection), MonoServiceLocator.GetMonoService<AzureTableStorageConnection>());
    }
}
