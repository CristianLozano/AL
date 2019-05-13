using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    // Variables
    public GameObject boidPrefab;
    public int boidNumber = 50;
    public GameObject[] allBoids;
    public Vector3 swimLimits = new Vector3(300,100,500);

    [Header("boid Settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    Vector3 pos;
    [Range(1.0f, 10.0f)]
    public float neighborDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        /*for (int i = 0; i < boidNumber; i++)
        {
            GameObject boid = Instantiate(boidPrefab, transform);

            float x, y, z;

            x = Random.Range(0, 50f);
            y = Random.Range(0, 50f);
            z = Random.Range(0, 50f);

            boid.transform.localPosition = new Vector3(x, y, z);
        }*/
        allBoids = new GameObject[boidNumber];
        for(int i = 0; i < boidNumber; i++){
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x,swimLimits.x), Random.Range(0,swimLimits.y),Random.Range(0, swimLimits.z));
            allBoids[i] = (GameObject) Instantiate(boidPrefab, pos, Quaternion.identity);
            allBoids[i].GetComponent<Boid>().controller = this;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
