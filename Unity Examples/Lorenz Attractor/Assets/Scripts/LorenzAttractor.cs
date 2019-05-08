using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LorenzAttractor : MonoBehaviour
{
    public float x = 0.1f;
    public float y = 0f;
    public float z = 0f;

    public float a = 10f;
    public float b = 28f;
    public float c = 8.0f / 3.0f;
    public float t = 0.01f;

    public int iterationNumber = 10000;
    
    void Start()
    {
        transform.position = new Vector3(x, y, z);

        StartCoroutine(LorenzMove());
    }

    private IEnumerator LorenzMove()
    {
        for (int i = 0; i < iterationNumber; i++)
        {
            float xt = x + t * a * (y - x);
            float yt = y + t * (x * (b - z) - y);
            float zt = z + t * (x * y - c * z);

            x = xt;
            y = yt;
            z = zt;
            
            transform.position = new Vector3(xt, yt, zt);

            yield return 0;
        }
    }
}