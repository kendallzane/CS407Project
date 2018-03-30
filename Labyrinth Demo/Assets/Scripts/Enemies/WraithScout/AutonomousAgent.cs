using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AutonomousAgent {
    public float pathRadius;                    //radius of the from the path to follow that can be deviated
    public float arrivalRadius;                 //size of the radius to determine an entity arriving at a destination
    public float wanderRadius;                  //size of the radius of thee circle for random points to wander to when idle.
    [HideInInspector ]public Vector2 wanderPoint;                //last wanter point generated
    public bool wanderTrigger;                  //flag to get another wander point
    public float maxSpeed;                      //terminal velocity of entity
    public float maxForce;                      //maximum force that can be applied to the entity

    public AutonomousAgent(float path_radius, float arrival_radius, float wander_radius, float max_speed, float max_force) 
    {
        pathRadius = path_radius;
        arrivalRadius = arrival_radius;
        wanderRadius = wander_radius;
        wanderTrigger = true;
        wanderPoint = new Vector2(0, 0);
        maxSpeed = max_speed;
        maxForce = max_force;
    }
}

[System.Serializable]
public class Path
{
    public List<Vector2> points;
    public float radius;

    public Path(Vector2 s, Vector2 e, float r)
    {
        points = new List<Vector2> { s, e };
        radius = r;
    }

    public Path(List<Vector2> ps, float r)
    {
        points = ps;
        radius = r;
    }

    public Path(List<GameObject> waypoints, float r)
    {
        Debug.Log("waypoints.count = " + waypoints.Count.ToString());
        points = new List<Vector2>();
        for (int i = 0; i < waypoints.Count; i++)
        {
            points.Add(new Vector2(waypoints[i].transform.position.x, waypoints[i].transform.position.y));
        }
        radius = r;
    }
}
