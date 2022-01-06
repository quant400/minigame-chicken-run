using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PowerUpMushroom : MonoBehaviour
{
    [SerializeField]
    float PowerupSpeedMultiplier, powerUpDuration;

    private void Start()
    {
        int chance = Random.Range(0 , 100);
        if (chance > SingleplayerGameControler.instance.GetMushroomPowerUpChance()) 
        {
           
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Light>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if(other.CompareTag("Player"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Light>().enabled = false;
            StartCoroutine( Powerup(other.gameObject));
        }
    }

    IEnumerator Powerup(GameObject player)
    {
        ThirdPersonController TPC = player.GetComponent<ThirdPersonController>();
        TPC.MoveSpeed *= PowerupSpeedMultiplier;
        TPC.SprintSpeed = TPC.MoveSpeed;
        
        yield return new WaitForSeconds(powerUpDuration);

        TPC.MoveSpeed /= PowerupSpeedMultiplier;
        TPC.SprintSpeed = TPC.MoveSpeed*1.5f;
        Destroy(gameObject);

    }


}
