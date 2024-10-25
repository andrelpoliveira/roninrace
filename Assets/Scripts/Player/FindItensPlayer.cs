using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindItensPlayer : MonoBehaviour
{
    [Header("Componentes no objeto")]
    public GameObject[] itensPlayer;

    #region My Methods
    public void StartChildren()
    {
        itensPlayer = new GameObject[transform.childCount];

        for(int i = 0; i < itensPlayer.Length; i++)
        {
            itensPlayer[i] = transform.GetChild(i).gameObject;
        }
    }
    #endregion

}
