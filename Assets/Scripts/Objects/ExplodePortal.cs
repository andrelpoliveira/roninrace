using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ExplodePortal : NetworkBehaviour
{
    
    //Objetos de cena
    public GameObject[] barreiraP;
    //Objetos de efeito
    public GameObject explodeVfx;
    //Variáeis de controle
    public int boxCol;
    public int randomNumber;
    public Vector2 randomCol;
    private List<GameObject> explosionVfx = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        boxCol = 0;
        randomNumber = (int)Random.Range(randomCol.x, randomCol.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (boxCol >= randomNumber)
        {
            ExplodeVfxServerRpc();
            boxCol = 0;
        }
    }

    /////////////////////////////////////////Funções RPC para o servidor////////////////////////////////////
    /// <summary>
    /// Spawn do Objeto na cena no servidor
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void ExplodeVfxServerRpc()
    {
        GameObject spawnVfx = Instantiate(explodeVfx, transform.position, transform.rotation);
        explosionVfx.Add(spawnVfx);
        spawnVfx.GetComponent<ExplosionP>().parent = this;
        spawnVfx.GetComponent<NetworkObject>().Spawn();
        for (int i = 0; i < barreiraP.Length; i++)
        {
            barreiraP[i].GetComponent<ToriPortal>().DisablePortalServerRpc();
        }
    }
    /// <summary>
    /// Destroy o objeto da cena no servidor
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {
        GameObject toDestroy = explosionVfx[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        explosionVfx.Remove(toDestroy);
        Destroy(toDestroy, 2f);
        for (int i = 0;i < barreiraP.Length; i++)
        {
            barreiraP[i].GetComponent<NetworkObject>().Despawn();
            Destroy(barreiraP[i], 2f);
        }
        Destroy(gameObject);
    }
}
