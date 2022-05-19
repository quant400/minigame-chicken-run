using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonTeleporter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 pos =new Vector3 (Random.Range(-95, 130), 40, Random.Range(-85, 120));
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = pos;
            other.GetComponent<CharacterController>().enabled = true;

        }
    }
}
