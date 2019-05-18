using UnityEngine;
using System.Collections;

public class BoidController : MonoBehaviour
{
    public GameObject boidPrefab;

    public int spawnCount = 10;

    public Vector3 swimLimits = new Vector3(300, 100, 500);

    public LayerMask searchLayer;

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Spawn();
        } 
    }

    public GameObject Spawn()
    {
        Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x), Random.Range(0, swimLimits.y), Random.Range(0, swimLimits.z));
        return Spawn(pos);
    }

    public GameObject Spawn(Vector3 position)
    {
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Random.rotation, 0.3f);
        GameObject boid = Instantiate(boidPrefab, position, rotation, transform);
        return boid;
    }
}