using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColisions : MonoBehaviour
{
    //Scripts
    public CharacterMovement characterMovement;
    public PlayerController playerController;
    //Variáveis de controle
    public float actualSpeed;
    public float speedReduction;


    #region Unity Methods
    void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
    }

    /// <summary>
    /// Tags:
    /// DragT - Sistema de ventiladores
    /// BarreiraP - derruba as portas
    /// Death - Retorna para o respawn da fase
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "DragT")
        {
            actualSpeed = characterMovement.speed;
            characterMovement.speed = speedReduction;
        }
        if(other.tag == "BarreiraP")
        {
            Debug.Log("Colidiu");
            other.gameObject.GetComponent<ExplodePortal>().boxCol++;
        }
        if(other.tag == "Death")
        {
            //playerController.enabled = false;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "DragT")
        {
            characterMovement.speed = actualSpeed;
        }
    }
    #endregion
    //////////////////////////////////////Métodos de traps//////////////////////////////////
    /////////////////////Os traps são ativados pelo TrapCollision///////////////////////////
    #region My Methods
    /// <summary>
    /// Paraliza o personagem por 2s
    /// </summary>
    public void GelatinTrap(float timeParalyze)
    {
        actualSpeed = characterMovement.speed;
        characterMovement.speed = 0f;
        StartCoroutine(DisableGelatinTrap(timeParalyze));
    }
    /// <summary>
    /// Inverte os controles do jogador por 3s
    /// </summary>
    /// <param name="timeParalyze"></param>
    public void ConfusionTrap(float timeParalyze)
    {
        actualSpeed = characterMovement.speed;
        characterMovement.speed = -8f;
        StartCoroutine(DisableConfusionaTrap(timeParalyze));
    }
    /// <summary>
    /// Reduz a velocidade do jogador por 2s
    /// </summary>
    /// <param name="timeParalyze"></param>
    public void LowerTrap(float timeParalyze)
    {
        actualSpeed = characterMovement.speed;
        characterMovement.speed = speedReduction;
        StartCoroutine (DisableLowerTrap(timeParalyze));
    }
    /// <summary>
    /// Corrotina para desativar o trap da gelatina
    /// </summary>
    /// <param name="timeP"></param>
    /// <returns></returns>
    IEnumerator DisableGelatinTrap(float timeP)
    {
        yield return new WaitForSeconds(timeP);
        characterMovement.speed = actualSpeed;
        actualSpeed = 0;
    }
    /// <summary>
    /// Corrotina para desativar o trap de confusão
    /// </summary>
    /// <param name="timeP"></param>
    /// <returns></returns>
    IEnumerator DisableConfusionaTrap(float timeP)
    {
        yield return new WaitForSeconds(timeP);
        characterMovement.speed = actualSpeed;
        actualSpeed = 0;
    }
    /// <summary>
    /// Corrotina para desativar o trap de lentidão
    /// </summary>
    /// <param name="timeP"></param>
    /// <returns></returns>
    IEnumerator DisableLowerTrap(float timeP)
    {
        yield return new WaitForSeconds(timeP);
        characterMovement.speed = actualSpeed;
        actualSpeed = 0;
    }
    #endregion
}
