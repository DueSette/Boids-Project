using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
	[SerializeField]
	protected float maxSpeed = 20.0f;

	public float MaxSpeed
	{
		get
		{
			return maxSpeed;
		}
	}

	[SerializeField]
	protected float maxSteering = 10.0f;

	public float MaxSteering
	{
		get
		{
			return maxSteering;
		}
	}

	[SerializeField] public Vector3 CurrentVelocity
	{
		get;
		protected set;
	}

    private List<SteeringBehaviour> steeringBehaviours = new List<SteeringBehaviour>();

    private void FixedUpdate()
	{
		CooperativeArbitration();
		UpdatePosition();
		UpdateDirection();
	}

	protected virtual void CooperativeArbitration()
	{
		bool showDebugLines = false;
		Vector3 steeringVelocity = Vector3.zero;
		
		GetComponents<SteeringBehaviour>(steeringBehaviours);
		foreach (SteeringBehaviour currentBehaviour in steeringBehaviours)
		{
			if(currentBehaviour.enabled)
			{
				steeringVelocity += currentBehaviour.UpdateBehaviour(this);
				showDebugLines |= currentBehaviour.ShowDebugLines;
			}
		}

		// Debug lines in scene view
		if (showDebugLines)
		{
			//Debug.DrawRay(transform.position, CurrentVelocity, Color.green);
			foreach (SteeringBehaviour currentBehaviour in steeringBehaviours)
			{
				if (currentBehaviour.enabled)
				{
					currentBehaviour.DebugDraw();
				}
			}
		}

		// Set final velocity
		CurrentVelocity += LimitSteering(steeringVelocity, maxSteering);
		CurrentVelocity = LimitVelocity(CurrentVelocity, maxSpeed);
	}

	/// <summary>
	/// Updates the position of the GAmeObject via Teleportation. In Craig Reynolds architecture this would the Locomotion layer
	/// </summary>
	protected virtual void UpdatePosition()
	{
		transform.position += CurrentVelocity * Time.deltaTime;
	}

	/// <summary>
	/// Sets the direction of the triangle to the direction it is moving in to give the illusion it is turning. Trying taking out the function
	/// call in Update() to see what happens
	/// </summary>
	protected virtual void UpdateDirection()
	{
		// Don't set the direction if no direction
		if (CurrentVelocity.sqrMagnitude > 0.0f)
		{
			transform.forward = Vector3.Normalize(new Vector3(CurrentVelocity.x, CurrentVelocity.y, 0.0f));
		}
	}

	#region Static Helper Functions
	/// <summary>
	/// Limits the velocity vector to the maxSpeed
	/// </summary>
	/// <param name="velocity">Velocity to limit</param>
	/// <param name="maxSpeed">Amount to limit to</param>
	/// <returns>New Vector that has been limited</returns>
	static public Vector3 LimitVelocity(Vector3 velocity, float maxSpeed)
	{
		// This limits the velocity to max speed. sqrMagnitude is used rather than magnitude as in magnitude a square root must be computed which is a slow operation.
		// By using sqrMagnitude and comparing with maxSpeed squared we can get around using the expensive square root operation.
		if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
		{
			velocity.Normalize();
			velocity *= maxSpeed;
		}
		return velocity;
	}

	/// <summary>
	/// Limits the steering vector to the maxSteering
	/// </summary>
	/// <param name="steeringVelocity">Steering velocity to limit</param>
	/// <param name="maxSteering">Amount to limit to</param>
	/// <returns>New Vector that has been limited</returns>
	static public Vector3 LimitSteering(Vector3 steeringVelocity, float maxSteering)
	{
		// This limits the velocity to max steering. sqrMagnitude is used rather than magnitude as in magnitude a square root must be computed which is a slow operation.
		// By using sqrMagnitude and comparing with maxSteering squared we can get around using the expensive square root operation.
		if (steeringVelocity.sqrMagnitude > maxSteering * maxSteering)
		{
			steeringVelocity.Normalize();
			steeringVelocity *= maxSteering;
		}
		return steeringVelocity;
	}
	#endregion
}
