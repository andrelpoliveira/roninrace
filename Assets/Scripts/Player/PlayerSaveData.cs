using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData : MonoBehaviour
{
    [Header("variáveis de Info do jogador")]
    public string userId;
    public string username;
    [Space]
    [Header("Scripts")]
    public BasicSpawner spawner;
    public InfoFirestoreJogador _infoJogador;

    private void Start()
    {
        spawner = FindObjectOfType<BasicSpawner>();
        _infoJogador = FindAnyObjectByType<InfoFirestoreJogador>();
        userId = spawner._userId;
        username = spawner._username;
        ClearInfos();
    }

    public void ClearInfos()
    {
        spawner._userId = "";
        spawner._username = "";
        spawner = null;
        Destroy(_infoJogador.gameObject);
    }
}
