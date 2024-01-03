using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using Proyecto26;
using System;
using Virsabi;

/// <summary>
/// This service allows easy connection to a table database on Azure.
/// </summary>
public class AzureTableStorageConnection : MonoBehaviour, IDBConnection
{
    [SerializeField, DisplayInspector]
    private AzureTableConnectionSettings connectionSettings;

    [Foldout("Debug", true)]
    [SerializeField]
    private List<TestTopScoreEntry> topScores = new List<TestTopScoreEntry>();

    [SerializeField]
    private bool DebugLog = false;


    private void Start()
    {
        //Sets header to Request the response in json format instead of default xml
        RestClient.DefaultRequestHeaders["Accept"] = "application/json;odata=nometadata";
    }

    [SerializeField, HideInInspector]
    private string SasKey, URI;

    private void OnValidate()
    {
        SasKey = connectionSettings.SasKey;
        URI = connectionSettings.Uri;
    }

    #region ButtonMethodTests
    [ButtonMethod]
    private void TestGetTable()
    {
        UpdateListFromTable(topScores);
    }

    [ButtonMethod]
    private void TestPostToTable()
    {
        TestTopScoreEntry test = new TestTopScoreEntry("Frank", 89.125f,"Office",100);

        PostToTable(test);
    }
    #endregion

    #region Public Methods


    /// <summary>
    /// Posts a single Serializable object to a table - no need to upload a whole table when posting.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item"></param>
    /// <param name="result">Callback; bool successfull, string response/errorMSG</param>
    public void PostItem<T>(T item, DB_Responses.OnGenericResult result)
    {
        string jsonStrem = JsonUtility.ToJson(item);

        if (DebugLog)
            Debug.Log(jsonStrem);

        if (DebugLog)
            Debug.Log("Request: " + URI + SasKey);

        RequestHelper currentRequest = new RequestHelper
        {
            Uri = URI + SasKey,
            Timeout = connectionSettings.MaxSecondsUntilTimeout,
            Body = item,
            Retries = connectionSettings.Retries
        };

        RestClient.Post<T>(currentRequest).Then(response =>
        {
            if (DebugLog)
                Debug.Log("JSON : " + JsonUtility.ToJson(response, true));

            result(true, response.ToString());

        }).Catch(err =>
        {
            try
            {
                Debug.LogError("Error: " + err.Message);
                result(false, err.Message);
            }catch(Exception ex)
            {
                Debug.LogError(ex.Message + "\n" + ex.StackTrace);
            }
        });
    }

