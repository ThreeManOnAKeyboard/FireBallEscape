using System.Collections.Generic;
using UnityEngine;

namespace Level.Spawner
{
	[System.Serializable]
	public class PooledObject
	{
		public GameObject objectToPool;
		public int pooledAmount = 20;
		public bool growable = true;
	}

	public class ObjectPool : MonoBehaviour
	{
		public static ObjectPool instance;

		public PooledObject[] pooledObjectTypes;

		private GameObject pooledObjectsParent;
		private Dictionary<string, List<GameObject>> pool;
		private List<GameObject> pooledObjects;

		private void Awake()
		{
			// Set up singleton reference
			if (instance == null)
			{
				instance = this;
			}
		}

		private void Start()
		{
			// Create the pooling parent
			pooledObjectsParent = new GameObject("PooledGameObjects");

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
			pooledObject.transform.parent = pooledObjectsParent.transform;
			pooledObject.transform.position = Vector3.zero;
			return pooledObject;
		}

		public GameObject GetPooledObject(GameObject gameObject)
		{
			pooledObjects = pool[gameObject.name];

			// Return first available inactive pooled GameObject
			for (int i = 0; i < pooledObjects.Count; i++)
			{
				if (!pooledObjects[i].activeInHierarchy)
				{
					return pooledObjects[i];
				}
			}

			// Check if gameobject can grow in amount
			for (int i = 0; i < pooledObjectTypes.Length; i++)
			{
				if (gameObject.name == pooledObjectTypes[i].objectToPool.name)
				{
					if (pooledObjectTypes[i].growable)
					{
						return AddPooledObject(gameObject);
					}
					else
					{
						return null;
					}
				}
			}

			return null;
		}

		public List<GameObject> GetPooledList(GameObject gameObject)
		{
			return pool[gameObject.name];
		}

		public List<GameObject> GetPooledLists(GameObject[] gameObjects)
		{
			List<GameObject> newList = new List<GameObject>();

			for (int i = 0; i < gameObjects.Length; i++)
			{
				newList.AddRange(pool[gameObjects[i].name]);
			}

			return newList;
		}
	}
}