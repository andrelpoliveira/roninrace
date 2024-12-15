using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ExplosionP : NetworkBehaviour
{
    public ExplodePortal parent;

    private void Start()
    {
        if (!IsOwner) { return; }

        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1);
        parent.DestroyServerRpc();
    }

}
