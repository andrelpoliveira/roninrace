using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Variaveis de Controle")]
    private Vector3 playerVelocity;
    public bool groundPlayer;
    [Tooltip("Variáveis de movimento e pulo")]
    public float speed;
    public float jumpHeight;
    private float gravityValue = 9.81f;
    private float verticalVelocity;
    [Tooltip("Variáveis de corrida")]
    public float runSpeed;
    public bool isRun;
    public float timeRun;

    [Header("Componentes")]
    public CharacterController characterController;

    [Header("Sistema de Input")]
    public InputActions playerActions;
    public Transform cameraTransform;

    #region Unity Methods
    private void Awake()
    {
        playerActions = new InputActions();
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        playerActions.Game.Jump.performed += Jump;
        playerActions.Game.Trap.performed += Trap;
    }

    private void OnEnable()
    {
        playerActions.Game.Jump.Enable();
        playerActions.Game.Trap.Enable();
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Game.Jump.Disable();
        playerActions.Game.Trap.Disable();
        playerActions.Disable();
    }

    private void FixedUpdate()
    {
        groundPlayer = characterController.isGrounded;
        if (groundPlayer && playerVelocity.y <0) 
            playerVelocity.y = 0;

        //Movimentação do player
        Vector2 input = playerActions.Game.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        move = move.x * cameraTransform.right + move.z * cameraTransform.forward;
        move.y = 0;
        characterController.Move(move * Time.deltaTime * speed);

        if(move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y = VerticalForceCalculate();
        characterController.Move(playerVelocity * Time.deltaTime);
    }
    #endregion

    #region My Methods

    private float VerticalForceCalculate()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = 0;
        }
        else
        {
            verticalVelocity -= gravityValue * Time.deltaTime;
        }
        return verticalVelocity;
    }

    public void Jump(InputAction.CallbackContext context) 
    {
        if (characterController.isGrounded) 
            verticalVelocity = MathF.Sqrt(jumpHeight * gravityValue * 2);
    }

    public void Trap(InputAction.CallbackContext context)
    {

    }
    #endregion
}
