using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Linq;
/// <summary>
/// Dados a serem guardados
/// nome de usu�rio.
/// level do usu�rio.
/// skins do usu�rio
/// moeda do jogo do usu�rio - compra as skins, traps dando tiro no gacha
/// diamante do usu�rio
/// traps do usu�rio
/// ranking do usu�rio
/// pets do usu�rio
/// personagens do usu�rio
/// assinatura do jogador
/// </summary>
public class FirebaseDataServer : MonoBehaviour
{
    [Header("Vari�veis Firestore")]
    FirebaseFirestore db;
    [Space]
    [Header("Vari�veis para armazenar dados do jogador")]
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
    [Header("Vari�veis para loading das infos do jogador")]
    public int userSkin;
    public int userPet;
    

    #region Unity Methods
    private void Start()
    {
        
    }
    #endregion

    #region Save and Load Methods
    /// <summary>
    /// Salva as informa��es no Firestore
    /// 1 grava��o e 7 leituras
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
    /// Recupera as informa��es no Firestore
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
