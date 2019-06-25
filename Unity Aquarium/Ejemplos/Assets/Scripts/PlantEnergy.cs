using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnergy : MonoBehaviour
{
    public float currentEnergy = 1000;
    public float biteEnergy = 100;

    private float maxEnergy;
    private float lastRecoverTime;

    void Start()
    {
        maxEnergy = currentEnergy;
        lastRecoverTime = Time.time;
    }

    public void LoseEnergy()
    {
        currentEnergy -= biteEnergy;
        if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }
        ChangeColor();
    }

    public void GainEnergy(float energyGain)
    {
        currentEnergy += energyGain;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        ChangeColor();
    }

    private void ChangeColor()
    {
        // No energy, "dead" plant
        if (currentEnergy < biteEnergy)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<MeshRenderer>().material.color = new Color(129f / 255f, 97f / 255f, 60f / 255f);
            }
        }
        // Between half and no energy
        else if (currentEnergy >= biteEnergy && currentEnergy < maxEnergy / 2)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<MeshRenderer>().material.color = new Color(128f / 255f, 128f / 255f, 0f / 255f);
            }
        }
        // More than half of energy
        else
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }
    }
}