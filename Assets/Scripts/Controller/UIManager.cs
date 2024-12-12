using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    public class UIManager : NetworkBehaviour
    {
        [Header("Paineis")]
        public GameObject[] paineisGame;
        [Space]
        [Header("TMP Texts UI")]
        public TMP_Text tmpLobbyCode;
        public TMP_Text tmpLobbyName;
        [Space]
        [Header("Botões UI")]
        public Button btnHost;
        public Button btnJoin;
        [Space]
        [Header("Camera")]
        public GameObject _camScene;

        #region Unity Methods
        void Start()
        {
            _camScene.SetActive(true);
            SwitchPanels(0);

            //TesteReturnInfoPlayer();
        }

        private void OnEnable()
        {
            btnHost.onClick.AddListener(StartHost);
            btnJoin.onClick.AddListener(StartCliente);
        }
        private void OnDisable()
        {
            btnHost?.onClick.RemoveListener(StartHost);
            btnJoin?.onClick.RemoveListener(StartCliente);
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
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public void StatsPlayer(string userId)
        {
            //tmptesteId.color = Color.red;
            //tmptesteId.text = userId;
            StartCoroutine(ClearInfo());
        }

        public void TesteReturnInfoPlayer()
        {
            if (AuthenticationService.Instance.PlayerId != null)
                tmpLobbyName.text = $"id: {AuthenticationService.Instance.PlayerId}";
            else
                tmpLobbyName.text = "id: null";
        }
        /////////////////////////////////Funções do Multiplayer/////////////////////////////////////////////
        /// <summary>
        /// Incia o jogo como modo Host
        /// </summary>
        public void StartHost()
        {
            _camScene.SetActive(false);
            SwitchPanels(1);
            NetworkManager.Singleton.StartHost();
        }
        /// <summary>
        /// Inicia o jogo como modo cliente
        /// </summary>
        public void StartCliente()
        {
            _camScene.SetActive(false);
            SwitchPanels(1);
            NetworkManager.Singleton.StartClient();
        }
        ////////////////////////////////Corrotinas /////////////////////////////////////////////////////////
        /// <summary>
        /// Corrotina para limpar as informações da tela
        /// </summary>
        /// <returns></returns>
        IEnumerator ClearInfo()
        {
            yield return new WaitForSeconds(2f);
            //tmptesteId.text = "";
        }
        #endregion
    }
}

