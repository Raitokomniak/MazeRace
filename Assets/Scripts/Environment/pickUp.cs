using UnityEngine;
using System.Collections;

public class pickUp : MonoBehaviour
{

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }

     }
}