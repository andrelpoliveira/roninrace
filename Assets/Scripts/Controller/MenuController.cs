using Game;
using Network;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controller
{
    public class MenuController : MonoBehaviour
    {
        [Header("Paineis")]
        public GameObject[] _panels;
        public GameObject _bgMenuGame;
        [Space]
        [Header("Botões")]
        public Button btnStart;
        public Button btnSettings;
        public Button btnCloseSettings;
        public Button btnCloseLogin;
        public Button btnLoginUnity;
        public Button btnSaveUsername;
        public Button btnContinueGame;
        public Button btnLogout;
        [Space]
        [Header("Componentes de textos")]
        public TMP_Text tmpWarningGame;
        public TMP_Text tmpWarningLogin;
        public TMP_Text tmpUserId;
        public GameObject tmpInputUserName;
        public TMP_Text tmpUsername;
        [Space]
        [Header("Scripts")]
        public AuthenticationController _authController;
        public CloudSaving _cloudSaving;
        [Tooltip("Variáveis privadas para controle")]
        private string _playeName;

        #region Unity Methods

        private void OnEnable()
        {
            SwitchPanel(0);
            StartGame();
            btnStart.onClick.AddListener(() => SwitchPanel(2));
            btnSettings.onClick.AddListener(() => SwitchPanel(1));
            btnCloseSettings.onClick.AddListener(() => SwitchPanel(0));
            btnCloseLogin.onClick.AddListener(() => SwitchPanel(0));
            btnLoginUnity.onClick.AddListener(LoginButtonPressed);
            btnContinueGame.onClick.AddListener(LoadingGame);
            btnSaveUsername.onClick.AddListener(SwitchButtonStartSave);
            btnLogout.onClick.AddListener(LogOut);
            _authController.OnSignedIn += AuthControler_OnSignedIn;

        }
        private void OnDisable()
        {
            btnStart.onClick.RemoveAllListeners();
            btnSettings.onClick.RemoveAllListeners();
            btnCloseSettings.onClick.RemoveAllListeners();
            btnCloseLogin.onClick.RemoveAllListeners();
            btnLoginUnity.onClick.RemoveAllListeners();
            btnContinueGame.onClick.RemoveAllListeners();
            btnSaveUsername.onClick.RemoveAllListeners();
            btnLogout.onClick.RemoveAllListeners();
            _authController.OnSignedIn -= AuthControler_OnSignedIn;
        }
        #endregion

        #region My Methods
        /// <summary>
        /// Start Game
        /// </summary>
        void StartGame()
        {
            _bgMenuGame.SetActive(true);
            tmpWarningGame.text = "";
            tmpWarningLogin.text = "";
            _authController = FindAnyObjectByType<AuthenticationController>();
            _cloudSaving = FindAnyObjectByType<CloudSaving>();
        }
        /// <summary>
        /// Função para os botões que irão controla os menus
        /// index 0 - Panel Start
        /// index 1 - Panel Settings
        /// index 2 - Panel Login
        /// index 3 - Panel Profile
        /// </summary>
        /// <param name="panelIndex"></param>
        public void SwitchPanel(int panelIndex)
        {
            ClearPanels();
            switch (panelIndex) 
            {
                case 0:
                    _panels[panelIndex].SetActive(true);
                    break;
                case 1:
                    _panels[panelIndex].SetActive(true);
                    break;
                case 2:
                    _panels[panelIndex].SetActive(true);
                    break;
                case 3:
                    _panels[panelIndex].SetActive(true);
                    btnCloseLogin.gameObject.SetActive(false);
                    
                    if (_cloudSaving.username != "")
                    {
                        tmpInputUserName.SetActive(false);
                        tmpUserId.text = _cloudSaving.username;
                        btnSaveUsername.gameObject.SetActive(false);
                        btnContinueGame.gameObject.SetActive(true);
                    }
                    else
                    {
                        tmpUserId.text = _playeName;
                        btnSaveUsername.gameObject.SetActive(true);
                        btnContinueGame.gameObject.SetActive(false);
                    }
                    break;
            }
        }
        /// <summary>
        /// Função para seligar todos os paineis ativos
        /// </summary>
        void ClearPanels()
        {
            for (int i = 0; i < _panels.Length; i++)
            {
                _panels[i].SetActive(false);
            }
        }

        public void ActivateWarningLogin(string error)
        {
            tmpWarningLogin.color = Color.red;
            tmpWarningLogin.text = error;
            StartCoroutine(ClearWarnings());
        }

        private async void LoginButtonPressed()
        {
            await _authController.InitSignIn();
        }

        private void AuthControler_OnSignedIn(PlayerInfo playerInfo, string playerName)
        {
            _playeName = playerName;

            StartCoroutine(WaitLoginResponse());
        }
        /// <summary>
        /// Função para o botão de logout
        /// </summary>
        public void LogOut()
        {
            _authController.SignOut();
            SwitchPanel(0);
        }
        /// <summary>
        /// Controle dos botões de alteração de nome de usuário e continue do game
        /// </summary>
        public void SwitchButtonStartSave()
        {
            if(tmpUsername.text != "")
            {
                _cloudSaving.username = tmpUsername.text;
                _cloudSaving.userLevel = 0;
                _cloudSaving.userCoin = 0;
                _cloudSaving.userDiamond = 0;
                _cloudSaving.SaveData();
                btnSaveUsername.gameObject.SetActive(false);
                btnContinueGame.gameObject.SetActive(true);
            }
            else
            {
                tmpWarningGame.color = Color.red;
                tmpWarningGame.text = "Invalid username";
            }
        }
        /// <summary>
        /// Botão para iniciar o game
        /// </summary>
        public void LoadingGame()
        {
            StartCoroutine(StartLobbyGame());
            SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
        }
        ///////////////////////////////////////Corrotinas///////////////////////////////////////////////////
        /// <summary>
        /// Corrotina para limpar os campos de alerta e informação
        /// </summary>
        /// <returns></returns>
        IEnumerator ClearWarnings()
        {
            yield return new WaitForSecondsRealtime(2f);
            tmpWarningGame.text = "";
            tmpWarningLogin.text = "";
        }
        /// <summary>
        /// Corrotina para aguardar o Load da informações da Cloud
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitLoginResponse()
        {
            Task task = _cloudSaving.LoadData();
            yield return new WaitForSecondsRealtime(1f);
            SwitchPanel(3);
        }
        /// <summary>
        /// Corrotina para mudar para a tela do lobby
        /// </summary>
        /// <returns></returns>
        IEnumerator StartLobbyGame()
        {
            ClearPanels();
            yield return new WaitForSeconds(3f);
            SceneManager.UnloadSceneAsync("Loading");
            OnHostLoad();  
        }
        /// <summary>
        /// Sincroniza a tela do lobby
        /// </summary>
        void OnHostLoad()
        {
            SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Single);
        }
        #endregion
    }
}

