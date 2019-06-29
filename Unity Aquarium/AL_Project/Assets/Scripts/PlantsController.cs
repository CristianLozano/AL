using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantsController : MonoBehaviour
{
    public GameObject lSystemPrefab;
    public int matrixSize = 6;
    public Vector3 environmentLimits = new Vector3(300, 100, 500);
    public float seasonChange = 30f;
    public bool summer = true;
    public float summerGain = 10f;
    public float winterGain = 2f;
    public float recoverTime = 5f;
    public List<Texture> floorImage;
    public GameObject rockPrefab;
    public List<Transform> rockSpawners;
    private GameObject currentRock;

    private List<PlantEnergy> plants = new List<PlantEnergy>();
    private float lastSeasonChange, lastRecoveryTime;
    private Light scenarioLight;
    private float interpolateParam = 1f;
    private Color summerColor, winterColor;
    private Transform floor;

    void Start()
    {
        scenarioLight = GameObject.Find("Directional Light").GetComponent<Light>();

        floor = GameObject.Find("Floor").transform;

        floor.GetComponent<Renderer>().material.mainTexture = floorImage[0];

        lastSeasonChange = lastRecoveryTime = Time.time;

        summerColor = new Color(1f, 0.9568627f, 0.8392157f);
        winterColor = new Color(1f, 1f, 1f);

        currentRock = Instantiate(rockPrefab, rockSpawners[0]);
        currentRock.transform.localPosition = Vector3.zero;

        for (int i = 0; i < Mathf.Pow(matrixSize, 2f); i++)
        {
            Vector3 pos = new Vector3();

            if (i < Mathf.Pow(matrixSize, 2f) / 2)
            {
                pos = this.transform.position + new Vector3(Random.Range(0, environmentLimits.x), 0f, Random.Range(-environmentLimits.z, environmentLimits.z));
            }
            else
            {
                pos = this.transform.position + new Vector3(Random.Range(-environmentLimits.x, 0), 0f, Random.Range(-environmentLimits.z, environmentLimits.z));
            }
            Spawn(pos);
        } 
    }

    void Update()
    {
        if (Time.time - lastRecoveryTime > recoverTime)
        {
            Sugarscape();
            lastRecoveryTime = Time.time;
        }
        
        if (summer)
        {
            floor.GetComponent<Renderer>().material.mainTexture = floorImage[0];

            if (interpolateParam < 1f)
            {
                interpolateParam += 0.01f;
            }
        }
        else
        {
            floor.GetComponent<Renderer>().material.mainTexture = floorImage[1];

            if (interpolateParam > 0f)
            {
                interpolateParam -= 0.01f;
            }
        }
        scenarioLight.color = Color.Lerp(winterColor, summerColor, interpolateParam);
    }

    public GameObject Spawn(Vector3 position)
    {
        GameObject plant = Instantiate(lSystemPrefab, position, Quaternion.identity, transform);
        plants.Add(plant.GetComponent<PlantEnergy>());
        return plant;
    }

    private void Sugarscape()
    {
        if (Time.time - lastSeasonChange > seasonChange)
        {
            summer = !summer;
            lastSeasonChange = Time.time;
            if(currentRock != null)
            {
                Destroy(currentRock);
            }
            
            if(summer)
            {
                currentRock = Instantiate(rockPrefab, rockSpawners[0]);
            }
            else
            {
                currentRock = Instantiate(rockPrefab, rockSpawners[1]);
            }
            currentRock.transform.localPosition = Vector3.zero;
            
        }

        if (summer)
        {
            for (int i = 0; i < plants.Count / 2; i++)
            {
                plants[i].GainEnergy(summerGain);
            }

            for (int i = plants.Count / 2; i < plants.Count; i++)
            {
                plants[i].GainEnergy(winterGain);
            }
        }
        else
        {
            for (int i = 0; i < plants.Count / 2; i++)
            {
                plants[i].GainEnergy(winterGain);
            }

            for (int i = plants.Count / 2; i < plants.Count; i++)
            {
                plants[i].GainEnergy(summerGain);
            }
        }
    }
}