using UnityEngine;

public class Wander : SteeringBehaviour
{
    /// <summary>
    /// Controls how large the imaginary circle is
    /// </summary>
    [SerializeField]
    protected float circleRadius = 100.0f;

    /// <summary>
    /// Controls how far from the agent position should the centre of the circle be
    /// </summary>
    [SerializeField]
    protected float circleDistance = 50.0f;
    private Vector3 point;
    [SerializeField] float refreshStep = 3.0f;
    [SerializeField] private float refreshTimer;
    [SerializeField] private float maxClosenessToPoint = 10;

    void OnEnable()
    {
        PickPoint();
    }

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
        //UpdateTimer();
        if((point - transform.position).sqrMagnitude < maxClosenessToPoint * maxClosenessToPoint)
            PickPoint();

        desiredVelocity = Vector3.Normalize(point - transform.position) * steeringAgent.MaxSpeed;
        steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;

        return steeringVelocity;
	}

    private void PickPoint()
    {
        refreshTimer = refreshStep;

           Vector3 target = quarry.position;

        //the direction where we should create our circle
        Vector3 directionToTarget = (target - transform.position).normalized;

        //pick the point we should move towards, just offset the direction
        Vector3 tempPoint = directionToTarget * circleDistance;

        point = new Vector3(transform.position.x + tempPoint.x + Random.Range(-circleRadius/2, circleRadius / 2), 
            tempPoint.y + transform.position.y + Random.Range(-circleRadius / 2, circleRadius / 2),
            0); //z axis is flat
        print(point);
    }
    /*
    private void UpdateTimer()
    {
        if(refreshTimer >= 0) { refreshTimer -= Time.deltaTime; return; }

        PickPoint();
    }
    */
}
