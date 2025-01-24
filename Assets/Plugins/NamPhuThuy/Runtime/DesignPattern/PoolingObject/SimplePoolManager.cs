using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamPhuThuy
{
    public class SimplePoolManager : SimpleSingleton<SimplePoolManager>
    {
        private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

        /// <summary>
        /// Creates a pool of inactive GameObjects.
        /// </summary>
        /// <param name="prefab">The prefab to pool.</param>
        /// <param name="poolSize">The initial size of the pool.</param>
        public void CreatePool(GameObject prefab, int poolSize)
        {
            string poolKey = prefab.name;
            if (!poolDictionary.ContainsKey(poolKey))
            {
                poolDictionary[poolKey] = new Queue<GameObject>();

                for (int i = 0; i < poolSize; i++)
                {
                    GameObject obj = Instantiate(prefab);
                    obj.SetActive(false);
                    poolDictionary[poolKey].Enqueue(obj);
                }
            }
        }

        /// <summary>
        /// Retrieves an object from the pool. If the pool is empty, a new object is instantiated.
        /// </summary>
        /// <param name="prefab">The prefab to retrieve from the pool.</param>
        /// <returns>A GameObject from the pool.</returns>
        public GameObject GetObjectFromPool(GameObject prefab)
        {
            string poolKey = prefab.name;
            if (poolDictionary.ContainsKey(poolKey) && poolDictionary[poolKey].Count > 0)
            {
                GameObject obj = poolDictionary[poolKey].Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(true);
                return obj;
            }
        }

        /// <summary>
        /// Returns an object to the pool, deactivating it in the process.
        /// </summary>
        /// <param name="obj">The GameObject to return to the pool.</param>
        public void ReturnObjectToPool(GameObject obj)
        {
            string poolKey = obj.name.Replace("(Clone)", "").Trim();
            if (poolDictionary.ContainsKey(poolKey))
            {
                obj.SetActive(false);
                poolDictionary[poolKey].Enqueue(obj);
            }
            else
            {
                Destroy(obj);
            }
        }
    }
}