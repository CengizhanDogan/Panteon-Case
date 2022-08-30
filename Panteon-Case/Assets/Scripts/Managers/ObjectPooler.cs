using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class ObjectPooler : MonoBehaviour
{
    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject gameObject;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public static ObjectPooler Instance { get; private set; }
    private void Awake()
    {
        #region Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        #endregion

        CreatePools();
    }

    private void CreatePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            UnitObject unitObject = UnitObjectManager.GetUnit(pool.tag);

            if (unitObject)
            {
                ObjectCreator.CreateObjects(unitObject, out var unitGameObject);
                if (unitObject as BuildingObject) ObjectCreator.CreateMover(unitObject, out unitGameObject);

                unitGameObject.SetActive(false);

                pool.gameObject = unitGameObject;
                objectPool.Enqueue(unitGameObject);

                unitGameObject.transform.SetParent(transform);
            }

            for (int i = 1; i < pool.size + 1; i++)
            {
                GameObject obj = Instantiate(pool.gameObject, transform);
                objectPool.Enqueue(obj);
                obj.SetActive(false);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    public void DestroyPoolObject(GameObject destroyObject)
    {
        destroyObject.SetActive(false);
    }
}
