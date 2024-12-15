using Unity.Netcode;
using UnityEngine;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Core;

public class PlayerData : NetworkBehaviour
{
    public TMP_Text tmpUsername;

    private string localPlayerName;

    public bool IsNotService = false;

    private async void Start()
    {
        if (IsNotService)
            return;

        if (IsOwner)
        {
            localPlayerName = await LoadPlayerNameFromCloud();
            SyncPlayerNameServerRpc(localPlayerName);
        }
    }

    #region RPC Methods
    [ServerRpc]
    public void SyncPlayerNameServerRpc(string playerName, ServerRpcParams rpcParams = default)
    {
        SyncPlayerNameClientRpc(playerName, rpcParams.Receive.SenderClientId);
    }
    [ClientRpc]
    public void SyncPlayerNameClientRpc(string playerName, ulong clientId)
    {
        if(NetworkManager.Singleton.LocalClientId == clientId)
            UpdateNameDisplay(playerName);
    }
    #endregion

    #region My Methods
    private void UpdateNameDisplay(string name)
    {
        if (tmpUsername != null)
            tmpUsername.text = name;
        else
            Debug.LogWarning("tmpUsername não foi atribuido no editor.");
    }
    #endregion

    #region Cloud Methods
    private async Task<string> LoadPlayerNameFromCloud()
    {
        try
        {
            var keys = new HashSet<string> { "username" };
            var result = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);
            if (result.TryGetValue("username", out var username))
            {
                Debug.Log($"Username encontrado {username.Value.GetAs<string>()}");
                return username.Value.GetAs<string>();
            }
            else
            {
                Debug.Log("Username não encontrado");
                return "DefaultPlayer";
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Erro ao carregar nome do jogador {ex}");
            return "ErrorPlayer";
        }
    }
    #endregion
}
