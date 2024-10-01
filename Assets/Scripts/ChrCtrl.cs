using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Transform cameraPivot;
    public float cameraRotationSpeed = 30f;

    private float yaw;                       
    private float pitch;                    

    private ChrBase characterBase;

    private void Awake()
    {
        characterBase = GetComponent<ChrBase>();
    }

    private void Start()
    {
        BTInputSystem.Instance.onAttack += Attack;
        BTInputSystem.Instance.onInteract += Interact;
        BTInputSystem.Instance.onShowInfo += ShowInfoBox;
        BTInputSystem.Instance.onHideInfo += HideInfoBox;
        BTInputSystem.Instance.onMouseWheel += OnMouseWheel;

    }

    private void OnMouseWheel(float mouseWheel)
    {
        Interaction_UI.Instance.ChangeSelection(mouseWheel);
    }

    private void Interact()
    {
        var playerCharacter = characterBase as PlayerCharacter;
        if (playerCharacter == null)
            return;

        playerCharacter.Interact();
    }

    private void ShowInfoBox()
    {
        var playerCharacter = characterBase as PlayerCharacter;
        if (playerCharacter == null)
            return;

        playerCharacter.ShowInfoBox();
    }

    private void HideInfoBox()
    {
        var playerCharacter = characterBase as PlayerCharacter;
        if (playerCharacter == null)
            return;

        playerCharacter.HideInfoBox();
    }

    private void Update()
    {
        Vector2 input = BTInputSystem.Instance.moveInput;        

        characterBase.Move(input, Camera.main.transform.rotation.eulerAngles.y);

        characterBase.Rotate(CameraSystem.Instance.AimingTargetPoint);

        characterBase.IsStrafe = BTInputSystem.Instance.isStrafe;
        characterBase.IsWalk = BTInputSystem.Instance.isWalk;

        characterBase.IsAiming = BTInputSystem.Instance.isAim;

    }

    private void LateUpdate()
    {
        Vector2 look = BTInputSystem.Instance.look; 
        CameraRotate(look);                         
    }

    private void Attack()
    {
        characterBase.Fire();
    }
    private void CameraRotate(Vector2 look)
    {
        yaw += look.x * cameraRotationSpeed * Time.deltaTime;      
        pitch -= look.y * cameraRotationSpeed * Time.deltaTime;
        if(BTInputSystem.Instance.isRecoiling == true)
        {
            pitch += BTInputSystem.Instance.recoilRecovery * Time.deltaTime;
        }
        pitch = Mathf.Clamp(pitch, -80, 80); 

        cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0.0f);
    }
}