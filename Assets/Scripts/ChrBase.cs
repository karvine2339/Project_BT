using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public float jumpForce;

    protected bool isWalk = false;
    protected bool isStrafe = false;
    protected float speed = 0f;
    protected float targetSpeed = 0f;
    protected float targetSpeedBlend = 0f;

    protected bool isPossibleMovement = false;
    protected bool isPossibleAttack = false;
    protected bool isAttacking = false;
    protected bool isEngaging = false;
    protected float engagingTime = 5.0f;

    protected float targetRotation;           
    protected float rotationVelocity;         
    protected float RotationSmoothTime = 0.12f; 

    protected UnityEngine.CharacterController unityCharacterController;
    protected Animator characterAnimator;

    public int maxHp;
    public int curHp;

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

    public virtual void SetAiming(float aiming)
    {

    }

    public virtual void OnDamaged(float damage)
    {
        if(isLive)
        {
            engagingTime = 5.0f;
            isEngaging = true;

            if (curShield > 0)
            {
                curShield -= (int)damage;
                HUDManager.Instance.shieldBar.fillAmount = curShield / maxShield;
            }
            else
            {
                curHp -= (int)damage;
                HUDManager.Instance.hpBar.fillAmount = curHp / maxHp;
            }

            if(curHp <= 0)
            {
                isLive = false;
            }
        }
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

        unityCharacterController.Move(targetDirection.normalized * moveSpeed * Time.deltaTime);

        //characterAnimator.SetFloat("Horizontal", false == IsPossibleMovement ? 0 : input.x);

        characterAnimator.SetFloat("Horizontal", input.x);

        //characterAnimator.SetFloat("Vertical", false == IsPossibleMovement ? 0 : input.y);

        characterAnimator.SetFloat("Vertical", input.y);
    }

    public void Rotate(Vector3 targetPoint)
    {
        if (!isStrafe)
            return;

        // 현재 위치와 목표 위치를 기준으로 방향 벡터 계산
        Vector3 position = transform.position;
        Vector3 direction = (targetPoint - position).normalized;
        direction.y = 0f; // 수평 평면에서만 회전

        // 목표 방향으로 회전하기 위한 Quaternion 생성
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // 현재 회전에서 목표 회전으로 부드럽게 보간
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

}

