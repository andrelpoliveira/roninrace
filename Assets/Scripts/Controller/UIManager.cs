using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Paineis")]
    public GameObject[] paineisGame;
    [Space]
    [Header("Teste")]
    public TMP_Text tmptesteId;


    #region Unity Methods
    void Start()
    {
        SwitchPanels(0);
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
    }

    public void StatsPlayer(string userId)
    {
        tmptesteId.color = Color.red;
        tmptesteId.text = userId;
        StartCoroutine(ClearInfo());
    }

    IEnumerator ClearInfo()
    {
        yield return new WaitForSeconds(2f);
        tmptesteId.text = "";
    }
    #endregion
}
