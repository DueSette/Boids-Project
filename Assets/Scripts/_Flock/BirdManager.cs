using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    public static List<Bird> birdList = new List<Bird>();
    
    public static BirdManager Instance { get; private set; }
    
    void OnEnable()
    {
        Bird.NewBirdEvent += AddBirbToList;
    }

    void OnDisable()
    {
        Bird.NewBirdEvent -= AddBirbToList;
    }

    void AddBirbToList(Bird b)
    {
        birdList.Add(b);
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
