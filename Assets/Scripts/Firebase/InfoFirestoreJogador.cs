using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoFirestoreJogador : MonoBehaviour
{
    public static InfoFirestoreJogador Instance;

    public string userId;
    public string username;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    
}
