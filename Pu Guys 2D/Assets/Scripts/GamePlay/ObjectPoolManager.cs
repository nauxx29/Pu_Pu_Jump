/* System.Collections.Generic;
using UnityEngine;

// For static use
public class ObjectPool
{
    public static Dictionary<StairType, Queue<Component>> poolDict = new Dictionary<StairType, Queue<Component>>();
    public static List<Component> DequeueList = new List<Component>();

    public static void SetupPool<T> (T pooledItemPrefab, int poolSize, StairType key) where T : Component
    {
        if (poolDict.ContainsKey(key))
        {
            return;
        }
            
        poolDict.Add(key, new Queue<Component>());

        for (int i = 0; i < poolSize; i++)
        {
            T pooledInstance = Object.Instantiate(pooledItemPrefab);
            pooledInstance.gameObject.SetActive(false);
            poolDict[key].Enqueue((T)pooledInstance);
        }
    }

    public static void ReturnToPool<T> (T item, StairType key) where T : Component
    {
        if (!item.gameObject.activeSelf)
        {
            return;
        }

        poolDict[key].Enqueue(item);
        DequeueList.Remove(item);
        item.gameObject.SetActive(false);
    }

    public static T FetchFromPool<T> (StairType key) where T : Component
    {
        Component dequeueComponent =  (T)poolDict[key].Dequeue();
        DequeueList.Add(dequeueComponent);
        return (T)dequeueComponent;
    }

    public static void ResetPool<T> () where T : Component
    {
        foreach (T item in DequeueList)
        {
            item.gameObject.SetActive (false);
        }
    }
}
*/



