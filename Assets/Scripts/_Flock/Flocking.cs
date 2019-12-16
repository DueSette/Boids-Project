using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : SteeringBehaviour
{
    Bird thisBirb;
    Collider[] nearbyPredators;

    public BirdScriptableSettings settings;
    [SerializeField] private List<Bird> flockMates;

    protected override void Start()
    {
        thisBirb = GetComponent<Bird>();
    }

    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
    {
        flockMates = GetPerceivedNumberOfFlockmates(thisBirb);
        Vector3 result = Vector3.zero;

        Vector3 cohesion = Vector3.zero;
        Vector3 separation = Vector3.zero;
        Vector3 alignment = Vector3.zero;

        Vector3 obstacleAvoidance = Vector3.zero;

        if (flockMates.Count > 0)
        {
            //each second statement in these lines is simply for debug purposes
             cohesion = CohesionSteering(thisBirb) * settings.cohesionWeight; 
             separation = SeparationSteering(thisBirb) * settings.separationWeight; 
             alignment = AlignmentSteering(thisBirb) * settings.alignmentWeight; 
        }      

        if (IsOnCollisionCourse(steeringAgent))
        {
            //dampens every other force by 15% - quicker than altering each flocking component individually
            cohesion *= 0.85f;
            alignment *= 0.85f;
            separation *= 0.85f;

            Vector3 obstacleAvoidanceDirection = FindClearPath();
            obstacleAvoidance = obstacleAvoidanceDirection * settings.avoidCollisionWeight;
            result += obstacleAvoidance;
        }

        Vector3 escapeFromPredatorSteer = EscapePredators(steeringAgent);

        if(escapeFromPredatorSteer != Vector3.zero) //if there is a predadtor, break up the flock (only cohesion and alignment)
        {
            cohesion *= -0.5f; //we dampen the value and make it negative, so the boids go away from one another and change direction
            alignment *= -0.5f;
            result += escapeFromPredatorSteer;
        }

        //lastly, add the three main flocking forces (done at the last step as these forces are influenced by more factors)
        result += cohesion + separation + alignment;

        //===Assigning Debug Variables
        cohesionVel = cohesion;
        separationVel = separation;
        alignmentVel = alignment;
        dvelTot = result;
        return result;
    }

    Vector3 CohesionSteering(Bird bird)
    { //unused value; in older iterations of this method you would need a reference to the Bird to obtain the flockmate list
        Vector3 perceivedCenterOfMass = Vector3.zero;

        foreach (Bird b in flockMates)        
            perceivedCenterOfMass += b.transform.position;
        
        perceivedCenterOfMass /= flockMates.Count;
        return ((perceivedCenterOfMass - transform.position) / 100);
    }

    Vector3 SeparationSteering(Bird bird)
    {
        Vector3 separation = Vector3.zero;

        foreach (Bird b in flockMates)       
            if (Vector3.SqrMagnitude(b.transform.position - bird.transform.position) < settings.separationRadius * settings.separationRadius)
                separation -= (b.transform.position - bird.transform.position);
        
        return separation;
    }

    Vector3 AlignmentSteering(Bird bird)
    {
        Vector3 perceivedVelocity = Vector3.zero;

        foreach (Bird b in flockMates)
             perceivedVelocity += b.transform.forward;

         return (perceivedVelocity / flockMates.Count) / 3; 
    }

    bool IsOnCollisionCourse(SteeringAgent bird)
    {
        switch(settings.collisionCheckMode)
        {
            case BirdScriptableSettings.CollisionCheckLogic.OverlapSphere:
                { //pretty bad and unrealistic looking
                    if (Physics.OverlapSphere(transform.position, settings.boundsRadius * 2, settings.obstacleMask).Length > 0)
                        return true;

                    return false;
                }

            case BirdScriptableSettings.CollisionCheckLogic.SphereCasting:
                {
                    if (Physics.SphereCast(transform.position, settings.boundsRadius, bird.CurrentVelocity.normalized, out RaycastHit hit, settings.collAvoidDistance, settings.obstacleMask))
                        return true;

                    return false;
                }

            case BirdScriptableSettings.CollisionCheckLogic.ConeSphereCasting:
                {
                    Vector3[] rayDirections = FlockingUtilities.frontDirections;

                    for (int i = 0; i < rayDirections.Length; i++)
                    {
                        Vector3 dir = transform.TransformDirection(rayDirections[i]);
                        
                        Debug.DrawRay(transform.position, dir);
                        if (Physics.SphereCast(transform.position, settings.boundsRadius / 2, dir, out RaycastHit hit, settings.collAvoidDistance, settings.obstacleMask))                       
                            return true;
                    }
                    return false;
                }

            default:
                return false;
        }
    }

    Vector3 FindClearPath()
    {
        Vector3[] rayDirections = FlockingUtilities.allDirections; //320 directions

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = transform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(transform.position, dir);

            //if we do NOT hit anything return this
            if (!Physics.SphereCast(ray, settings.boundsRadius, settings.collAvoidDistance, settings.obstacleMask))
            {
                return dir;
            }          
        }
        return transform.forward;
    }

    private Vector3 cohesionVel, separationVel, alignmentVel, dvelTot; //debug variables
    public override void DebugDraw()
    {
        Debug.DrawRay(transform.position, cohesionVel, Color.white);
        Debug.DrawRay(transform.position, separationVel, Color.blue);
        Debug.DrawRay(transform.position, alignmentVel, Color.red);
        Debug.DrawRay(transform.position, dvelTot, Color.black);
    }

    private List<Bird> GetPerceivedNumberOfFlockmates(Bird bird)
    {
        List<Bird> result = new List<Bird>();
        foreach (Bird b in BirdManager.birdList)
        {
            if (b != bird && IsInRange(b)) //If we are not currently operating on our own birb
            {
                result.Add(b);
            }
        }

        return result;

        bool IsInRange(Bird b)
        {
            if (b == null)
                return false;

            Vector3 dist = transform.position - b.transform.position;
            if (dist.sqrMagnitude < settings.viewRadius * settings.viewRadius)
                return true;
            return false;
        }
    }

    private Vector3 EscapePredators(SteeringAgent bird)
    {
        Vector3 result = Vector3.zero;
        nearbyPredators = Physics.OverlapSphere(transform.position, settings.checkForPredatorRadius, settings.predatorMask);

        foreach(Collider col in nearbyPredators)
        {
            Vector3 dir = Vector3.Normalize(transform.position - col.transform.position);
            result += dir * bird.MaxSpeed * settings.predatorEscapeWeight;
        }

        return result;
    }
}
