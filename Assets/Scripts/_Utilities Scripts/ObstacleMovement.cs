using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float speed;
    enum DirectionState { Yaxis, Xaxis, Zaxis, NONE }
    [SerializeField] DirectionState dir = DirectionState.NONE;
    [SerializeField] bool oppositeDirection = false;

    // Update is called once per frame
    void Update()
    {
        float mov = Mathf.Sin(Time.time) * speed * Time.deltaTime;
        Vector3 v = Vector3.zero;

        switch (dir)
        {
            case DirectionState.Xaxis:
                {                
                    v = new Vector3(mov, 0, 0);
                    break;
                }
            case DirectionState.Yaxis:
                {
                    v = new Vector3(0, mov, 0);
                    break;
                }
            case DirectionState.Zaxis:
                {
                    v = new Vector3(0, 0, mov);
                    break;
                }
            default:
                break;
        }

        transform.position += oppositeDirection ? -v : v;
    }
}
