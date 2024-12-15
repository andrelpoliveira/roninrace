using Unity.Netcode;
using UnityEngine;

public class ToriPortal : NetworkBehaviour
{
    /// <summary>
    /// Função RPC para o servidor
    /// </summary>
    [ServerRpc]
    public void DisablePortalServerRpc()
    {
        this.gameObject.SetActive(false);
    }
}
