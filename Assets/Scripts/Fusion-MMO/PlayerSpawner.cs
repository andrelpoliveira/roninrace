using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public NetworkPrefabRef playerPrefab;
    public Transform posPlayerSpawn;
    public Quaternion posPlayerRotation = Quaternion.identity;

    public NetworkRunner _networkRunner;

    public void PlayerJoined(PlayerRef player)
    {
        _networkRunner = FindObjectOfType<NetworkRunner>();
        if(_networkRunner != null)
        {
            //NetworkObject spawnedObject = _networkRunner.Spawn(playerPrefab, posPlayerSpawn, posPlayerRotation);
            Runner.Spawn(playerPrefab, posPlayerSpawn.position, posPlayerRotation);
            /*if (player == Runner.LocalPlayer)
            {
                Runner.Spawn(playerPrefab, posPlayerSpawn, Quaternion.identity);
            }*/
        }
        else
        {
            Debug.Log("NetworkRunner não encontrado!");
        }
        
    }
}
