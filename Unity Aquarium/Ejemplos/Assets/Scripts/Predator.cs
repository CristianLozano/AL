using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
    // Reference to the controller.
    public BoidController controller;
    [Range(2f, 10f)]
    public float speed = 4f;
    public float maxAngle;
    public float maxRadius;
    [Range(0f, 10f)]
    public float rotationAngle;
    public LayerMask preyLayer;
    public LayerMask predatorLayer;
    public float maxSize = 10f;
    public Vector3 initialSize;
    public float age;
    public float energy, reproductiveEnergy;
    public float waitTime, cooldownTime;
    public bool alive = false;
    public bool male = false;
    public float eatPreyEnergy = 1000f;

    private bool isInFov = false;
    private Collider target;
    private float lastRotation;
    public bool reproduce = false;

    private float size = 3f;
    private float growthRate = 0.0001f;
    private float deathAge;
    private float deathTime, lifeTime, ageTime, lastReproductionMoment;
    private float adultAge = 3f, oldAge = 7f;
    private int currentSkin = 0;
    private Transform closestPredator;
    private Transform predatorContainer;

    private bool changedAdult = false, changedOld = false;
    


    // Visually show predator vision range
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine3 = Quaternion.AngleAxis(maxAngle, transform.right) * transform.forward * maxRadius;
        Vector3 fovLine4 = Quaternion.AngleAxis(-maxAngle, transform.right) * transform.forward * maxRadius;

        if (!isInFov)
            Gizmos.color = Color.blue;
        else
            Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);
        Gizmos.DrawRay(transform.position, fovLine3);
        Gizmos.DrawRay(transform.position, fovLine4);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);
    }*/

    private void Start()
    {
        controller = GameObject.Find("BoidController").GetComponent<BoidController>();
        alive = true;
        lastRotation = Time.time;
        predatorContainer = transform.parent;
        initialSize = transform.localScale;
        deathAge = Random.Range(20f, 25f);
        lifeTime = Time.time;
        initialSize = transform.localScale;

        StartCoroutine(WaitForSkin());

        // Randomly select gender
        male = Random.value > 0.5f;
    }

    private IEnumerator WaitForSkin()
    {
        yield return new WaitForSeconds(1f);
        currentSkin = UnityEngine.Random.Range(0, controller.youngPredatorSkins.Count);
        transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.mainTexture = controller.youngPredatorSkins[currentSkin];
    }

    public bool inFOV(Transform checkingObject, float maxAngle, float maxRadius)
    {
        // Search only for prey
        Collider[] overlaps = Physics.OverlapSphere(checkingObject.position, maxRadius, preyLayer);
        List<Collider> foundPrey = new List<Collider>(overlaps);

        // Follow the same prey if still in sight
        if (foundPrey.Contains(target))
        {
            Vector3 directionBetween = (target.transform.position - checkingObject.position).normalized;

            float angle = Vector3.Angle(checkingObject.forward, directionBetween);

            if (angle <= maxAngle)
            {
                Ray ray = new Ray(checkingObject.position, target.transform.position - checkingObject.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, maxRadius))
                {
                    if (hit.transform == target.transform)
                    {
                        return true;
                    }
                }
            }
        }

        foreach (Collider prey in foundPrey)
        {
            if (prey != null)
            {
                Vector3 directionBetween = (prey.transform.position - checkingObject.position).normalized;

                float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                if (angle <= maxAngle)
                {
                    Ray ray = new Ray(checkingObject.position, prey.transform.position - checkingObject.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, maxRadius))
                    {
                        if (hit.transform == prey.transform)
                        {
                            target = prey;
                            return true;
                        }  
                    }
                }
            }
        }

        return false;
    }

    private void Update()
    {
        if(alive)
        {
            // Increase age of the predator
            IncreaseAge();

            if (age >= deathAge)
            {
                KillPredator();
                return;
            }

            LoseEnergy();

            if(energy <= 0)
            {
                KillPredator();
                return;
            }

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

            if(!reproduce)
            {
                isInFov = inFOV(transform, maxAngle, maxRadius);

                if (isInFov)
                {
                    ChasePrey();
                }
                else
                {
                    FreeMovement();
                }
            }
            else
            {
                if(closestPredator == null)
                {
                    // Looks up nearby predators.
                    Collider[] nearbyPredator = Physics.OverlapSphere(transform.position, maxRadius * 3, predatorLayer);
                    List<Collider> couples = new List<Collider>(nearbyPredator);

                    if(couples.Count == 1)
                    {
                        FreeMovement();
                        return;
                    }

                    float minDistance = float.PositiveInfinity;

                    foreach (Collider predator in nearbyPredator)
                    {
                        if (predator.gameObject.GetComponent<Predator>().alive == false) continue;
                        if (predator.gameObject.GetComponent<Predator>().male == male) continue;
                        if (predator.gameObject == gameObject) continue;

                        Transform t = predator.transform;
                        float distance = Vector3.Distance(transform.localPosition, t.localPosition);

                        if(distance < minDistance)
                        {
                            minDistance = distance;
                            closestPredator = t;
                        }
                    }

                    if(minDistance == float.PositiveInfinity)
                    {
                        FreeMovement();
                    }
                }
                else
                {
                    transform.LookAt(closestPredator);
                    transform.position = Vector3.MoveTowards(transform.position, closestPredator.position, speed * Time.deltaTime);
                }
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
    
    private void IncreaseAge()
    {
        if(Time.time - ageTime >= 2f)
        {
            age += 0.1f;
            ageTime = Time.time;
        }

        if(age >= adultAge && age < oldAge && !changedAdult)
        {
            currentSkin  = UnityEngine.Random.Range(0, controller.adultPredatorSkins.Count);
            transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.mainTexture = controller.adultPredatorSkins[currentSkin];
            changedAdult = true;
        }
        else if(age >= oldAge && !changedOld)
        {
            currentSkin  = UnityEngine.Random.Range(0, controller.oldPredatorSkins.Count);
            transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.mainTexture = controller.oldPredatorSkins[currentSkin];
            changedOld = true;
        }
    }

    private void LoseEnergy()
    {
        if(Time.time - lifeTime >= waitTime)
        {
            float volume = transform.localScale.x * transform.localScale.y + transform.localScale.z;

            energy -= volume;
            energy -= speed;
            lifeTime = Time.time;
        }
    }

    public void KillPredator()
    {
        Rigidbody deadPredator = GetComponent<Rigidbody>();
        alive = false;
        deathTime = Time.time;
        deadPredator.mass = 20000;
        deadPredator.drag = 2;
        deadPredator.angularDrag = 2;
        // Assign dead layer
        gameObject.layer = 10;
        GetComponent<Rigidbody>().useGravity = true;
        Animation animator = transform.GetChild(0).GetComponent<Animation>();
        animator.Stop();
    }

    private void ChasePrey()
    {
        Vector3 newDir = Vector3.RotateTowards(transform.forward, target.transform.position, speed * Time.deltaTime, 0.0f);
        //transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, speed * Time.deltaTime);
        //transform.rotation = Quaternion.LookRotation(newDir);
        transform.LookAt(target.transform);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        // Check if the position of the predator and prey are approximately equal.
        if (Vector3.Distance(transform.position, target.transform.position) < 3f)
        {
            Debug.Log("Ate prey");
            Animation animator = transform.GetChild(0).GetComponent<Animation>();
            animator.Stop();
            animator.Play("Attack");
            // Prepare animation of swimming to continue
            animator.PlayQueued("Swim");
            target.GetComponent<Boid>().KillBoid();
            energy += eatPreyEnergy;
            target = null;
        }
    }

    private void FreeMovement()
    {
        if (Time.time - lastRotation >= 10f)
        {
            float x, y, z = 0f;
            x = Random.Range(-rotationAngle, rotationAngle);
            y = Random.Range(-rotationAngle, rotationAngle);
            z = Random.Range(-rotationAngle, rotationAngle);
            transform.Rotate(new Vector3(x, y, z));
            lastRotation = Time.time;
        }

        if (transform.localRotation.z >= 20f || transform.localRotation.z <= -20f)
        {
            transform.localRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, 0f, 0);
        }

        // Moves forward.
        transform.position += transform.forward * (speed * Time.deltaTime);

        // Check to don't collider with walls
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRadius))
        {
            if (hit.transform.tag == "Wall")
            {
                Quaternion lookQuaternion = transform.rotation * Quaternion.AngleAxis(180, Vector3.up);
                transform.rotation = lookQuaternion;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (reproduce)
        {
            if (col.transform == closestPredator)
            {
                Predator parentTraits = closestPredator.GetComponent<Predator>();

                GameObject child = Instantiate(gameObject, transform.localPosition + Vector3.one * 5, Quaternion.identity, predatorContainer);
                energy -= 5000;
                parentTraits.energy -= 5000;
                parentTraits.reproduce = reproduce = false;
                parentTraits.lastReproductionMoment = lastReproductionMoment = Time.time;

                Predator childTraits = child.GetComponent<Predator>();

                childTraits.speed = (parentTraits.speed + speed) / Random.Range(1.5f, 2.5f) + Random.Range(-0.1f, 0.1f);
                childTraits.maxRadius = (parentTraits.maxRadius + maxRadius) / Random.Range(1.5f, 2.5f) + Random.Range(-0.1f, 0.1f);
                childTraits.maxSize = (parentTraits.maxSize + maxSize) / Random.Range(1.5f, 2.5f) + Random.Range(-0.1f, 0.1f);
                childTraits.reproductiveEnergy = (parentTraits.reproductiveEnergy + reproductiveEnergy) / Random.Range(1.5f, 2.5f) + Random.Range(-0.1f, 0.1f);
                childTraits.energy = 10000 + Random.Range(-100f, 100f);
                childTraits.age = 0f;

                // Color Mutation
                Color parent1Color = closestPredator.GetChild(0).GetChild(1).GetComponent<Renderer>().material.color;
                Color parent2Color = transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.color;
                float r, g, b = 0f;
                r = (parent1Color.r + parent2Color.r) / Random.Range(1.5f, 2.5f) + Random.Range(-0.1f, 0.1f);
                g = (parent1Color.g + parent2Color.g) / Random.Range(1.5f, 2.5f) + Random.Range(-0.1f, 0.1f);
                b = (parent1Color.b + parent2Color.b) / Random.Range(1.5f, 2.5f) + Random.Range(-0.1f, 0.1f);
                child.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.color = new Color(r, g, b);

                // Size mutation
                float x, y, z = 0f;
                x = (parentTraits.initialSize.x + initialSize.x) / Random.Range(1.5f, 2.5f) + Random.Range(-0.1f, 0.1f);
                y = (parentTraits.initialSize.y + initialSize.y) / Random.Range(1.5f, 2.5f) + Random.Range(-0.1f, 0.1f);
                z = (parentTraits.initialSize.z + initialSize.z) / Random.Range(1.5f, 2.5f) + Random.Range(-0.1f, 0.1f);
                child.transform.localScale = childTraits.initialSize = new Vector3(x, y, z);

                child.transform.localPosition = transform.localPosition;

                parentTraits.reproduce = false;
                closestPredator = null;

                // Prevent getting stuck in one position after reproduction
                GetComponent<CapsuleCollider>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = true;
            }
        }
    }
}