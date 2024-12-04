using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    [Header("Paineis")]
    public GameObject[] paineis;
    public GameObject[] loginPanels;
    public Image bgSettings;
    [Space]
    [Header("Componentes de UI")]
    public Button btnSound;
    [Space]
    [Header("Controle de cena")]
    public int _scene;
    [Space]
    [Header("Componentes do Profile")]
    public RawImage imgPicProfile;
    public TMP_Text tmpUsername;
    public TMP_Text tmpEmail;

    #region Unity Methods
    private void Start()
    {

    }
    #endregion

    #region My Methods

    /////////////////////////////////////////Paineis de entrada do game//////////////////////////////////

    /// <summary>
    /// Alterna entre os painéis de menu do game
    /// index 0 - Painel Join
    /// index 1 - Painel Configurações
    /// index 2 - Painel Volume
    /// index 3 - Painel Idioma
    /// index 4 - Painel de Escolha de Login
    /// </summary>
    /// <param name="indexPanel"></param>
    public void SwitchPanels(int indexPanel)
    {
        switch (indexPanel)
        {
            case 0:
                //
                break;
            case 1:
                OpenSettings();
                break;
            case 2:
                OpenSoundPanel();
                break;
            case 3:
                OpenLanguagePanel();
                break;
            case 4:
                OpenLoginPanel();
                break;
        }
    }
    /// <summary>
    /// Abre painel de settings
    /// </summary>
    public void OpenSettings()
    {
        paineis[1].SetActive(true);
        OpenSoundPanel();
    }
    /// <summary>
    /// Abre o painel de Controle de volume
    /// </summary>
    public void OpenSoundPanel()
    {
        bgSettings.color = new Color(0f, 0.6f, 0.9450981f, 0.6f);
        paineis[3].SetActive(false);
        paineis[2].SetActive(true);
    }
    /// <summary>
    /// Abre o painel de idiomas
    /// </summary>
    public void OpenLanguagePanel()
    {
        bgSettings.color = new Color(0.9960785f, 0.2666667f, 0f, 0.6f);
        paineis[2].SetActive(false);
        paineis[3].SetActive(true);
    }
    /// <summary>
    /// Abre o painel de Login para escolha do jogador
    /// </summary>
    public void OpenLoginPanel()
    {
        paineis[4].SetActive(true);
        SwitchLoginPanels(0);
    }

    ///////////////////////////////////Paineis de Login do game//////////////////////////////////////////

    /// <summary>
    /// Alterna entre os paineís de registro e login do game
    /// index 0 - Painel de escolha de login
    /// index 1 - Painel de login por email
    /// index 2 - Painel de profile do google
    /// index 3 - Painel de registro por email
    /// </summary>
    /// <param name="indexPanelsLogin"></param>
    public void SwitchLoginPanels(int indexPanelsLogin)
    {
        switch (indexPanelsLogin)
        {
            case 0:
                OpenMultipleLogin();
                break;
            case 1:
                OpenEmailLogin();
                break;
            case 2:
                OpenProfileGoogle();
                break;
            case 3:
                OpenEmailRegister();
                break;
            case 4:
                OpenForgetPassword();
                break;
        }
    }
    /// <summary>
    /// Abre o painel de escolha de login
    /// </summary>
    public void OpenMultipleLogin()
    {
        DisableLoginPanels();
        loginPanels[0].SetActive(true);
    }
    /// <summary>
    /// Abre o painel de Login por email
    /// </summary>
    public void OpenEmailLogin()
    {
        DisableLoginPanels();
        loginPanels[1].SetActive(true);
    }
    /// <summary>
    /// Abre o painel de Profile do google
    /// </summary>
    public void OpenProfileGoogle()
    {
        DisableLoginPanels();
        loginPanels[2].SetActive(true);
    }
    /// <summary>
    /// Abre o painel de registro por email
    /// </summary>
    public void OpenEmailRegister()
    {
        DisableLoginPanels();
        loginPanels[3].SetActive(true);
    }
    /// <summary>
    /// Abre o painel de recuperação de senha
    /// </summary>
    public void OpenForgetPassword()
    {
        DisableLoginPanels();
        loginPanels[4].SetActive(true);
    }
    /// <summary>
    /// Desativa os paineis de login 
    /// </summary>
    public void DisableLoginPanels()
    {
        for (int i = 0; i < loginPanels.Length; i++)
        {
            loginPanels[i].SetActive(false);
        }
    }
    /// <summary>
    /// Inicia o game
    /// </summary>
    public void StartGameMultiplayer()
    {
        SceneManager.LoadScene(_scene, LoadSceneMode.Single);
    }
    #endregion
}
