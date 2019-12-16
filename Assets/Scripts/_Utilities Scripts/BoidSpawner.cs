using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [SerializeField] int birdsToSpawn;
    [SerializeField] GameObject birdPrefab;
    [SerializeField, Tooltip("The radius of the sphere where every boid will be spawned")] float spawnSphereRadius;

    void Awake()
    {
        //Spawns and positions the bird
        for(int i = 0; i < birdsToSpawn; i++)
        {
            GameObject boid = Instantiate(birdPrefab, transform);
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnSphereRadius;
            boid.transform.position = pos;
            Vector3 rot = Vector3.zero;
            boid.transform.rotation = Quaternion.Euler(rot.RandomVector());
        }
    }

    //Allows us to view the size of the spawn sphere
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnSphereRadius);
    }
}
