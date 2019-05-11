using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 position, velocity, acceleration, alignment, cohesion, separation; // position, velocity, and acceleration in
                                                                                      // a vector datatype
    public float neighborhoodRadius; // radius in which it looks for fellow boids
    public float maxSpeed; // maximum magnitude for the velocity vector
    public float maxSteerForce; // maximum magnitude of the steering vector
    public float sc; // scale factor for the render of the boid
    public float flap;
    public float t;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3((float)Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        acceleration = new Vector3(0, 0, 0);
        neighborhoodRadius = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        t += .1;
        flap = 10 * sin(t);
        // acceleration.add(steer(new Vector(mouseX,mouseY,300),true));
        // acceleration.add(new Vector(0,.05,0));
        if (avoidWalls)
        {
            acceleration.add(Vector.multiply(avoid(new Vector(position.x(), flockHeight, position.z())), 5));
            acceleration.add(Vector.multiply(avoid(new Vector(position.x(), 0, position.z())), 5));
            acceleration.add(Vector.multiply(avoid(new Vector(flockWidth, position.y(), position.z())), 5));
            acceleration.add(Vector.multiply(avoid(new Vector(0, position.y(), position.z())), 5));
            acceleration.add(Vector.multiply(avoid(new Vector(position.x(), position.y(), 0)), 5));
            acceleration.add(Vector.multiply(avoid(new Vector(position.x(), position.y(), flockDepth)), 5));
        }
        flock(bl);
        move();
        checkBounds();
    }
}




/*

  public void run(ArrayList<Boid> bl)
{
    t += .1;
    flap = 10 * sin(t);
    // acceleration.add(steer(new Vector(mouseX,mouseY,300),true));
    // acceleration.add(new Vector(0,.05,0));
    if (avoidWalls)
    {
        acceleration.add(Vector.multiply(avoid(new Vector(position.x(), flockHeight, position.z())), 5));
        acceleration.add(Vector.multiply(avoid(new Vector(position.x(), 0, position.z())), 5));
        acceleration.add(Vector.multiply(avoid(new Vector(flockWidth, position.y(), position.z())), 5));
        acceleration.add(Vector.multiply(avoid(new Vector(0, position.y(), position.z())), 5));
        acceleration.add(Vector.multiply(avoid(new Vector(position.x(), position.y(), 0)), 5));
        acceleration.add(Vector.multiply(avoid(new Vector(position.x(), position.y(), flockDepth)), 5));
    }
    flock(bl);
    move();
    checkBounds();
}

Vector avoid(Vector target)
{
    Vector steer = new Vector(); // creates vector for steering
    steer.set(Vector.subtract(position, target)); // steering vector points away from
    steer.multiply(1 / sq(Vector.distance(position, target)));
    return steer;
}

//-----------behaviors---------------

void flock(ArrayList<Boid> boids)
{
    //alignment
    alignment = new Vector(0, 0, 0);
    int alignmentCount = 0;
    //cohesion
    Vector posSum = new Vector();
    int cohesionCount = 0;
    //separation
    separation = new Vector(0, 0, 0);
    Vector repulse;
    for (int i = 0; i < boids.size(); i++)
    {
        Boid boid = boids.get(i);
        //alignment
        float distance = Vector.distance(position, boid.position);
        if (distance > 0 && distance <= neighborhoodRadius)
        {
            alignment.add(boid.velocity);
            alignmentCount++;
        }
        //cohesion
        float dist = dist(position.x(), position.y(), boid.position.x(), boid.position.y());
        if (dist > 0 && dist <= neighborhoodRadius)
        {
            posSum.add(boid.position);
            cohesionCount++;
        }
        //separation
        if (distance > 0 && distance <= neighborhoodRadius)
        {
            repulse = Vector.subtract(position, boid.position);
            repulse.normalize();
            repulse.divide(distance);
            separation.add(repulse);
        }
    }
    //alignment
    if (alignmentCount > 0)
    {
        alignment.divide((float)alignmentCount);
        alignment.limit(maxSteerForce);
    }
    //cohesion
    if (cohesionCount > 0)
        posSum.divide((float)cohesionCount);
    cohesion = Vector.subtract(posSum, position);
    cohesion.limit(maxSteerForce);

    acceleration.add(Vector.multiply(alignment, 1));
    acceleration.add(Vector.multiply(cohesion, 3));
    acceleration.add(Vector.multiply(separation, 1));
}

void move()
{
    velocity.add(acceleration); // add acceleration to velocity
    velocity.limit(maxSpeed); // make sure the velocity vector magnitude does not
    // exceed maxSpeed
    position.add(velocity); // add velocity to position
    frame.setPosition(position);
    frame.setRotation(Quaternion.multiply(new Quaternion(new Vector(0, 1, 0), atan2(-velocity.z(), velocity.x())),
      new Quaternion(new Vector(0, 0, 1), asin(velocity.y() / velocity.magnitude()))));
    acceleration.multiply(0); // reset acceleration
}

void checkBounds()
{
    if (position.x() > flockWidth)
        position.setX(0);
    if (position.x() < 0)
        position.setX(flockWidth);
    if (position.y() > flockHeight)
        position.setY(0);
    if (position.y() < 0)
        position.setY(flockHeight);
    if (position.z() > flockDepth)
        position.setZ(0);
    if (position.z() < 0)
        position.setZ(flockDepth);
}

void render()
{
    pushStyle();

    // uncomment to draw boid axes
    //scene.drawAxes(10);

    strokeWeight(2);
    stroke(color(40, 255, 40));
    fill(color(0, 255, 0, 125));

    // highlight boids under the mouse
    if (scene.trackedFrame("mouseMoved") == frame)
    {
        stroke(color(0, 0, 255));
        fill(color(0, 0, 255));
    }

    // highlight avatar
    if (frame == avatar)
    {
        stroke(color(255, 0, 0));
        fill(color(255, 0, 0));
    }

    //draw boid
    beginShape(TRIANGLES);
    vertex(3 * sc, 0, 0);
    vertex(-3 * sc, 2 * sc, 0);
    vertex(-3 * sc, -2 * sc, 0);

    vertex(3 * sc, 0, 0);
    vertex(-3 * sc, 2 * sc, 0);
    vertex(-3 * sc, 0, 2 * sc);

    vertex(3 * sc, 0, 0);
    vertex(-3 * sc, 0, 2 * sc);
    vertex(-3 * sc, -2 * sc, 0);

    vertex(-3 * sc, 0, 2 * sc);
    vertex(-3 * sc, 2 * sc, 0);
    vertex(-3 * sc, -2 * sc, 0);
    endShape();

    popStyle();
}*/