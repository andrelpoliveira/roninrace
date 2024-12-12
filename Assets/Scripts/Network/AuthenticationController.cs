using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;
using Unity.Services.Authentication.PlayerAccounts;
using System.Configuration;
using System.Text;
using Controller;

namespace Network
{
    public class AuthenticationController : MonoBehaviour
    {
        [Tooltip("Events")]
        public event Action<PlayerInfo, string> OnSignedIn;
        [Tooltip("Scripts")]
        MenuController _menuController;
        [Tooltip("Variáveis")]
        string m_ExternalIds;
        private PlayerInfo playerInfo;

        #region Unity Methods
        async void Awake()
        {
            await UnityServices.InitializeAsync();
            PlayerAccountService.Instance.SignedIn += SignInWithUnity;
        }
        #endregion

        #region My Methods
        /// <summary>
        /// Sistema de login pela unity
        /// </summary>
        private async void SignInWithUnity()
        {
            try
            {
                var accessToken = PlayerAccountService.Instance.AccessToken;

                await SignInWithUnityAsync(accessToken);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        /// <summary>
        /// Função para instanciar o sistema de login
        /// </summary>
        /// <returns></returns>
        public async Task InitSignIn()
        {
            await PlayerAccountService.Instance.StartSignInAsync();
        }

        async Task SignInWithUnityAsync(string accessToken)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);
                Debug.Log("SignIn is successful.");

                playerInfo = AuthenticationService.Instance.PlayerInfo;

                var name = await AuthenticationService.Instance.GetPlayerNameAsync();

                OnSignedIn?.Invoke(playerInfo, name);
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }

        void OnDestroy() 
        {
            PlayerAccountService.Instance.SignedIn -= SignInWithUnity;
        }
        /// <summary>
        /// Sistema de logout pela unity
        /// </summary>
        public void SignOut()
        {
            AuthenticationService.Instance.SignOut();
            PlayerAccountService.Instance.SignOut();
            OnDestroy();
            Debug.Log("Signed Out successfully");
        }
        #endregion
    }
}

