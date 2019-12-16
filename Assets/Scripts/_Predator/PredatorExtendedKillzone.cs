using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorExtendedKillzone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bird"))
            Destroy(other.gameObject);
    }
}
