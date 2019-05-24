using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
    [Range(2f, 10f)]
    public float speed = 4f;
    public float maxAngle;
    public float maxRadius;
    [Range(0f, 10f)]
    public float rotationAngle;
    public LayerMask preyLayer;

    private bool isInFov = false;
    private Collider target;
    private float lastRotation;

    private void OnDrawGizmos()
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
    }

    private void Start()
    {
        lastRotation = Time.time;
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
        isInFov = inFOV(transform, maxAngle, maxRadius);

        if (isInFov)
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
                target = null;
            }
        }
        else
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
    }
}