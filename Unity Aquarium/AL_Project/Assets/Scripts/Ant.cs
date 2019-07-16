using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    private float lastRotation;

    // Reference to the controller.
    public AntController controller;
    public float waitTime = 5;
    public float speed = 1;
    public bool hasFood = false;
    public GameObject pheromonePrefab;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("AntController").GetComponent<AntController>();

        lastRotation = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFood)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.parent.position, speed * Time.deltaTime);

            Instantiate(pheromonePrefab, transform.position, Quaternion.identity);

            if (transform.position == transform.parent.position)
            {
                hasFood = false;
            }
        }
        else
        {
            // Looks up nearby pheromones
            Collider[] nearbyFood = Physics.OverlapSphere(transform.position, 10, controller.foodLayer);
            List<Collider> foundFood = new List<Collider>(nearbyFood);

            if (foundFood.Count > 0)
            {
                int randomPheromone = Random.Range(0, foundFood.Count);
                transform.LookAt(foundFood[randomPheromone].transform.position);
                transform.position = Vector3.MoveTowards(transform.position, foundFood[randomPheromone].transform.position, speed * Time.deltaTime);
                return;
            }

            if (Time.time - lastRotation > waitTime)
            {
                lastRotation = Time.time;
                RandomRotation();
                return;
            }

            transform.localPosition += transform.forward * (speed * Time.deltaTime);
        }
    }

    private void RandomRotation()
    {
        transform.localEulerAngles = new Vector3(0, Random.Range(-60, 60), 0);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "AntWall")
        {
            float currentY = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(0, currentY + 180, 0);
        }

        if (!hasFood)
        {
            if (col.gameObject.tag == "Food")
            {
                hasFood = true;
                Destroy(col.gameObject);
            }
        }
    }
}
