using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand = true;
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler sharedInstance;

    [SerializeField] private List<GameObject> pooledObjects;
    [SerializeField] private List<ObjectPoolItem> itemsToPool;
    
    private void Awake()
    {
        sharedInstance = this;
    }

	private void OnEnable()
	{
        InitPool();
	}

    private void InitPool()
	{
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = Instantiate(item.objectToPool, gameObject.transform, true);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string objectTag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].CompareTag(objectTag))
                return pooledObjects[i];
        }

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.CompareTag(objectTag))
            {
                if (item.shouldExpand)
                {
                    GameObject obj = Instantiate(item.objectToPool, gameObject.transform, true);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);

                    return obj;
                }
            }
        }

        return null;
    }
}
