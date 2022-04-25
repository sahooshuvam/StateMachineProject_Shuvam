using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPoolScripts : MonoBehaviour
{
    public static ObjectPoolScripts Instance;
    public GameObject enemyPrefabs;
    public int number;
    public float spawnRadius;
    public bool spawnOnStart = true;
    Vector3 result;
    float time;

    public List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        return;
    }

    // Start is called before the first frame update
    void Start()
    {
        AddToPool();
    }


    private void AddToPool()
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
            {
                result = hit.position;
                GameObject temp = Instantiate(enemyPrefabs, result, Quaternion.identity);
                temp.SetActive(false);
                pool.Add(temp);
            }
            else
                i--;
        }
    }

    public GameObject GetObjectsFromPool(string tagName)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].gameObject.tag == tagName)
            {
                if (!pool[i].activeInHierarchy)
                {
                    return pool[i];
                }

            }
        }

        return null;
    }
}
