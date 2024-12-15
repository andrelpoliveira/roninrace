using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //other.GetComponent<CharacterMovement>().enabled = false;
            //other.transform.position = respawnPoint.position;
        }
    }
}
