using UnityEngine;

public class Arrival : SteeringBehaviour
{
    /// <summary>
    /// Controls how far from the target position should the agent start to slow down
    /// </summary>
	[SerializeField]
	protected float arrivalRadius = 200.0f;

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
        Vector3 target;

            target = quarry.position;
        Vector3 desiredMovement = target - steeringAgent.transform.position;
        float distance = desiredMovement.magnitude;

        Vector3 steering;
        if (distance > 0)
        {
            desiredMovement.Normalize();
            if (distance < arrivalRadius)
                desiredMovement *= steeringAgent.MaxSpeed * (distance / arrivalRadius);
            else
                desiredMovement *= steeringAgent.MaxSpeed;

            steering = desiredMovement - steeringAgent.CurrentVelocity;
            return steering;
        }
        return Vector3.zero;
    }
}
