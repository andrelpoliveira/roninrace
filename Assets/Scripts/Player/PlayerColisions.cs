using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerColisions : NetworkBehaviour
{
    [Header("Scripts")]
    public PlayerController playerController;
    [Space]
    [Header("Variáveis de controle")]
    public float actualSpeed;
    public float speedReduction;
    

    #region Unity Methods
    void Start()
    {
        playerController = GetComponent<PlayerController>();
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
            actualSpeed = playerController.speed;
            playerController.speed = speedReduction;
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
        if(other.tag == "NPC" && IsOwner)
        {
            playerController.IsNpc = true;
            other.GetComponent<NPCLobby>().LobbySwitch();
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "DragT")
        {
            playerController.speed = actualSpeed;
        }
        if (other.tag == "NPC" && IsOwner)
        {
            playerController.IsNpc = false;
            other.GetComponent<NPCLobby>().CloseLobby();
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
        actualSpeed = playerController.speed;
        playerController.speed = 0f;
        StartCoroutine(DisableGelatinTrap(timeParalyze));
    }
    /// <summary>
    /// Inverte os controles do jogador por 3s
    /// </summary>
    /// <param name="timeParalyze"></param>
    public void ConfusionTrap(float timeParalyze)
    {
        actualSpeed = playerController.speed;
        playerController.speed = -8f;
        StartCoroutine(DisableConfusionTrap(timeParalyze));
    }
    /// <summary>
    /// Reduz a velocidade do jogador por 2s
    /// </summary>
    /// <param name="timeParalyze"></param>
    public void LowerTrap(float timeParalyze)
    {
        actualSpeed = playerController.speed;
        playerController.speed = speedReduction;
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
        playerController.speed = actualSpeed;
        actualSpeed = 0;
    }
    /// <summary>
    /// Corrotina para desativar o trap de confusão
    /// </summary>
    /// <param name="timeP"></param>
    /// <returns></returns>
    IEnumerator DisableConfusionTrap(float timeP)
    {
        yield return new WaitForSeconds(timeP);
        playerController.speed = actualSpeed;
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
        playerController.speed = actualSpeed;
        actualSpeed = 0;
    }
    #endregion
}
