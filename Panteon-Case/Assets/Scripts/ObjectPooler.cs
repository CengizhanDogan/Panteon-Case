using System;
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
        [HideInInspector] public GameObject gameObject;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #region Singleton
    public static ObjectPooler Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            UnitObject unitObject = UnitObjectManager.GetUnit(pool.tag);

            ObjectCreator.CreateObjects(unitObject, out var unitGameObject);
            if(unitObject as BuildingObject) ObjectCreator.CreateMover(unitObject, out unitGameObject);

            unitGameObject.SetActive(false);

            pool.gameObject = unitGameObject;
            objectPool.Enqueue(unitGameObject);

            unitGameObject.transform.SetParent(transform);

            for (int i = 1; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.gameObject, transform);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation, out GameObject spawnedObject)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist");
            spawnedObject = null;
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        spawnedObject = objectToSpawn;
        return objectToSpawn;
    }
}
