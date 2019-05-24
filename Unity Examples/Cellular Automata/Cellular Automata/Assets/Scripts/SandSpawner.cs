using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandSpawner : MonoBehaviour
{
    public GameObject sandGrain;
    public float spawnTime;

    private float lastTime;

    // Start is called before the first frame update
    void Start()
    {
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastTime >= spawnTime)
        {
            Instantiate(sandGrain, transform);
            lastTime = Time.time;
        }
    }
}
