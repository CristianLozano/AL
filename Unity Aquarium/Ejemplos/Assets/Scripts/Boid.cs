using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour
{
    // Reference to the controller.
    public BoidController controller;

    [Range(0.1f, 20.0f)]
    public float velocity = 6.0f;

    [Range(0.0f, 0.9f)]
    public float velocityVariation = 0.5f;

    [Range(0.1f, 20.0f)]
    public float rotationCoeff = 4.0f;

    [Range(0.1f, 50.0f)]
    public float neighborDist = 2.0f;

    public float maxSize = 10f;

    public int age;
    public int energy;
    public bool alive = false;

    private float size = 3f;
    private float growthRate = 0.0001f;
    private int deathAge;
    private float deathTime;


    // Random seed.
    private float noiseOffset;

    // Caluculates the separation vector with a target.
    Vector3 GetSeparationVector(Transform target)
    {
        Vector3 diff = transform.position - target.transform.position;
        float diffLen = diff.magnitude;
        float scaler = Mathf.Clamp01(1.0f - diffLen / neighborDist);
        return diff * (scaler / diffLen);
    }

    void Start()
    {
        controller = GameObject.Find("BoidController").GetComponent<BoidController>();
        alive = true;
        deathAge = Random.Range(2000, 10000);
        noiseOffset = Random.value * 10.0f;
    }

    void Update()
    {
        if (alive)
        {
            Vector3 currentPosition = transform.position;
            Quaternion currentRotation = transform.rotation;

            // Current velocity randomized with noise.
            float noise = Mathf.PerlinNoise(Time.time, noiseOffset) * 2.0f - 1.0f;
            float boidVelocity = velocity * (1.0f + noise * velocityVariation);

            // Initializes the vectors.
            Vector3 separation = Vector3.zero;
            Vector3 alignment = controller.transform.forward;
            Vector3 cohesion = controller.transform.position;

            // Looks up nearby boids.
            Collider[] nearbyBoids = Physics.OverlapSphere(currentPosition, neighborDist, controller.searchLayer);

            // Accumulates the vectors.
            foreach (Collider boid in nearbyBoids)
            {
                if (boid.gameObject.GetComponent<Boid>().alive == false) continue;
                if (boid.gameObject == gameObject) continue;

                Transform t = boid.transform;
                separation += GetSeparationVector(t);
                alignment += t.forward;
                cohesion += t.position;
            }

            float avg = 1.0f / nearbyBoids.Length;
            alignment *= avg;
            cohesion *= avg;
            cohesion = (cohesion - currentPosition).normalized;

            // Calculates a rotation from the vectors.
            Vector3 direction = separation + alignment + cohesion;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, direction.normalized);

            // Applys the rotation with interpolation.
            if (rotation != currentRotation)
            {
                float ip = Mathf.Exp(-rotationCoeff * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(rotation, currentRotation, ip);
            }

            // Moves forawrd.
            transform.position = currentPosition + transform.forward * (velocity * Time.deltaTime);

            // Control size of the boid
            size += growthRate;

            if (size >= maxSize)
            {
                size = maxSize;
            }
            else
            {
                transform.localScale = Vector3.one * size;
            }

            // Increase age of the boid
            age++;

            if (age >= deathAge)
            {
                KillBoid();
            }
        }
        else
        {
            if ((Time.time - deathTime) >= 60f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void KillBoid()
    {
        Rigidbody deadBoid = GetComponent<Rigidbody>();
        alive = false;
        deathTime = Time.time;
        deadBoid.mass = 2;
        deadBoid.drag = 2;
        deadBoid.angularDrag = 2;
        // Assign dead layer
        gameObject.layer = 10;
        GetComponent<Rigidbody>().useGravity = true;
    }
}