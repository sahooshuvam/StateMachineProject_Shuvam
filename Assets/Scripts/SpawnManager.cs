using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{

    float time;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    private void Update()
    {
        time = time + Time.deltaTime;
        if (time > 3f)
        {
            GameObject enemyFromPool = ObjectPoolScripts.Instance.GetObjectsFromPool("Enemy");
            if (enemyFromPool != null)
            {
                enemyFromPool.SetActive(true);
            }
            time = 0f;
        }
    }

   
}
