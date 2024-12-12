using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

namespace Game
{
    [Serializable]
    public class PlayerData
    {
        public string userId;
        
    }

    public class GameLocalSave : MonoBehaviour
    {
        public string userId;
        [Tooltip("Local Save Path")]
        private string filePath;

        #region Unity Methods
        private void Awake()
        {
            filePath = Application.persistentDataPath + "playerinfo.dat";

            if (File.Exists(filePath)) { Load(); } else { Save(); }
        }
        #endregion

        #region My Methods
        /// <summary>
        /// Salva as informações do jogador
        /// </summary>
        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(filePath);

            PlayerData data = new PlayerData();

            data.userId = userId;

            bf.Serialize(file, data);
            file.Close();
        }
        /// <summary>
        /// Recupera as informações salvas
        /// </summary>
        public void Load() 
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);

            file.Close();

            userId = data.userId;
        }
        #endregion
    }
}

