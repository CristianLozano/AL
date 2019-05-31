using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public Vector3 initialSize;

    public float age;
    public float energy, reproductiveEnergy;
    public float waitTime, cooldownTime;
    public bool alive = false;

    public LayerMask plantLayer;

    private float size = 3f;
    private float growthRate = 0.0001f;
    private float deathAge;
    private float deathTime, lifeTime, ageTime, lastReproductionMoment;
    private float adultAge = 3f, oldAge = 7f;
    private int currentSkin = 0;
    public Collider target;
    

    // Random seed.
    private float noiseOffset;

    private bool reproduce = false;

    private Transform closestBoid;

    private bool changedAdult = false, changedOld = false;

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
        //controller = transform.parent.GetComponent<BoidController>();
        alive = true;
        deathAge = Random.Range(10f, 15f);
        noiseOffset = Random.value * 10.0f;
        lifeTime = Time.time;
        initialSize = transform.localScale;

        currentSkin  = UnityEngine.Random.Range(0, controller.youngPreySkins.Count);
        transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = controller.youngPreySkins[currentSkin];
    }

    void Update()
    {
        if (alive)
        {
            // Control size of the boid
            size += growthRate;

            if (size >= maxSize)
            {
                size = maxSize;
            }
            else
            {
                transform.localScale += Vector3.one * growthRate;
            }

            if (energy >= reproductiveEnergy && age >= adultAge  && Time.time - lastReproductionMoment > cooldownTime)
            {
                reproduce = true;
            }
            else
            {
                reproduce = false;
            }

            // Flocking behaviour
            if(!reproduce)
            {
                if(energy > reproductiveEnergy)
                {
                    FlockMovement();
                }
                else
                {
                    SearchFood(transform, neighborDist * 10);
                }
            }
            else
            {
                if(closestBoid == null)
                {
                    // Looks up nearby boids.
                    Collider[] nearbyBoids = Physics.OverlapSphere(transform.position, neighborDist, controller.searchLayer);
                    List<Collider> couples = new List<Collider>(nearbyBoids);

                    if(couples.Count == 1)
                    {
                        FlockMovement();
                        return;
                    }

                    float minDistance = float.PositiveInfinity;

                    foreach (Collider boid in nearbyBoids)
                    {
                        if (boid.gameObject.GetComponent<Boid>().alive == false) continue;
                        if (boid.gameObject == gameObject) continue;

                        Transform t = boid.transform;
                        float distance = Vector3.Distance(transform.localPosition, t.localPosition);

                        if(distance < minDistance)
                        {
                            minDistance = distance;
                            closestBoid = t;
                        }
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, closestBoid.position, velocity * Time.deltaTime);
                }
            }

            // Increase age of the boid
            IncreaseAge();

            if (age >= deathAge)
            {
                KillBoid();
            }

            LoseEnergy();

            if(energy <= 0)
            {
                KillBoid();
            }
        }
        else
        {
            if ((Time.time - deathTime) >= 30f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FlockMovement()
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
    }

    public void SearchFood(Transform checkingObject, float maxRadius)
    {
        // Search only for plants
        Collider[] overlaps = Physics.OverlapSphere(checkingObject.position, maxRadius, plantLayer);
        List<Collider> foundPlant = new List<Collider>(overlaps);

        if (target != null)
        {
            if (target.GetComponent<PlantEnergy>().currentEnergy == 0f)
            {
                target = null;
            }
        }

        // Follow the same plant if still in sight
        if (foundPlant.Contains(target))
        {
            transform.LookAt(target.transform);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, velocity * Time.deltaTime);
            return;
        }

        float closestDistance = float.PositiveInfinity;
        int counter = 0;
        int chosenPlant = -1;
        
        foreach (Collider plant in foundPlant)
        {
            if (plant != null)
            {
                float distance = Vector3.Distance(transform.position, plant.transform.position);

                if(distance < closestDistance)
                {
                    if (plant.GetComponent<PlantEnergy>().currentEnergy > 0f)
                    {
                        closestDistance = distance;
                        chosenPlant = counter;
                    }
                }
                
                counter++;
            }
        }

        if(foundPlant.Count == 0)
        {
            FlockMovement();
            return;
        }

        if (chosenPlant == -1)
        {
            FlockMovement();
            return;
        }

        target = foundPlant[chosenPlant];

        if(target == null)
        {
            FlockMovement();
            return;
        }
    }

    private void IncreaseAge()
    {
        if(Time.time - ageTime >= 2f)
        {
            age += 0.1f;
            ageTime = Time.time;
        }

        if(age >= adultAge && age < oldAge && !changedAdult)
        {
            currentSkin  = UnityEngine.Random.Range(0, controller.adultPreySkins.Count);
            transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = controller.adultPreySkins[currentSkin];
            changedAdult = true;
        }
        else if(age >= oldAge && !changedOld)
        {
            currentSkin  = UnityEngine.Random.Range(0, controller.oldPreySkins.Count);
            transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = controller.oldPreySkins[currentSkin];
            changedOld = true;
        }
    }
    private void LoseEnergy()
    {
        if(Time.time - lifeTime >= waitTime)
        {
            float volume = transform.localScale.x * transform.localScale.y + transform.localScale.z;

            energy -= volume;
            energy -= velocity;
            lifeTime = Time.time;
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

    void OnCollisionEnter(Collision col)
    {
        if (reproduce)
        {
            if (col.transform == closestBoid)
            {
                Boid parentTraits = closestBoid.GetComponent<Boid>();

                GameObject child = Instantiate(gameObject, transform.localPosition + Vector3.one * 5, Quaternion.identity, controller.boidContainer);
                energy -= 5000;
                parentTraits.energy -= 5000;
                parentTraits.reproduce = reproduce = false;
                parentTraits.lastReproductionMoment = lastReproductionMoment = Time.time;

                Boid childTraits = child.GetComponent<Boid>();

                childTraits.velocity = (parentTraits.velocity + velocity) / 2f + Random.Range(-0.1f, 0.1f);
                childTraits.neighborDist = (parentTraits.neighborDist + neighborDist) / 2f + Random.Range(-0.1f, 0.1f);
                childTraits.maxSize = (parentTraits.maxSize + maxSize) / 2f + Random.Range(-0.1f, 0.1f);
                childTraits.reproductiveEnergy = (parentTraits.reproductiveEnergy + reproductiveEnergy) / 2f + Random.Range(-0.1f, 0.1f);
                childTraits.energy = 10000 + Random.Range(-100f, 100f);
                childTraits.age = 0f;

                // Color Mutation
                Color parent1Color = closestBoid.GetChild(0).GetComponent<Renderer>().material.color;
                Color parent2Color = transform.GetChild(0).GetComponent<Renderer>().material.color;
                float r, g, b = 0f;
                r = (parent1Color.r + parent2Color.r) / 2f + Random.Range(-0.1f, 0.1f);
                g = (parent1Color.g + parent2Color.g) / 2f + Random.Range(-0.1f, 0.1f);
                b = (parent1Color.b + parent2Color.b) / 2f + Random.Range(-0.1f, 0.1f);
                child.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(r, g, b);

                // Size mutation
                float x, y, z = 0f;
                x = (parentTraits.initialSize.x + initialSize.x) / 2f + Random.Range(-0.1f, 0.1f);
                y = (parentTraits.initialSize.y + initialSize.y) / 2f + Random.Range(-0.1f, 0.1f);
                z = (parentTraits.initialSize.z + initialSize.z) / 2f + Random.Range(-0.1f, 0.1f);
                child.transform.localScale = childTraits.initialSize = new Vector3(x, y, z);

                child.transform.localPosition = transform.localPosition;

                parentTraits.reproduce = false;
                closestBoid = null;
            }
        }
        else if (col.collider == target)
        {
            energy += target.GetComponent<PlantEnergy>().currentEnergy;
            target.GetComponent<PlantEnergy>().currentEnergy = 0f;
            target.GetComponent<PlantEnergy>().ChangeColor();
            target = null;
        }
    }
}