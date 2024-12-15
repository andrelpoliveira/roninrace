using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeTrap
{
    GELATIN, CONFUSION, LOWER, UNLUCKYW
}

public class TrapsCollision : MonoBehaviour
{

    [Header("Controle de tipos de traps")]
    public TypeTrap trap;
    [Header("Variáveis de controle")]
    public float timeTrap;
    [Header("Componentes Relógio Azarado")]
    public Transform[] watchObj;
    public int unluckyRandom;
    public Vector2 sortUnlucky;

    private void Start()
    {
        unluckyRandom = (int)Random.Range(sortUnlucky.x, sortUnlucky.y);
    }

    /// <summary>
    /// Traps
    /// Gelatin - congela o jogador por 2s
    /// Confusion - inverte os controles do jogador por 3s
    /// Lower - diminui a velocidade do jogador por 2s
    /// UnluckW - pode ativar o avanço ou retrocesso do jogador
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            TypeControl(other.gameObject);            
        }
    }
    /// <summary>
    /// Função de controle do tipo de trap
    /// </summary>
    /// <param name="obj"></param>
    public void TypeControl(GameObject obj)
    {
        switch (trap)
        {
            case TypeTrap.GELATIN:
                obj.GetComponent<PlayerColisions>().GelatinTrap(timeTrap);
                Destroy(this.gameObject, 0.5f);
                break;
            case TypeTrap.CONFUSION:
                obj.GetComponent<PlayerColisions>().ConfusionTrap(timeTrap);
                Destroy(this.gameObject, 0.5f);
                break;
            case TypeTrap.LOWER:
                obj.GetComponent<PlayerColisions>().LowerTrap(timeTrap);
                Destroy(this.gameObject, 0.5f);
                break;
            case TypeTrap.UNLUCKYW:
                if (unluckyRandom <= 4)
                {
                    //obj.GetComponent<CharacterMovement>().enabled = false;
                    obj.transform.position = watchObj[0].position;
                }
                if(unluckyRandom >= 5)
                {
                    //obj.GetComponent<CharacterMovement>().enabled = false;
                    obj.transform.position = watchObj[1].position;
                }
                Destroy(this.gameObject, 1f);
                break;
        }
    }
}
