using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SierpinskiTriangleGenerate : MonoBehaviour
{
    public List<Transform> vertices = new List<Transform>();
    public GameObject pointPrefab;
    public int iterations = 100;
    public int batchSize = 500;
    [Range(1, 100)]
    public int distancePercent = 50;
    public Text counter, percentage;
    
    private Transform currentSphere;

    void Start()
    {
        percentage.text = "Percentage: " + distancePercent + "%";

        currentSphere = transform.GetChild(vertices.Count);

        StartCoroutine(GeneratePoint(iterations));
    }

    // Use IEnumerator to have appearing effect
    public IEnumerator GeneratePoint(int iterations)
    {
        for (int i = 0; i < iterations/ batchSize; i++)
        {
            for (int j = 0; j < batchSize; j++)
            {
                Transform randomVertex = vertices[Random.Range(0, vertices.Count)];

                Vector3 newPos = new Vector3();

                // Calculate point between target and current point, depending on distance percentage
                newPos = currentSphere.localPosition + (randomVertex.localPosition - currentSphere.localPosition) / (100f/distancePercent);

                currentSphere = Instantiate(pointPrefab, newPos, Quaternion.identity, transform).transform;

                counter.text = "Counter: " + ((i + 1) * batchSize);
            }

            yield return 0;
        }
    }
}
