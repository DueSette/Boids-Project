using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flocking))]

public class Bird : SteeringAgent
{
    public BirdScriptableSettings settings;

    public delegate void BirdAssignment(Bird thisBirb);
    public static event BirdAssignment NewBirdEvent;

    void Start()
    {
        NewBirdEvent(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(settings.killTag))
        {
            BirdManager.birdList.Remove(this);
            Destroy(gameObject);
        }
    }
}
