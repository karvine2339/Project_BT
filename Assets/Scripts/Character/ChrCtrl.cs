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

    private DroppedWeapon weapon;

    private void Awake()
    {
        UIManager.Show<HUDManager>(UIList.HUDCanvas);
        characterBase = GetComponent<ChrBase>();
    }

    private void Start()
    {
        BTInputSystem.Instance.onReload += Reload; 
        BTInputSystem.Instance.onFire += Fire;
        BTInputSystem.Instance.onInteract += Interact;
        BTInputSystem.Instance.onMouseWheel += OnMouseWheel;
        BTInputSystem.Instance.onChangedPrimaryWeapon += OnChangedPrimaryWeapon;
        BTInputSystem.Instance.onChangedSecondaryWeapon += OnChangedSecondaryWeapon;
        BTInputSystem.Instance.onEquipWeapon += OnEquipWeapon;
        BTInputSystem.Instance.onWeaponSkill += OnWeaponSkill;
        BTInputSystem.Instance.onDash += OnDash;

        UIManager.Show<TacticalManager>(UIList.TacticalManual_Canvas);
        UIManager.Show<InventoryManager>(UIList.InventoryCanvas);
        UIManager.Show<WorkShopManager>(UIList.WorkShopCanvas);
        UIManager.Show<Interaction_UI>(UIList.Interaction_UICanvas);
        UIManager.Show<OopartsManager>(UIList.OopartsSelectCanvas);
        UIManager.Show<ShopManager>(UIList.ShopCanvas);

        characterBase.Teleport(UserDataModel.Singleton.PlayerSessionData.lastPosition);
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

    private void Update()
    {
        Vector2 input = BTInputSystem.Instance.moveInput;        

        characterBase.Move(input, Camera.main.transform.rotation.eulerAngles.y);

        UserDataModel.Singleton.SetPlayerSessionData(
            characterBase.transform.position,
            characterBase.curHp,
            characterBase.curShield);

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

    private void Fire()
    {
        characterBase.Fire();
    }

    private void OnEquipWeapon()
    {
        characterBase.EquipWeapon(weapon);
    }

    private void Reload()
    {
        characterBase.Reload();
    }

    private void OnChangedPrimaryWeapon()
    {
        characterBase.ChangedPrimaryWeapon();
    }

    private void OnChangedSecondaryWeapon()
    {
        characterBase.ChangedSecondaryWeapon();
    }

    private void OnDash()
    {
        characterBase.Dash();
    }

    //## ChrCtrl
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

    private void OnWeaponSkill()
    {
        characterBase.ActiveWeaponSkill();
    }
}