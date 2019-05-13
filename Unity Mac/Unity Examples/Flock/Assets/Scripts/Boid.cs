using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private void Awake(){

        transform.localPosition = new Vector3(5f,5f,5f);
    }
    // Start is called before the first frame update
    void Start()
    {
        //Destroying shit
       // Destroy(gameObject);
        //if(transform.childCount!= 0){
          //  Destroy(transform.GetChild(0).gameObject);
        //}//
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
