using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    [Header("Input Canvas")]
    public GameObject[] canvas;
    [Space]
    [Header("Panels")]
    public GameObject[] panelsGame;
    [Space]
    [Header("Buttons Canvas")]
    public Button startButton;
    [Space]
    [Tooltip("Panel Character")]
    [Header("TMP")]
    public TMP_Text tmpNameCharacter;
    public TMP_Text[] tmpNamesCharacters;
    [Tooltip("TMP Skins")]
    public TMP_Text tmpNameSkin;
    public TMP_Text tmpCostSkin;
    [Tooltip("TMP Talents")]
    public TMP_Text tmpTalentName;
    public TMP_Text tmpDescriptionTalent;
    public TMP_Text tmpCostTalent;
    [Tooltip("TMP Acessories")]
    public TMP_Text tmpNameAcessory;
    public TMP_Text tmpCostAcessory;
    [Space]
    [Header("Buttons")]
    public Button btnNext;
    public Button btnPrevious;
    [Space]
    [Header("Variáveis de controle")]
    public string[] nameCharacter;
    public GameObject[] characters;
    [Tooltip("Skins")]
    public string[] nameSkin;
    public int[] costSkin;
    [Tooltip("Acessory")]
    public string[] nameAcessory;
    public int[] costAcessory;
    [Tooltip("Talents")]
    public string[] talentName;
    public string[] talentDescription;
    public int[] costTalent;
    [Tooltip("Controle de personagem")]
    public int indexCharacter;
    [Tooltip("Controle de talentos")]
    public int indexTalent;
    [Tooltip("Controle de Skins e Acessórios")]
    public int indexSkin;
    public int indexAcessory;

    #region Unity Methods
    /// <summary>
    /// Função Start
    /// </summary>
    void Start()
    {
        StartGame(0);
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => StartGame(1));
    }

    #endregion

    #region My Methods
    /// <summary>
    /// Função de iniciar os canvas do game
    /// </summary>
    /// <param name="index"></param>
    public void StartGame(int index)
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].gameObject.SetActive(false);
        }
        canvas[index].gameObject.SetActive(true);

        for (int i = 0;i < tmpNamesCharacters.Length; i++)
        {
            tmpNamesCharacters[i].text = nameCharacter[i];
        }

        if (index == 1)
            SwitchPanels(0);
    }
    /// <summary>
    /// Função de controle dos painéis do game
    /// </summary>
    /// <param name="indexPanels"></param>
    public void SwitchPanels(int indexPanels)
    {
        for(int i = 0;i < panelsGame.Length; i++)
        {
            panelsGame[i].gameObject.SetActive(false);
        }
        panelsGame[indexPanels].gameObject.SetActive(true);

        if(indexPanels == 1)
        {
            SwitchNameCharacter(indexTalent);
            SwitchSkins(indexTalent);
            SwitchTalents(indexTalent);
        }
    }
    /// <summary>
    /// Função para trocar nomes e personagens na UI
    /// </summary>
    /// <param name="indexName"></param>
    public void SwitchNameCharacter(int indexName)
    {
        tmpNameCharacter.text = "";
        tmpNameCharacter.text = nameCharacter[indexName];
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].gameObject.SetActive(false);
        }
        characters[indexName].gameObject.SetActive(true);
        
        indexCharacter = indexName;
        indexTalent = 0;
        SwitchTalents(indexTalent);
    }
    /// <summary>
    /// Função para trocar nome e valor das skins
    /// </summary>
    /// <param name="indexSkins"></param>
    public void SwitchSkins(int indexSkins)
    {
        tmpNameSkin.text = "";
        tmpCostSkin.text = "";
        tmpNameSkin.text = nameSkin[indexSkins];
        tmpCostSkin.text = costSkin[indexSkins].ToString();
        HideSkins(indexSkins);
    }
    /// <summary>
    /// Função para trocar nome e valor dos acessórios
    /// </summary>
    /// <param name="indexAcessories"></param>
    public void SwitchAcessories(int indexAcessories)
    {
        tmpNameAcessory.text = "";
        tmpCostAcessory.text = "";
        tmpNameAcessory.text = nameAcessory[indexAcessories];
        tmpCostAcessory.text = costAcessory[indexAcessories].ToString();
        HideAcessory(indexAcessories);
    }
    /// <summary>
    /// Função de controle dos talentos de cada character
    /// 
    /// talent 0 = Double Jump
    /// talent 1 = Dash
    /// talent 2 = Trap
    /// </summary>
    /// <param name="indexTalents"></param>
    public void SwitchTalents(int indexTalents)
    {
        indexTalent += indexTalents;
        ResetTalentText();
        switch(indexTalent)
        {
            case 0:
                btnPrevious.interactable = false;
                btnNext.interactable = true;
                break;
            case 1:
                btnPrevious.interactable = true;
                btnNext.interactable = true;
                break;
            case 2:
                btnPrevious.interactable = true;
                btnNext.interactable = false;
                break;
        }
        tmpTalentName.text = talentName[indexTalent];
        tmpDescriptionTalent.text = talentDescription[indexTalent];
        tmpCostTalent.text = costTalent[indexTalent].ToString();
    }
    /// <summary>
    /// Limpa os campos de textos dos talentos
    /// </summary>
    void ResetTalentText()
    {
        tmpTalentName.text = "";
        tmpDescriptionTalent.text = "";
        tmpCostTalent.text = "";
    }
    /// <summary>
    /// Função que inicia as skins do personagem
    /// </summary>
    /// <param name="skins"></param>
    void HideSkins(int skins)
    {
        characters[indexCharacter].GetComponent<FindItensPlayer>().StartChildren();

        switch (skins)
        {
            case 0:
                characters[indexCharacter].GetComponent<FindItensPlayer>().itensPlayer[0].SetActive(true);
                characters[indexCharacter].GetComponent<FindItensPlayer>().itensPlayer[1].SetActive(false);
                indexSkin = 0;
                break;
            case 1:
                characters[indexCharacter].GetComponent<FindItensPlayer>().itensPlayer[0].SetActive(false);
                characters[indexCharacter].GetComponent<FindItensPlayer>().itensPlayer[1].SetActive(true);
                indexSkin = 1;
                break;
        }
    }
    /// <summary>
    /// Função que inicia os acessórios do personagem
    /// </summary>
    /// <param name="acessory"></param>
    void HideAcessory(int acessory)
    {
        characters[indexCharacter].GetComponent<FindItensPlayer>().StartChildren();

        switch (acessory)
        {
            case 0:
                characters[indexCharacter].GetComponent<FindItensPlayer>().itensPlayer[2].SetActive(true);
                characters[indexCharacter].GetComponent<FindItensPlayer>().itensPlayer[3].SetActive(false);
                indexAcessory = 2;
                break;
            case 1:
                characters[indexCharacter].GetComponent<FindItensPlayer>().itensPlayer[2].SetActive(false);
                characters[indexCharacter].GetComponent<FindItensPlayer>().itensPlayer[3].SetActive(true);
                indexAcessory = 3;
                break;
        }
    }
    //////////////////////////////////////////Botões de controle do modo de jogo///////////////////////////////////
    /// <summary>
    /// Botão start pareamento
    /// </summary>
    public void ButtonStartSeason()
    {
        GameManager gameManager = GameManager.instance;

        gameManager.characterIndex = indexCharacter;
        gameManager.characterSkinIndex = indexSkin;
        gameManager.characterAcessoryIndex = indexAcessory;
        
    }

    #endregion
}
