using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            int portal = Random.Range(0, 2);
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = transform.GetChild(portal).position;
            other.GetComponent<CharacterController>().enabled = true;

        }
    }
}
