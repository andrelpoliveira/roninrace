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
    //public BasicSpawner spawner;
    public UIManager _uiManager;

    private void Start()
    {
        //spawner = FindObjectOfType<BasicSpawner>();
        _uiManager = FindObjectOfType<UIManager>();
        ClearInfos();
    }

    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G))
            BtnStatsPlayer();
    }

    public void ClearInfos()
    {
        _uiManager.StatsPlayer(userId);
        //_uiManager = null;
        //Destroy(_infoJogador.gameObject);
    }

    public void BtnStatsPlayer()
    {
        _uiManager.StatsPlayer(userId);
    }
}
