using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class ChrBase : MonoBehaviour
{
    public bool IsAiming { get; set; }

    public bool IsPossibleMovement
    {
        get => isPossibleMovement;
        set => isPossibleMovement = value;
    }

    public bool IsStrafe
    {
        get => isStrafe;
        set => isStrafe = value;
    }

    public bool IsWalk
    {
        get => isWalk;
        set => isWalk = value;
    }

    public float moveSpeed;
    public float rotationSpeed;

    [HideInInspector] public bool isGrenade = false;
    [HideInInspector] public bool isStrafe = false;
    protected bool isWalk = false;
    protected float speed = 0f;
    protected float targetSpeed = 0f;
    protected float targetSpeedBlend = 0f;

    protected bool isPossibleMovement = false;
    protected bool isPossibleAttack = false;
    protected bool isAttacking = false;
    protected bool isEngaging = false;
    protected float engagingTime = 5.0f;

    protected float verticalVelocity;
    protected bool isGrounded;
    protected float jumpTimeout = 0.1f;
    protected float jumpTimeoutDelta = 0f;

    public float groundRadius = 0.1f;
    public float groundOffset = 0.1f;
    public LayerMask groundLayer;

    protected float targetRotation;           
    protected float rotationVelocity;         
    protected float RotationSmoothTime = 0.12f;

    [HideInInspector] public List<float> DecreaseDamage = new List<float>();
    [HideInInspector] public List<float> DecreaseCoolDown = new List<float>();
    [HideInInspector] public List<float> IncreaseDamage = new List<float>();
    [HideInInspector] public List<float> IncreaseFireRate = new List<float>();
    [HideInInspector] public List<float> IncreaseRecoil = new List<float>();
    [HideInInspector] public List<float> DecreaseShopPrice = new List<float>();
    [HideInInspector] public List<float> IncreaseGainCoin = new List<float>();
    [HideInInspector] public List<float> IncreaseWeaponDrop = new List<float>();
    [HideInInspector] public List<float> IncreaseOopartsDrop = new List<float>();
    [HideInInspector] public List<float> IncreaseShield = new List<float>();

    protected UnityEngine.CharacterController unityCharacterController;
    [HideInInspector] public Animator characterAnimator;

    public float maxHp;
    public float curHp;

    public float maxShield;
    public float curShield;

    bool isLive;

    protected virtual void Awake()
    {
        characterAnimator = GetComponent<Animator>();
        unityCharacterController = GetComponent<UnityEngine.CharacterController>();
    }

    protected virtual void Update()
    {
        FreeFall();
        GroundCheck();

        characterAnimator.SetFloat("Strafe", isStrafe ? 1f : 0f);

        speed = isWalk ? 1f : 3f;

        targetSpeedBlend = Mathf.Lerp(targetSpeedBlend, targetSpeed, Time.deltaTime * 10f);

        characterAnimator.SetFloat("Speed", targetSpeedBlend);
    }

    protected virtual void Start()
    {
        isLive = true;

        curHp = maxHp;
        curShield = maxShield;

    }

    public virtual void Fire()
    {

    }

    public virtual void ChangedPrimaryWeapon()
    {

    }

    public virtual void ChangedSecondaryWeapon()
    {

    }

    public virtual void EquipWeapon(DroppedWeapon weapon)
    {

    }

    public virtual void SetAiming(float aiming)
    {

    }

    public virtual void ActiveWeaponSkill()
    {

    }

    public virtual void OnDamaged(float damage)
    {
        if(isLive)
        {
            engagingTime = 5.0f;
            isEngaging = true;

            float _damage = CalculateOopartsValue(damage,DecreaseDamage);

            if (curShield > 0)
            {
                curShield -= _damage;
                if (curShield < 0)
                {
                    curShield = 0;
                }
                HUDManager.Instance.UpdateShieldHUD(curShield, maxShield);
            }
            else if (curShield <= 0)
            {
                curHp -= _damage;
                HUDManager.Instance.UpdateHpHUD(curHp, maxHp);
            }

            if(curHp <= 0)
            {
                isLive = false;
            }
        }
    }
    public virtual void Reload()
    {

    }

    public void Move(Vector2 input, float yAxisAngle)
    {
        targetSpeed = input.magnitude > 0f ? targetSpeed = speed : 0f;

        //if (IsPossibleMovement == false)
        //{
        //    input = Vector2.zero;
        //}

        float magnitude = input.magnitude;
        if (magnitude <= 0.1f)
            return;

  

        Vector3 inputDirection = new Vector3(input.x, 0, input.y).normalized;

        targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + yAxisAngle;

        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
            ref rotationVelocity, RotationSmoothTime);

        if (!isStrafe)
        {
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }

        Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;

        if (OopartsActiveManager.Instance.oopartsActive[(int)OopartsType.Boots] == true)
        {
            unityCharacterController.Move(targetDirection.normalized * moveSpeed * 1.3f * Time.deltaTime
        + new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime);
        }
        else
        {
            unityCharacterController.Move(targetDirection.normalized * moveSpeed * Time.deltaTime
        + new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime);
        }

        //characterAnimator.SetFloat("Horizontal", false == IsPossibleMovement ? 0 : input.x);

        characterAnimator.SetFloat("Horizontal", input.x);

        //characterAnimator.SetFloat("Vertical", false == IsPossibleMovement ? 0 : input.y);

        characterAnimator.SetFloat("Vertical", input.y);
    }

    public void Rotate(Vector3 targetPoint)
    {
        if (!isStrafe)
            return;


        Vector3 position = transform.position;
        Vector3 direction = (targetPoint - position).normalized;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void FreeFall()
    {
        if (isGrounded == false)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        else 
        {
            if (jumpTimeoutDelta > 0)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                verticalVelocity = 0f;
            }
        }
    }

    public void GroundCheck()
    {
        Vector3 spherePosition = transform.position + (Vector3.down * groundOffset);

        isGrounded = Physics.CheckSphere(spherePosition, groundRadius, groundLayer, QueryTriggerInteraction.Ignore);
    }

    public float CalculateOopartsValue(float oopartsValue, List<float> listValue)
    {
        float baseOopartsValue = 1.0f;

        foreach (var value in listValue)
        {
            baseOopartsValue *= (1 - value / 100);
        }

        return oopartsValue * baseOopartsValue;
    }

    public float CalculateOopartsFireRateValue(float oopartsValue, List<float> listValue)
    {
        float baseOopartsFireRateValue = 1.0f;

        foreach(var value in listValue)
        {
            baseOopartsFireRateValue /= 1 + value / 100;
        }
        return oopartsValue * baseOopartsFireRateValue;
    }
}

