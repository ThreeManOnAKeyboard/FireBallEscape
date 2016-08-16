﻿using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PooledObject
{
	public GameObject objectToPool;
	public int pooledAmount = 20;
}

public class ObjectPool : MonoBehaviour
{
	public static ObjectPool Instance;

	public PooledObject[] pooledObjectTypes;

	private Dictionary<string, List<GameObject>> pool;

	void Awake()
	{
		// Set up singleton reference
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Start()
	{
		// Set up the pool and instantiate all initial objects
		pool = new Dictionary<string, List<GameObject>>();
		PooledObject pooledObjectType;
		for (int i = 0; i < pooledObjectTypes.Length; i++)
		{
			pooledObjectType = pooledObjectTypes[i];
			pool.Add(pooledObjectType.objectToPool.name, new List<GameObject>(pooledObjectType.pooledAmount));
			for (int j = 0; j < pooledObjectType.pooledAmount; j++)
			{
				AddPooledObject(pooledObjectType.objectToPool);
			}
		}
	}

	private GameObject AddPooledObject(GameObject newObject)
	{
		GameObject pooledObject = Instantiate(newObject);
		pooledObject.SetActive(false);
		pool[newObject.name].Add(pooledObject);
		return pooledObject;
	}

	public GameObject GetPooledObject(GameObject gameObject)
	{
		List<GameObject> pooledObjects = pool[gameObject.name];

		// Return first available inactive pooled GameObject
		for (int i = 0; i < pooledObjects.Count; i++)
		{
			if (!pooledObjects[i].activeInHierarchy)
			{
				return pooledObjects[i];
			}
		}

		return AddPooledObject(gameObject);
	}
}