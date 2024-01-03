using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDBConnection
{
    /// <summary>
    /// Post a single item to a database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item"></param>
    void PostItem<T>(T item, DB_Responses.OnGenericResult result);

    /// <summary>
    /// Get a single item from a database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item"></param>
    void GetItem<T>(T item, DB_Responses.OnGenericResult result);

    /// <summary>
    /// Get the full table as generic array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    void GetTableAsArray<T>(DB_Responses.OnArrayReturn<T> arrayResult, DB_Responses.OnGenericResult result);

    /// <summary>
    /// Get the full table as generic list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="listResult"></param>
    void GetTableAsList<T>(DB_Responses.OnListReturn<T> listResult, DB_Responses.OnGenericResult result);
}

public class DB_Responses
{

    public delegate void OnArrayReturn<T>(T[] arrayResult);

    public delegate void OnListReturn<T>(List<T> listResult);

    public delegate void OnGenericResult(bool successFull, string response);
}
