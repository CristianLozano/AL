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
    public LayerMask predatorLayer;

    public List<Texture> youngPreySkins;
    public List<Texture> adultPreySkins;
    public List<Texture> oldPreySkins;

    public List<Texture> youngPredatorSkins;
    public List<Texture> adultPredatorSkins;
    public List<Texture> oldPredatorSkins;

    void Start()
    {
        for (int i = 0; i < 34; i++)
        {
            // Load 1st batch of young prey skins
            string path = "Sprites/PreySkins/Young/zebra_skin_young" + i;
            Texture skin = Resources.Load<Texture>(path);
            youngPreySkins.Add(skin);

            // Load 1st batch of adult prey skins
            path = "Sprites/PreySkins/Adult/zebra_skin_adult" + i;
            skin = Resources.Load<Texture>(path);
            adultPreySkins.Add(skin);

            // Load 1st batch of old prey skins
            path = "Sprites/PreySkins/Old/zebra_skin_elder" + i;
            skin = Resources.Load<Texture>(path);
            oldPreySkins.Add(skin);

            // Load 1st batch of young predator skins
            path = "Sprites/PredatorSkins/Young/cheetah_skin_young" + i;
            skin = Resources.Load<Texture>(path);
            youngPredatorSkins.Add(skin);

            // Load 1st batch of adult predator skins
            path = "Sprites/PredatorSkins/Adult/cheetah_skin_adult" + i;
            skin = Resources.Load<Texture>(path);
            adultPredatorSkins.Add(skin);

            // Load 1st batch of old predator skins
            path = "Sprites/PredatorSkins/Old/cheetah_skin_elder" + i;
            skin = Resources.Load<Texture>(path);
            oldPredatorSkins.Add(skin);
        }

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