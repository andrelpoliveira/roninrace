using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using Unity.Netcode;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : NetworkBehaviour
{
    [Header("Variaveis de Movimento")]
    private Vector3 playerVelocity;
    private Vector2 moveInput;
    private Vector3 move;
    public float speed;
    public float smoothTime = 0.8f;
    private float currentVelocity;
    [Space]
    [Header("Variáveis de Pulo")]
    public float jumpHeight;
    private float gravityValue = -9.81f;
    public float gravityMultiplier = 3f;
    private float verticalVelocity;
    public bool _jumpPressed;
    public bool isDoubleJump;
    [SerializeField]private int numberOfJumps;
    public int maxNumberOfJumps = 2;
    [Space]
    [Header("Variáveis de corrida")]
    public float runSpeed;
    public bool isRun;
    public float timeRun;
    [Space]
    [Header("Componentes")]
    public CharacterController characterController;
    public Transform camPoint;
    [Space]
    [Header("Sistema de Input")]
    public InputActions playerActions;
    public Transform cameraTransform;
    public GameObject virtualCamera;

    #region Unity Methods
    private void Awake()
    {
        playerActions = new InputActions();
        characterController = GetComponent<CharacterController>();
        virtualCamera = GameObject.Find("3rdPersonCinemachine");
        //Input Actions
        //playerActions.Game.Move.performed += Move;
        playerActions.Game.Jump.performed += Jump;
        playerActions.Game.Trap.performed += Trap;
    }

    public void Start()
    {
        virtualCamera.GetComponent<CinemachineCamera>().Follow = camPoint;
        cameraTransform = Camera.main.transform;
        GetComponent<PlayerInput>().camera = Camera.main;
        speed = 6f;
    }
    private void OnEnable()
    {
        playerActions.Game.Move.Enable();
        playerActions.Game.Jump.Enable();
        playerActions.Game.Trap.Enable();
        playerActions.Enable();
    }
    
    private void OnDisable()
    {
        playerActions.Game.Move.Disable();
        playerActions.Game.Jump.Disable();
        playerActions.Game.Trap.Disable();
        playerActions.Disable();
    }

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) Destroy(this);
    }
    /// <summary>
    /// Update para o modo multiplayer
    /// </summary>
    public void Update()
    {
        if (!IsOwner)
            return;

        if (cameraTransform == null)
            return;
        
        MovePlayer();
        OnJump();

        _jumpPressed = false;
        
    }
    #endregion

    #region My Methods
    ///////////////////////////////////Inputs/////////////////////////////////////////////////////////////////
    /// <summary>
    /// Método de movimentação new input system
    /// </summary>
    /// <param name="context"></param>
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log(moveInput.ToString());
    }
    /// <summary>
    /// Método que cancela a movimentação new input system
    /// </summary>
    /// <param name="context"></param>
    public void MoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }
    /// <summary>
    /// Método de pular new input system
    /// </summary>
    /// <param name="context"></param>
    public void Jump(InputAction.CallbackContext context) 
    {
        if(!context.started) return;
        if (!IsGrounded() && numberOfJumps >= maxNumberOfJumps) return;
        if(numberOfJumps == 0) StartCoroutine(WaitForLanding());


        if (isDoubleJump)
        {
            numberOfJumps++;
            _jumpPressed = true;
        }
        if (!isDoubleJump)
        {
            numberOfJumps = maxNumberOfJumps;
            _jumpPressed = true;
        }
            
    }
    /// <summary>
    /// Método de aplicar traps new input system
    /// </summary>
    /// <param name="context"></param>
    public void Trap(InputAction.CallbackContext context)
    {

    }
    //////////////////////////////////////Sistema de movimentação/////////////////////////////////////////////
    /// <summary>
    /// Métodos para chamada no Update para multiplayer
    /// </summary>
    public void MovePlayer()
    {
        if(IsGrounded() && playerVelocity.y < 0)
            playerVelocity.y = 0;

        //Movimentação
        move = new Vector3(moveInput.x, 0f, moveInput.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        characterController.Move(move * Time.deltaTime * speed);

        //Gravidade
        ApplyGravity();
        characterController.Move(playerVelocity * Time.deltaTime);

        //Rotação
        float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothTime * Time.deltaTime);
    }
    /// <summary>
    /// Aplica gravidade ao jogador
    /// </summary>
    public void ApplyGravity()
    {
        if (IsGrounded() && verticalVelocity < 0.0f) { verticalVelocity = -1.0f; }
        else { verticalVelocity += gravityValue * gravityMultiplier * Time.deltaTime; }

        playerVelocity.y = verticalVelocity;
    }
    /// <summary>
    /// Método de pulo
    /// </summary>
    public void OnJump()
    {
        if(_jumpPressed)
            verticalVelocity = jumpHeight;
    }

    private IEnumerator WaitForLanding() 
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(IsGrounded);

        numberOfJumps = 0;
    }

    private bool IsGrounded() => characterController.isGrounded;
    #endregion
}
