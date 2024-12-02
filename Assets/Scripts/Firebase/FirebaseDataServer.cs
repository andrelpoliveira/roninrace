using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Linq;
/// <summary>
/// Dados a serem guardados
/// nome de usuário.
/// level do usuário.
/// skins do usuário
/// moeda do jogo do usuário - compra as skins, traps dando tiro no gacha
/// diamante do usuário
/// traps do usuário
/// ranking do usuário
/// pets do usuário
/// personagens do usuário
/// assinatura do jogador
/// </summary>
public class FirebaseDataServer : MonoBehaviour
{
    [Header("Variáveis Firestore")]
    FirebaseFirestore db;
    [Space]
    [Header("Variáveis para armazenar dados do jogador")]
    public string userId;
    public string username;
    public int userLevel;
    public string[] nameSkin;
    public int[] costSkins;
    public float userCoins;
    public int userDiamond;
    public string[] nameTrap;
    public int[] costTraps;
    public int userRanking;
    public string[] namePet;
    public int[] costPets;
    public bool userVip;
    [Space]
    [Header("Variáveis para loading das infos do jogador")]
    public int userSkin;
    public int userPet;
    

    #region Unity Methods
    private void Start()
    {
        
    }
    #endregion

    #region Save and Load Methods
    /// <summary>
    /// Salva as informações no Firestore
    /// 1 gravação e 7 leituras
    /// </summary>
    public void SaveData()
    {
        db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection(userId).Document("userData");
        Dictionary<string, object> usuarios = new Dictionary<string, object>();
        usuarios[$"username"] = username;

        docRef.SetAsync(usuarios).ContinueWithOnMainThread(task => 
        {
            if(task.IsCompleted)
                Debug.Log("Added data to the userData document in the " + userId + "collections");
            else
                Debug.LogError("No added data: " + task.Exception);
        });
    }
    /// <summary>
    /// Recupera as informações no Firestore
    /// </summary>
    public void LoadData() 
    {
        db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection(userId).Document("userData");
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task => { 
            DocumentSnapshot snapshot = task.Result;
            Dictionary<string, object> usuarios = snapshot.ToDictionary();
            if (snapshot.Exists)
            {
                username = usuarios[$"username"].ToString();
            }
            else
            {
                Debug.Log(String.Format("Document {0} does not exist", snapshot.Id));
            }
        });
    }
    #endregion

    #region My Methods

    #endregion
}
