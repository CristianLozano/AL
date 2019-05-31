using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnergy : MonoBehaviour
{
    public float currentEnergy = 100;

    public void ChangeColor()
    {
        if (currentEnergy < 1)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }
}