    /// <summary>
    /// Updates a List from a table, with callback.
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="listResult">Callback; bool successfull, string response/errorMSG, List - the list to be updated</param>
    public void GetTableAsList<T>(DB_Responses.OnListReturn<T> listResult, DB_Responses.OnGenericResult result)
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Only works in playmode");
            return;
        }

        if (DebugLog)
            Debug.Log("Request: " + URI + SasKey);

        RequestHelper currentRequest = new RequestHelper
        {
            Uri = URI + SasKey,
            Timeout = connectionSettings.MaxSecondsUntilTimeout,
            Retries = connectionSettings.Retries
        };

        RestClient.Get<ValueWrapper<T>>(currentRequest).Then(response =>
        {
            //Debug.LogWarning("response" + response.)
            if (DebugLog)
                Debug.Log("Response : " + JsonUtility.ToJson(response, true));
            if (DebugLog)
                Debug.Log("value : " + JsonUtility.ToJson(response.value, true));
            if (DebugLog)
                Debug.Log("value length : " + response.value.Length);
            
            try //need to apply try/catch for any callbacks
            {
                listResult(new List<T>(response.value));
                result(true, "Success");
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message + "\n" + ex.StackTrace);
            }
            
        }
        ).Catch(err =>
        {
            Debug.LogError("Error: " + err.Message);
            try //need to apply try/catch for any callbacks
            {
                result(false, err.Message); 
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message + "\n" + ex.StackTrace);
            }
            
        });

        Debug.LogWarning("finished");
    }

    /// <summary>
    /// Example of override without general result handling - not preferred - we want error handling on all applications.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="listResult"></param>
    private void GetTableAsList<T>(DB_Responses.OnListReturn<T> listResult)
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Only works in playmode");
            return;
        }

        if (DebugLog)
            Debug.Log("Request: " + URI + SasKey);

        RequestHelper currentRequest = new RequestHelper
        {
            Uri = URI + SasKey,
            Timeout = connectionSettings.MaxSecondsUntilTimeout,
            Retries = connectionSettings.Retries
        };

        RestClient.Get<ValueWrapper<T>>(currentRequest).Then(response =>
        {
            if (DebugLog)
                Debug.Log("Response : " + JsonUtility.ToJson(response, true));
            if (DebugLog)
                Debug.Log("value : " + JsonUtility.ToJson(response.value, true));
            if (DebugLog)
                Debug.Log("value length : " + response.value.Length);
            try
            {
                listResult(new List<T>(response.value));
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message + "\n" + ex.StackTrace);
            }
        }
        ).Catch(err =>
        {
            try
            {
                listResult(null);
                Debug.LogError("Error: " + err.Message);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message + "\n" + ex.StackTrace);
            }
        });
    }

    /// <summary>
    /// Get a generic Array from the table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arrayResult"></param>
    public void GetTableAsArray<T>(DB_Responses.OnArrayReturn<T> arrayResult, DB_Responses.OnGenericResult result)
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Only works in playmode");
            return;
        }

        if (DebugLog)
            Debug.Log("Request: " + URI + SasKey);

        RequestHelper currentRequest = new RequestHelper
        {
            Uri = URI + SasKey,
            Timeout = connectionSettings.MaxSecondsUntilTimeout,
            Retries = connectionSettings.Retries
        };

        RestClient.Get<ValueWrapper<T>>(currentRequest).Then(response =>
        {
            if (DebugLog)
                Debug.Log("Response : " + JsonUtility.ToJson(response, true));
            if (DebugLog)
                Debug.Log("value : " + JsonUtility.ToJson(response.value, true));
            if (DebugLog)
                Debug.Log("value length : " + response.value.Length);
            try
            {
                arrayResult(response.value);
                result(true, "Success");
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message + "\n" + ex.StackTrace);
            }
        }
        ).Catch(err =>
        {
            try
            {
                Debug.LogError("Error: " + err.Message);
                arrayResult(null);
                result(false, err.Message);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message + "\n" + ex.StackTrace);
            }
        });
    }


    /// <summary>
    /// Need method for filtering/finding item on table using SQL queries?
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item"></param>
    public void GetItem<T>(T item, DB_Responses.OnGenericResult result)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Deprecated

    /// <summary>
    /// Updates a List from a table. No callback.
    /// </summary>
    /// <typeparam name="T">The type of list</typeparam>
    /// <param name="objectList">reference to the list - will be dynamically updated after some time.</param>
    public void UpdateListFromTable<T>(List<T> objectList)
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Only works in playmode");
            return;
        }

        if (DebugLog)
            Debug.Log("Request: " + URI + SasKey);

        RequestHelper currentRequest = new RequestHelper
        {
            Uri = URI + SasKey,
            Timeout = connectionSettings.MaxSecondsUntilTimeout,
            Retries = connectionSettings.Retries
        };

        RestClient.Get<ValueWrapper<T>>(currentRequest).Then(response =>
        {
            if (DebugLog)
                Debug.Log("Response : " + JsonUtility.ToJson(response, true));
            if (DebugLog)
                Debug.Log("value : " + JsonUtility.ToJson(response.value, true));
            if (DebugLog)
                Debug.Log("value length : " + response.value.Length);
            Debug.Log("EHRE: " + response.value.ToString());
            objectList.Clear();
            try
            {
                objectList.AddRange(new List<T>(response.value));
            }
            catch (Exception ex)
            {
                Debug.Log(response.value);
                Debug.LogError(ex.Message + "\n" + ex.StackTrace);
            }

        }
        ).Catch(err => Debug.LogError("Error: " + err.Message));
    }

    /// <summary>
    /// Posts a single Serializable object to a table - no need to upload a whole table when posting.
    /// </summary>
    /// <typeparam name="T">The type of object</typeparam>
    /// <param name="item">the object to upload</param>
    public void PostToTable<T>(T item)
    {
        string jsonStrem = JsonUtility.ToJson(item);

        if (DebugLog)
            Debug.Log(jsonStrem);

        if (DebugLog)
            Debug.Log("Request: " + URI + SasKey);

        RequestHelper currentRequest = new RequestHelper
        {
            Uri = URI + SasKey,
            Timeout = connectionSettings.MaxSecondsUntilTimeout,
            Retries = connectionSettings.Retries,
            Body = item
        };

        RestClient.Post<T>(currentRequest).Then(response =>
        {
            if (DebugLog)
                Debug.Log("JSON : " + JsonUtility.ToJson(response, true));

        }).Catch(err => Debug.LogError("Error: " + err.Message));
    }

    #endregion


    #region Wrapper and Test Classes
    /// <summary>
    /// Wrapper for handling json formats with starts with {"value": [
    /// Commonly found in jsons converted from tables - like on azure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    private class ValueWrapper<T>
    {
        public T[] value;
    }

    /// <summary>
    /// Just a class for testing 
    /// </summary>
    [Serializable]
    public class TestTopScoreEntry : TableEntry
    {
        public string UserName;
        public string ChallengeID;
        public int Score;
        public float SecondsUsed;

        public TestTopScoreEntry(string userName, float secondsUsed, string challengeID, int score)
        {
            this.UserName = userName;
            this.SecondsUsed = secondsUsed;
            ChallengeID = challengeID;
            Score = score;
        }
    }

    public abstract class TableEntry : object
    {
        public string PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString();
    }
    #endregion

}

#region Service Setup
public abstract class TableConnection
{
    public abstract void PostToTable();
    public abstract void GetTable();
        
}

class ConsoleTableConnection : TableConnection
{
    public override void GetTable()
    {
        
    }

    public override void PostToTable()
    {
        
    }
}


class Locator
{
    private static TableConnection service_;

    public static TableConnection getTableConnection() => service_;

    static void provide(TableConnection service)
    {
        service_ = service;
    }
}
#endregion