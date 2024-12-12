using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEditor;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    [Tooltip("Input Actions")]
    public InputActionReference moveControl;
    public InputActionReference jumpControl;
    [Space]
    [Header("Variáveis de movimento")]
    //private Vector2 _input;
    //private Vector3 _direction;
    public float speed;
    //[Tooltip("Controle de rotação")]
    public float smoothTime = 4f;
    //private float _currentVelocity;
    [Tooltip("Gravidade")]
    private float _gravity = -9.81f;
    public float gravityMultiplier = -3.0f;
    private Vector3 _velocity;
    private bool _isGrounded;
    [Space]
    [Header("Variáveis de pulo")]
    public float jumpPower;
    public int maxNumberJumps = 2;
    private int _numberOfJumps;
    public bool isDoubleJump;
    [Space]
    [Header("Componentes")]
    //public Transform camPoint;
    public CharacterController _characterController;
    public CinemachineCamera _camera;
    public AudioListener _audioListener;
    public Transform cameraMain;

    #region Unity Methods
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _audioListener.enabled = true;
            _camera.Priority = 1;
        }
        else
        {
            _camera.Priority = 0;
        }
    }
    /// <summary>
    /// Ativa após o componente se tornar ativo ou habilitado
    /// </summary>
    private void OnEnable()
    {
        moveControl.action.Enable();
        jumpControl.action.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    /// <summary>
    /// Desativa após o componente se tornar inativo ou desabilitado
    /// </summary>
    private void OnDisable()
    {
        moveControl.action.Disable();
        jumpControl.action.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if(!IsOwner) return;

        ApplyMoviment();
        
    }
    #endregion

    #region My Methods
    /////////////////////////////////////Funções para aplicação /////////////////////////////////////////
    /// <summary>
    /// Aplica movimentação ao personagem
    /// </summary>
    void ApplyMoviment()
    {
        _isGrounded = _characterController.isGrounded;
        if (_isGrounded && _velocity.y < 0f)
            _velocity.y = 0f;

        Vector2 movement = moveControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cameraMain.transform.forward * move.z + cameraMain.transform.right * move.y;
        move.y = 0f;
        _characterController.Move(move * Time.deltaTime * speed);

        if (jumpControl.action.triggered && _isGrounded)
        {
            _velocity.y += Mathf.Sqrt(jumpPower * gravityMultiplier * _gravity);
            Debug.Log("Jump");
        }

        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);

        ApplyRotation(movement);
    }

    void ApplyRotation(Vector2 movement)
    {
        if(movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMain.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothTime);
        }
    }
    
    #endregion
}
