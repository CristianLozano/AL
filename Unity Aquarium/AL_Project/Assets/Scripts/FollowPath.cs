using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    private List<Transform> waypoints = new List<Transform>();

    public int currentWaypoint = 0;
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Transform waypointContainer = GameObject.Find("Waypoints").transform;

        foreach (Transform waypoint in waypointContainer)
        {
            waypoints.Add(waypoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.001f)
        {
            currentWaypoint++;

            if (currentWaypoint > waypoints.Count - 1)
            {
                currentWaypoint = 0;
            }
        }
    }
}