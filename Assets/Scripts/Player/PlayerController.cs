using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Scripts")]
    public CharacterMovement characterMovement;
    [Tooltip("Input Actions")]
    private InputActions inputActions;
    [Header("Mobile Canvas")]
    public bool useMobile;
    public GameObject mobileCanvas;

    //#region Unity Methods
    /*private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Enable();
        RefreshOnScreenControls();
    }*/
    /// <summary>
    /// Método de validação da Unity
    /// </summary>
    //private void OnValidate()
    //{
    //    RefreshOnScreenControls ();
    //}

    //private void Update()
    //{
    //    var moveInput = inputActions.Game.Move.ReadValue<Vector2>();
    //    var wantsToJump = inputActions.Game.Jump.WasPressedThisFrame();

    //    characterMovement.SetInput(new CharacterMovementInput()
    //    {
    //        MoveInput = moveInput,
    //        wantToJump = wantsToJump
    //    });
    //}
    //#endregion

    //#region My Methods
    ///// <summary>
    ///// Controla a entrada para mobile
    ///// </summary>
    //private void RefreshOnScreenControls()
    //{
    //    mobileCanvas.SetActive(useMobile);
    //    Cursor.visible = useMobile;
    //}
    //#endregion
}
