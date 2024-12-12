using Unity.Services.Core;
using UnityEngine;
using System;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Network
{
    public class CloudSaving : MonoBehaviour
    {
        [Header("Variáveis de save do usuário")]
        public string username;
        public int userLevel;
        public int userCoin;
        public int userDiamond;

        #region Unity Methods

        #endregion

        #region My Methods

        public async void SaveData()
        {
            var data = new Dictionary<string, object> 
            {
                {"username", username},
                {"userLevel", userLevel.ToString() },
                {"userCoin", userCoin.ToString() },
                {"userDiamond", userDiamond.ToString() }

            };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log($"Save data");
        }

        public async Task LoadData()
        {
            //Novo método para retorno de informações da Unity CloudSave
            ISet<string> keys = new HashSet<string> 
            {
                "username", "userLevel", "userCoin", "userDiamond"
            };
            //var data = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

            /*foreach (var keyValuePair in data) 
            {
                Debug.Log($"Key: {keyValuePair.Key}, Raw Value: {keyValuePair.Value}");
            }*/

            try
            {
                Dictionary<string, Item> serverData = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

                username = serverData["username"].Value.GetAsString();
                userLevel = int.Parse(serverData["userLevel"].Value.GetAsString());
                userCoin = int.Parse(serverData["userCoin"].Value.GetAsString());
                userDiamond = int.Parse(serverData["userDiamond"].Value.GetAsString());
            }
            catch (System.Exception ex) 
            {
                Debug.LogError(ex);
            }

        }
        #endregion
    }
}

