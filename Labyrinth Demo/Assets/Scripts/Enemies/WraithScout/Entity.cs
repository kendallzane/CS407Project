using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : Destructable {

    public AutonomousAgent aaParams = new AutonomousAgent(0.0f, 0.0f, 0.0f, 0.0f, 0.0f);     
    public float minSpeedAnimationChange = 0.5f;                        //minimum speed needed for the animation to change
    [HideInInspector] public bool isEnabled = true;                     //flag to enable entity
    [HideInInspector] public bool invulnerable = false;                 //flag for if the entity can be damaged
    [HideInInspector] public bool animationMovementOverride = false;    //flag telling all Entities that movement is being controller by an animation or animation event
    [HideInInspector] protected EDir direction = EDir.Down;             //enum to denote direction Entity is facing
    [HideInInspector] protected bool attacking = false;                 //flag denoting if the entity is attacking
    [HideInInspector] protected EStateID currentState = EStateID.Idle;  //state of the finite state machine
    protected Vector2 plannedDirection = new Vector2(0, 0);

    #region Finite State Machine
    protected virtual void FSM()
    {

    }
    #endregion

    #region Animation Relative
    protected virtual void SetDirection()
    {
        bool xLarger = Mathf.Abs(plannedDirection.x) > Mathf.Abs(plannedDirection.y);
        if (plannedDirection.x > minSpeedAnimationChange && xLarger)
        {
            direction = EDir.Right;
        }
        else if (plannedDirection.x < -minSpeedAnimationChange && xLarger)
        {
            direction = EDir.Left;
        }
        else if (plannedDirection.y > minSpeedAnimationChange && !xLarger)
        {
            direction = EDir.Up;
        }
        else if (plannedDirection.y < -minSpeedAnimationChange && !xLarger)
        {
            direction = EDir.Down;
        }
    }

    protected virtual void CheckFlipSprite(EDir dir)
    {
        //animation stuff
        if (direction == dir)
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
    }

    protected virtual void Hurt()
    {

    }

    protected virtual void Death()
    {

    }

    protected virtual void Moving()
    {

    }
    #endregion

    #region Autonomous Agent Behaviors
    protected virtual Vector2 AASeek(Vector3 targetLocation)
    {
        targetLocation.z = 0;
        Vector3 startLocation = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        Vector2 desired = (targetLocation - startLocation).normalized * aaParams.maxSpeed;
        plannedDirection = desired;
        Vector2 steering = (desired - rb.velocity).normalized * aaParams.maxForce;
        return steering;
    }

    protected virtual Vector2 AAArrive(Vector3 targetLocation)
    {
        targetLocation.z = 0;
        Vector3 startLocation = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        float arrivalRatio = 1.0f;
        if ((targetLocation - startLocation).magnitude < aaParams.arrivalRadius)
        {
            arrivalRatio = ((targetLocation - startLocation).magnitude / aaParams.arrivalRadius);
        }
        Vector2 desired = (targetLocation - startLocation).normalized * aaParams.maxSpeed * arrivalRatio;
        plannedDirection = desired;
        Vector2 steering = (desired - rb.velocity).normalized * aaParams.maxForce;
        return steering;
    }

    protected virtual Vector2 AAFollow(Path targetPath)
    {
        Vector2 currentLoc = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 predictLoc = currentLoc + (rb.velocity * Time.deltaTime);
        float distanceOffPath = 10000000f;
        Vector2 target = new Vector2(0, 0);
        int indexUsed = 1;
        for (int i = 0; i < targetPath.points.Count - 1; i++)
        {
            Vector2 a = targetPath.points[i];
            Vector2 b = targetPath.points[i + 1];

            Vector2 unitDir = (b - a).normalized;
            Vector2 normalPoint = a + unitDir * Vector2.Dot(unitDir, predictLoc); //unit vector * scalar projection
            if (((normalPoint.x < Mathf.Min(a.x, b.x) || normalPoint.x > Mathf.Max(a.x, b.x)))
                || ((normalPoint.y < Mathf.Min(a.y, b.y) || normalPoint.y > Mathf.Max(a.y, b.y))))
            {
                normalPoint = b;
            }
            Vector2 alongPath = (b - a).normalized * (rb.velocity.magnitude + 1) * 0.5f;
            float distance = Vector2.Distance(predictLoc, normalPoint);
            if (distance < distanceOffPath)
            {
                indexUsed = i + 1;
                distanceOffPath = distance;
                target = normalPoint + alongPath * (rb.velocity.magnitude + 1) * 0.5f;
            }
        }
        Vector2 steering = new Vector2(0, 0);
        if (distanceOffPath > targetPath.radius)
        {
            steering = AASeek(new Vector3(target.x, target.y, 0));
        } 
        if (Vector2.Distance(currentLoc, targetPath.points[indexUsed]) < aaParams.arrivalRadius && indexUsed + 1 < targetPath.points.Count) {
            steering = AASeek(new Vector3(targetPath.points[indexUsed + 1].x, targetPath.points[indexUsed + 1].y, 0));
            targetPath.points.RemoveAt(indexUsed - 1);
        }
        return steering;
    }

    protected virtual Vector2 AAWander()
    {
        if (aaParams.wanderTrigger)
        {
            float angle = Random.Range(0, Mathf.PI * 2);
            aaParams.wanderPoint = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * aaParams.wanderRadius;
            aaParams.wanderTrigger = false;
        }
        return AAArrive(aaParams.wanderPoint);   
    }
    #endregion  
}
