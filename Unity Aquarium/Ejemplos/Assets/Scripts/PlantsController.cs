using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantsController : MonoBehaviour
{
    public GameObject lSystemPrefab;

    public int spawnCount = 10;

    public Vector3 environmentLimits = new Vector3(300, 100, 500);

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Spawn();
        } 
    }

    public GameObject Spawn()
    {
        Vector3 pos = this.transform.position + new Vector3(Random.Range(-environmentLimits.x, environmentLimits.x), 0f, Random.Range(-environmentLimits.z, environmentLimits.z));
        return Spawn(pos);
    }

    public GameObject Spawn(Vector3 position)
    {
        GameObject plant = Instantiate(lSystemPrefab, position, Quaternion.identity, transform);
        return plant;
    }
}
