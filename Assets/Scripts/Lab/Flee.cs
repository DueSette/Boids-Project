using UnityEngine;

public class Flee : SteeringBehaviour
{
	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
        Vector3 target;


        
            target = quarry.position;

        desiredVelocity = Vector3.Normalize(transform.position - target) * steeringAgent.MaxSpeed;

        // Calculate steering velocity and limit it according to how much it can turn
        steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;

        // Implement me
        return steeringVelocity;
	}
}
