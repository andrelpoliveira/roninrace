using System;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

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
    }

    public void Start()
    {
        NetworkObject thisObject = GetComponent<NetworkObject>();
        if (thisObject.HasStateAuthority)
        {
            virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = camPoint;
            cameraTransform = Camera.main.transform;
            GetComponent<PlayerInput>().camera = Camera.main;
        }
    }
    private void OnEnable()
    {
        playerActions.Game.Enable();
    }
    
    private void OnDisable()
    {
        playerActions.Game.Disable();
    }
    /// <summary>
    /// Update para o modo multiplayer
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false)
            return;

        if (cameraTransform == null)
            return;
        
        MovePlayer();
        OnJump();

        _jumpPressed = false;
        
    }
    #endregion

    #region My Methods
    /// <summary>
    /// Método de movimentação new input system
    /// </summary>
    /// <param name="context"></param>
    public void Move(InputAction.CallbackContext context)
    {
        if (Object.HasStateAuthority)
        {
            moveInput = context.ReadValue<Vector2>();
        }
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


        if (Object.HasStateAuthority && isDoubleJump)
        {
            numberOfJumps++;
            _jumpPressed = true;
        }
        if (Object.HasStateAuthority && !isDoubleJump)
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
    /// <summary>
    /// Métodos para chamada no FixedUpdateNetwork para multiplayer
    /// </summary>
    public void MovePlayer()
    {
        if(IsGrounded() && playerVelocity.y < 0)
            playerVelocity.y = 0;

        //Movimentação
        move = new Vector3(moveInput.x, 0f, moveInput.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        characterController.Move(move * Runner.DeltaTime * speed);

        //Gravidade
        ApplyGravity();
        characterController.Move(playerVelocity * Runner.DeltaTime);

        //Rotação
        float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothTime * Runner.DeltaTime);
    }
    /// <summary>
    /// Aplica gravidade ao jogador
    /// </summary>
    public void ApplyGravity()
    {
        if (IsGrounded() && verticalVelocity < 0.0f) { verticalVelocity = -1.0f; }
        else { verticalVelocity += gravityValue * gravityMultiplier * Runner.DeltaTime; }

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
