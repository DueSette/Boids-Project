using UnityEngine;

public class PredatorSeek : SteeringBehaviour
{
    [SerializeField] BirdScriptableSettings settings;
    private float timeBeforeTargetSwitch = 0;
    [SerializeField] float timeAllotedToKillTarget = 4f;

    protected override void Start()
    {
        base.Start();
        AcquireTarget();
    }

    void AcquireTarget()
    {
        timeBeforeTargetSwitch = timeAllotedToKillTarget; //we give a limited amount of time before searching for another quarry

        for (int i = 0; i < BirdManager.birdList.Count; i++)
        {
            Bird b = BirdManager.birdList[i];
            if (b == null)
                continue; //we need this because if the reference to the bird object is destroyed as this is executing it might throw a NullReference

            if(quarry == null)
                quarry = b.transform;

            if ((b.transform.position - transform.position).sqrMagnitude < (quarry.position - transform.position).sqrMagnitude)
                quarry = b.transform;
        }

        if (quarry == null)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
        timeBeforeTargetSwitch -= Time.deltaTime;
        if (timeBeforeTargetSwitch <= 0f)
            AcquireTarget();

        // Get the target position from the mouse input
        if (quarry == null)
            AcquireTarget();

        // We need to check again if a target is found, if it is, then find the desired direction and speed
        if (quarry == null)      
            return Vector3.zero;         
        

        desiredVelocity = Vector3.Normalize(quarry.position - transform.position) * steeringAgent.MaxSpeed;

        if (IsOnCollisionCourse(steeringAgent))
            desiredVelocity += FindClearPath() * settings.predatorCollAvoidWeight;
               
		steeringVelocity = desiredVelocity;
		return steeringVelocity;
	}

    bool IsOnCollisionCourse(SteeringAgent birb)
    {
        {
            if (Physics.SphereCast(transform.position, settings.predatorSphereBoundsRadius, birb.CurrentVelocity.normalized, out RaycastHit hit, settings.predatorCollAvoidDistance, settings.obstacleMask))
                return true;
            return false;
        }
    }

    Vector3 FindClearPath()
    {
        Vector3[] rayDirections = FlockingUtilities.allDirections;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = transform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(transform.position, dir);
            if (!Physics.SphereCast(ray, settings.boundsRadius, settings.collAvoidDistance, settings.obstacleMask))
            {
                return dir;
            }
        }
        return transform.forward;
    }
}
