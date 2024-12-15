using Unity.Netcode;
using UnityEngine;

public class RequestAllPlayersName : MonoBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    public void RequestAllPlayerNamesServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        
    }
}
