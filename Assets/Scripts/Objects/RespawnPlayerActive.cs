using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayerActive : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //other.GetComponent<CharacterMovement>().enabled = true;
        }
    }
}
