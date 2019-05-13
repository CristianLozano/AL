using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{

    //variables
    public GameObject boidPrefab;
    public int boidNumber;
    private int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        /* for(int i = 0;i < boidNumber;i++){
            GameObject boid = Instantiate(boidPrefab,transform);
            float x,y,z;
            x = Random.Range(0,50f);
            y = Random.Range(0,50f);
            z = Random.Range(0,50f);
            boid.transform.localPosition = new Vector3(x,y,z);
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(counter < boidNumber){
            GameObject boid = Instantiate(boidPrefab,transform);
            float x,y,z;
            x = Random.Range(0,50f);
            y = Random.Range(0,50f);
            z = Random.Range(0,50f);
            boid.transform.localPosition = new Vector3(x,y,z);
            counter++;
        }
    }
}
