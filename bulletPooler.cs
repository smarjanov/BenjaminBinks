using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletPooler : MonoBehaviour
{
    [System.Serializable]
    public class BulletPool
    {

        public string tag;
        public GameObject prefabBullet;
        public int intSize;
    }

    public static bulletPooler Instance;
    public float thrust;
    public Transform bulletPosition;

    private void Awake()
    {
        Instance = this;
    }

    public List<BulletPool> pools;
    public Dictionary<string, Queue<GameObject>> bulletDictionary;
    void Start()
    {
        bulletDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (BulletPool pool in pools)
        {
            Queue<GameObject> bulletPool = new Queue<GameObject>();
            for (int i=0;i<pool.intSize; i++)
            {
                GameObject obj = Instantiate(pool.prefabBullet);
                obj.SetActive(false);
                bulletPool.Enqueue(obj);
            }

            bulletDictionary.Add(pool.tag, bulletPool);

        }


    }
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!bulletDictionary.ContainsKey(tag)) {

            Debug.LogWarning("Aint working");
            return null;
        }

        GameObject objectToSpawn = bulletDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        Rigidbody rigidBullet = objectToSpawn.GetComponent<Rigidbody>();
        rigidBullet.velocity = thrust * bulletPosition.forward;

        bulletDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;

    }


}
