using Controller;
using UnityEngine;
using Unity.Netcode;

public class NPCLobby : NetworkBehaviour
{
    /// <summary>
    /// Paineis do Lobby
    /// index 0 - Join
    /// index 1 - Gameplay
    /// index 2 - Lobby Select
    /// index 3 - Lobby Wait
    /// </summary>

    [Header("Scripts")]
    public UIManager _uiManager;
    [Space]
    [Header("Número dos Paineis")]
    public int painelGameplay;
    public int painelNpc;

    #region Unity Methods
    private void Start()
    {
        _uiManager = FindAnyObjectByType<UIManager>();
    }
    #endregion
    #region My Methods
    /// <summary>
    /// Switch para o painel referente ao NPC
    /// </summary>
    public void LobbySwitch()
    {
        _uiManager.SwitchPanels(painelNpc);
    }
    /// <summary>
    /// Switch para o painel da GamePlay
    /// </summary>
    public void CloseLobby()
    {
        _uiManager.SwitchPanels(painelGameplay);
    }
    #endregion
}
