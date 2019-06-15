using UnityEngine;
using System.Collections.Generic;
using System;

public class AntController : MonoBehaviour
{
    public GameObject antPrefab;

    public int spawnCount = 10;

    public Vector3 swimLimits = new Vector3(300, 100, 500);

    public LayerMask foodLayer;

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Spawn();
        }
    }

    public GameObject Spawn()
    {
        Vector3 pos = this.transform.position + new Vector3(UnityEngine.Random.Range(-swimLimits.x, swimLimits.x), 0, UnityEngine.Random.Range(-swimLimits.z, swimLimits.z));
        return Spawn(pos);
    }

    public GameObject Spawn(Vector3 position)
    {
        GameObject boid = Instantiate(antPrefab, position, new Quaternion(0, UnityEngine.Random.Range(0, 360), 0, 1), transform);
        return boid;
    }
}