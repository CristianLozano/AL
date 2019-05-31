using UnityEngine;
using System.Collections.Generic;
using System;

public class BoidController : MonoBehaviour
{
    public GameObject boidPrefab;

    public int spawnCount = 10;

    public Vector3 swimLimits = new Vector3(300, 100, 500);

    public Transform boidContainer;

    public LayerMask searchLayer;

    public List<Texture> youngPreySkins;
    public List<Texture> adultPreySkins;
    public List<Texture> oldPreySkins;

    public List<Texture> youngPredatorSkins;
    public List<Texture> adultPredatorSkins;
    public List<Texture> oldPredatorSkins;

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Spawn();
        } 
    }

    public GameObject Spawn()
    {
        Vector3 pos = this.transform.position + new Vector3(UnityEngine.Random.Range(-swimLimits.x, swimLimits.x), UnityEngine.Random.Range(-swimLimits.y, swimLimits.y), UnityEngine.Random.Range(-swimLimits.z, swimLimits.z));
        return Spawn(pos);
    }

    public GameObject Spawn(Vector3 position)
    {
        Quaternion rotation = Quaternion.Slerp(transform.rotation, UnityEngine.Random.rotation, 0.3f);
        GameObject boid = Instantiate(boidPrefab, position, rotation, boidContainer);
        return boid;
    }
}