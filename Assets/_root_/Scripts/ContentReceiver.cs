using UnityEngine;


/// <summary>
/// The EntryReceiver is an abstract class for
/// receiving data from the EntryNode.
/// </summary>
/// <remarks>Author: Alexander Larsen.</remarks>
public abstract class ContentReceiver<T> : MonoBehaviour
{
    public abstract void ReceiveContent(T content);

    public abstract void HideContent(float hideDelay);

}
