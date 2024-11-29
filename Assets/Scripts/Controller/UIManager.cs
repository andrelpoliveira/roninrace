using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Paineis")]
    public GameObject[] paineisGame;
    [Space]
    [Header("Scripts")]
    public BasicSpawner basicSpawner;
    #region Unity Methods
    void Start()
    {
        SwitchPanels(0);
        basicSpawner = FindObjectOfType<BasicSpawner>();
    }
    #endregion

    #region My Methods
    /// <summary>
    /// Faz o switch entre os painéis
    /// </summary>
    /// <param name="indexPanel"></param>
    public void SwitchPanels(int indexPanel)
    {
        for (int i = 0; i < paineisGame.Length; i++)
        {
            paineisGame[i].SetActive(false);
        }
        paineisGame[indexPanel].SetActive(true);
    }
    /// <summary>
    /// Botão de seleção de personagem inicial
    /// </summary>
    /// <param name="indexPlayer"></param>
    public void ChosenPlayer(int indexPlayer)
    {
        basicSpawner._chosenPlayer = indexPlayer;
    }
    #endregion
}
