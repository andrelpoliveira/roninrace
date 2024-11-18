using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("Controle de conexão")]
    public bool connectOnAwake;
    [Tooltip("Fusion")]
    [HideInInspector]public NetworkRunner _networkRunner;

    [Header("Objeto Player")]
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [Header("Objeto Spawn")]
    public Transform _spawnPlayer;
    
    private void Awake()
    {
        if (connectOnAwake)
            StartSeason();
    }

    public async void ConnectToRunner(GameMode gameMode)
    {
        if(_networkRunner == null)
        {
            _networkRunner = gameObject.AddComponent<NetworkRunner>();
        }

        await _networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            SessionName = "Teste Ronin",
            PlayerCount = 5,
        });
    }
    /// <summary>
    /// Inicia como host
    /// </summary>
    public void StartSeason()
    {
        if(_networkRunner == null)
            ConnectToRunner(GameMode.Shared);
    }
    /// <summary>
    /// Entra em algum host já criado
    /// </summary>
    public void EnterSeason()
    {
        if(_networkRunner == null)
            ConnectToRunner(GameMode.Client);
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        NetworkObject playerObject = runner.Spawn(_playerPrefab,_spawnPlayer.position,Quaternion.identity);

        runner.SetPlayerObject(runner.LocalPlayer, playerObject);

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
       
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        /*if (runner.IsServer)
        {
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_prefab, spawnPosition, Quaternion.identity, player);

            _spawnedCharacters.Add(player, networkPlayerObject);
        }*/
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        /*if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }*/
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
       
    }
}
