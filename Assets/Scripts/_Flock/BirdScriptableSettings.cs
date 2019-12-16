using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BirdScriptableSettings : ScriptableObject
{
    [Range(0, 10)]
    public float cohesionWeight = 1;
    [Range(0, 10)]
    public float alignmentWeight = 1;
    [Range(0, 10)]
    public float separationWeight = 1;
    [Tooltip("Value that will be used to determine when another boid is considered part of the flock")]
    public float viewRadius = 25;
    [Tooltip("Value that will be used to determine when another boid is too close - the value input here is squared internally")]
    public float separationRadius = 10;

    [Header("Collisions")]
    public LayerMask obstacleMask;
    public string killTag;

    [Tooltip("Radius of the sphere that is cast to detect obstacles")]
    public float boundsRadius = 1f;

    [Range(0, 10)] public float avoidCollisionWeight = 3f;
    public float collAvoidDistance = 70;

    public enum CollisionCheckLogic { SphereCasting, OverlapSphere, ConeSphereCasting }
    public CollisionCheckLogic collisionCheckMode = CollisionCheckLogic.SphereCasting;

    [Header("Predator Escaping")]
    public LayerMask predatorMask;
    public float checkForPredatorRadius = 35f;
    [Range(0, 10)] public float predatorEscapeWeight = 3f;

    [Header("Predator Values")]
    public float predatorSphereBoundsRadius = 1.25f;
    public float predatorCollAvoidDistance = 25;
    [Range(0, 5)] public float predatorCollAvoidWeight = 1f;
    
}