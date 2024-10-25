using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodePortal : MonoBehaviour
{
    //Objetos de cena
    public GameObject[] barreiraP;
    //Objetos de efeito
    public GameObject explodeVfx;
    //Variáeis de controle
    public int boxCol;
    public int randomNumber;
    public Vector2 randomCol;

    // Start is called before the first frame update
    void Start()
    {
        boxCol = 0;
        randomNumber = (int)Random.Range(randomCol.x, randomCol.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (boxCol >= randomNumber)
        {
            for (int i = 0; i < barreiraP.Length; i++)
            {
                barreiraP[i].SetActive(false);
                Destroy(barreiraP[i], 0.5f);
            }
            Instantiate(explodeVfx, transform.position, transform.rotation);
            
        }
    }
}
