using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;
using System.Runtime.CompilerServices;


public class PlayerCharacter : ChrBase
{
    public Projectile projectilePrefab;
    public float projectileSpeed = 10f;
    public float grenadeSpeed = 15f;

    protected float aiming = 0f;
    private float aimingBlend = 0f;

    public float fireRate = 0.1f;
    public float reloadTime = 1.3f;
    private float shieldIncreaseSpeed = 10f;
    [HideInInspector] public bool isReload = false;

    [HideInInspector] public float credit;
    [HideInInspector] public float curCredit;

    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private LayerMask targetPointLayerMask = new LayerMask();
    [SerializeField] private LayerMask droppedWeaponLayerMask = new LayerMask();
    [SerializeField] private Transform aimPointPosition;
    [SerializeField] private Vector3 targetPointPosition;
    private Rig aimRig;
    private RigBuilder rigBuilder;

    private float aimRigWeight;

    public static PlayerCharacter Instance { get; private set; }

    private InteractionSensor interactionSensor;

    private List<IInteractable> currentInteractionItems = new List<IInteractable>();

    public Transform weaponRoot;
    [HideInInspector] public Weapon[] weapons = new Weapon[2];

    [HideInInspector] public Weapon currentWeapon = null;

    private GameObject weapon;
    private GameObject weapon2;

    [HideInInspector] public float skillCoolDown = 0.0f;
    [HideInInspector] public float skillCoolDuration = 0.0f;
    [HideInInspector] public bool isCoolDown = false;

    public Rigidbody grenadePrefab;

    private DroppedWeapon droppedWeapon = null;

    public int pelletCount = 10;     
    public float spreadAngle = 5f;

    public int explosionCount = 0;


    protected override void Awake()
    {
        base.Awake();

        Instance = this;

        interactionSensor = GetComponentInChildren<InteractionSensor>();
        interactionSensor.OnDetected += OnDetectedInteraction;
        interactionSensor.OnLostSignal += OnLostInteraction;

        //weapons = GetComponentsInChildren<Weapon>(true);
    }

    public void Interact()
    {
        if (currentInteractionItems.Count <= 0)
        {
            return;
        }

        Interaction_UI.Instance.ExecuteInteractionData();

    }

    private void OnDetectedInteraction(IInteractable interactable)
    {
        if (interactable.IsAutoInteract == true)
        {
            interactable.Interact(this);
        }
        else
        {
            Interaction_UI.Instance.AddInteractionData(interactable);
            currentInteractionItems.Add(interactable);
        }
    }

    private void OnLostInteraction(IInteractable interactable)
    {
        Interaction_UI.Instance.RemoveInteractionData(interactable);

        currentInteractionItems.Remove(interactable);
    }
    protected override void Start()
    {
        base.Start();

        HUDManager.Instance.UpdateHpHUD(curHp, maxHp);
        HUDManager.Instance.UpdateShieldHUD(curShield, maxShield);

        curCredit = 500000;
        credit = curCredit;
        HUDManager.Instance.creditText.text = credit.ToString();
  

        aimRig = GetComponentInChildren<Rig>();
        rigBuilder = GetComponentInChildren<RigBuilder>();

        aimColliderLayerMask = LayerMask.GetMask("Wall");
        targetPointLayerMask = ~LayerMask.GetMask("AimPoint");
        droppedWeaponLayerMask = LayerMask.GetMask("DroppedWeapon");

        weapons[0] = GetComponentInChildren<Weapon>();

        SetStartWeapon();

    }

    public override void SetAiming(float aiming)
    {
        this.aiming = aiming;
        characterAnimator.SetFloat("Aiming", this.aiming);
    }

    protected override void Update()
    {

        aimingBlend = Mathf.Lerp(aimingBlend, (IsAiming ? 1f : 0f), Time.deltaTime * 10f);
        SetAiming(aimingBlend);

        characterAnimator.SetFloat("Strafe", isStrafe ? 1f : 0f);

        speed = isWalk ? 1f : 3f;
        targetSpeedBlend = Mathf.Lerp(targetSpeedBlend, targetSpeed, Time.deltaTime * 10f);

        characterAnimator.SetFloat("Speed", targetSpeedBlend);

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnDamaged(10);
            Debug.Log("OnDamaged!");
        }

        if (isEngaging)
        {
            engagingTime -= Time.deltaTime;
            if (engagingTime < 0.0f)
                isEngaging = false;
        }

        UpdateShield();
        AimStatement();
        RayWeapon();
        GroundCheck();
        FreeFall();

        if (isStrafe)
        {
            if (fireRate > 0.0f)
            {
                fireRate -= Time.deltaTime;
            }
        }
    }

    public override void Fire()
    {

        if (isStrafe)
        {  
            if (fireRate <= 0.0f)
            {
                if (currentWeapon.curAmmo == 0)
                {
                    if (isReload)
                        return;

                    StartCoroutine(ReloadCo());
                    return;
                }
                if (isReload == true)
                    return;

                Muzzle();

                if (OopartsActiveManager.Instance.oopartsActive[(int)OopartsType.Dice])
                {
                    explosionCount++;
                }

                NormalFire();
                //ShotgunFire();



                currentWeapon.curAmmo--;
                PlayerStat.Instance.bulletDamage = Random.Range(PlayerStat.Instance.BulletMinDamage, PlayerStat.Instance.BulletMaxDamage);

                fireRate = PlayerCharacter.Instance.CalculateOopartsFireRateValue(PlayerStat.Instance.FireRate,
                                                                                  PlayerCharacter.Instance.IncreaseFireRate);

                HUDManager.Instance.SetWeaponAmmo(currentWeapon.curAmmo, currentWeapon.maxAmmo);

                BTInputSystem.Instance.TriggerRecoil();
            }
        }
    }

    public override void ChangedPrimaryWeapon()
    {
        if (weapons[0] != null)
        {
            if (isReload)
                return;

            if (weapons[0].gameObject != null)
            {
                if (weapons[0].gameObject.activeSelf == false)
                {
                    weapons[0].gameObject.SetActive(true);
                    currentWeapon = weapons[0];
                }

                HUDManager.Instance.SetWeaponInfo(weapons[0].weaponName, weapons[0].CurrentAmmo, weapons[0].MaxAmmo);
                HUDManager.Instance.curWeaponImage.sprite = weapons[0].weaponImage;
                HUDManager.Instance.firstWeapon.GetComponent<Image>().sprite = weapons[0].weaponImage;
                HUDManager.Instance.firstWeapon.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                HUDManager.Instance.firstWeapon.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
                HUDManager.Instance.skillIcon.sprite = currentWeapon.skillImage;
                if (weapons[1] != null)
                {
                    HUDManager.Instance.secondWeapon.GetComponent<Image>().sprite = weapons[1].weaponImage;
                    HUDManager.Instance.secondWeapon.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                    HUDManager.Instance.secondWeapon.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
                }

                if (weapons[1] != null)
                {
                    weapons[1].gameObject.SetActive(false);
                }

                Debug.Log($"WeaponChanged. Current Weapon = {weapons[0].name}");
            }
            characterAnimator.SetTrigger("ChangeWeapon");
        }
    }

    public override void ChangedSecondaryWeapon()
    {
        if (weapons[1] != null)
        {
            if (isReload)
                return;

            if (weapons[1].gameObject != null)
            {
                if (weapons[1].gameObject.activeSelf == false)
                {
                    weapons[1].gameObject.SetActive(true);
                    currentWeapon = weapons[1];
                }
                HUDManager.Instance.SetWeaponInfo(weapons[1].weaponName, weapons[1].CurrentAmmo, weapons[1].MaxAmmo);
                HUDManager.Instance.curWeaponImage.sprite = weapons[1].weaponImage;
                HUDManager.Instance.secondWeapon.GetComponent<Image>().sprite = weapons[1].weaponImage;
                HUDManager.Instance.secondWeapon.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                HUDManager.Instance.secondWeapon.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
                HUDManager.Instance.skillIcon.sprite = currentWeapon.skillImage;

                if (weapons[0] != null)
                {
                    HUDManager.Instance.firstWeapon.GetComponent<Image>().sprite = weapons[0].weaponImage;
                    HUDManager.Instance.firstWeapon.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                    HUDManager.Instance.firstWeapon.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);

                }

                if (weapons[0] != null)
                {
                    weapons[0].gameObject.SetActive(false);
                }

                Debug.Log($"WeaponChanged. Current Weapon = {weapons[1].name}");
            }
        }
        characterAnimator.SetTrigger("ChangeWeapon");
    }

    IEnumerator ReloadCo()
    {
        isReload = true;
        BTInputSystem.Instance.isStrafe = false;
        characterAnimator.SetBool("IsReload", true);
        yield return new WaitForSeconds(reloadTime);
        currentWeapon.curAmmo = currentWeapon.maxAmmo;
        HUDManager.Instance.SetWeaponAmmo(currentWeapon.curAmmo, currentWeapon.maxAmmo);
        characterAnimator.SetBool("IsReload", false);
        isReload = false;
    }

    public void UpdateShield()
    {
        if (isEngaging == false && curShield != maxShield)
        {
            curShield += Time.deltaTime * CalculateOopartsValue(shieldIncreaseSpeed,IncreaseShield);

            HUDManager.Instance.UpdateShieldHUD(curShield, maxShield);
            if (curShield >= maxShield)
                curShield = maxShield;
        }
    }

    public void SetStartWeapon()
    {
        currentWeapon = weapons[0];
        weapons[0].baseMinDamage = 1000;
        weapons[0].baseMaxDamage = 2000;
        weapons[0].baseCriticalProbability = 20f;
        weapons[0].baseCriticalDamage = 1f;
        weapons[0].baseFireRate = 0.2f;
        weapons[0].weaponName = "WHITE FANG 465";
        weapons[0].effectString = new string[3];

        weapons[0].effectString[0] = "";
        weapons[0].effectString[1] = "";
        weapons[0].effectString[2] = "";
        weapons[0].InitWeaponStat();
        weapons[0].InitFirstWeaponUI();
    }

    public override void OnDamaged(float damage)
    {
        base.OnDamaged(damage);
    }

    public void Muzzle()
    {
        GameObject muzzle = Instantiate(currentWeapon.muzzleFlash,
            currentWeapon.muzzleFlash.transform.position, currentWeapon.muzzleFlash.transform.rotation);
        muzzle.gameObject.SetActive(true);
        Destroy(muzzle, 1.0f);
    }

    public void AimStatement()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask, QueryTriggerInteraction.Ignore))
        {
            aimPointPosition.position = raycastHit.point;
        }


        if (Physics.Raycast(ray, out RaycastHit targetRaycastHit, 999f, targetPointLayerMask, QueryTriggerInteraction.Ignore))
        {
            targetPointPosition = targetRaycastHit.point;
        }
        aimRigWeight = isStrafe ? 0.8f : 0f;
        aimRig.weight = Mathf.Clamp(aimRig.weight, 0f, 0.8f);
        aimRig.weight = isStrafe ? 0.8f : 0f;
    }

    public override void Reload()
    {
        if (isReload)
            return;

        if (currentWeapon.curAmmo == 30)
            return;

        StartCoroutine(ReloadCo());
        currentWeapon.curAmmo = currentWeapon.maxAmmo;
    }

    public override void EquipWeapon(DroppedWeapon weapon)
    {
        weapon = droppedWeapon;
        if (weapon != null)
        {
            weapon.Interact(this);
        }
    }


    public void RayWeapon()
    {
        RaycastHit Hit;
        Vector3 direction = Camera.main.transform.forward * 3;

        Debug.DrawRay(Camera.main.transform.position, direction * 3, color: Color.red);
        if (Physics.Raycast(Camera.main.transform.position, direction, out Hit, 3f, droppedWeaponLayerMask))
        {
            if (Hit.collider.TryGetComponent(out droppedWeapon))
            {
                droppedWeapon?.ShowInfoBox(this);
                weapon = droppedWeapon.gameObject;
                
                if(!weapon.Equals(weapon2))
                {
                    Interaction_UI.Instance.HideInfoBox();
                    weapon2 = weapon;
                }  
            }
        }
        else
        {
            droppedWeapon = null;
            Interaction_UI.Instance.HideInfoBox();
        }
    }

    public void UpdateCredit(float value)
    {
        credit += value;
        curCredit += value;
        HUDManager.Instance.creditText.text = PlayerCharacter.Instance.credit.ToString();
    }

    public void ShotgunFire()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            Vector3 aimDir = (targetPointPosition - currentWeapon.fireStartPoint.position).normalized;
            Projectile newBullet = Instantiate(projectilePrefab, currentWeapon.fireStartPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
            newBullet.gameObject.SetActive(true);
            //newBullet.SetForce(projectileSpeed);

            float spreadX = Random.Range(-spreadAngle, spreadAngle);
            float spreadY = Random.Range(-spreadAngle, spreadAngle);

            Quaternion spreadRotation = Quaternion.Euler(spreadX, spreadY, 0);
            Vector3 shootDirection = spreadRotation * aimDir;

            newBullet.GetComponent<Rigidbody>().velocity = shootDirection * projectileSpeed;
        }
    }

    public void NormalFire()
    {
        Vector3 aimDir = (targetPointPosition - currentWeapon.fireStartPoint.position).normalized;
        Projectile newBullet = Instantiate(projectilePrefab, currentWeapon.fireStartPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
        newBullet.gameObject.SetActive(true);
        newBullet.SetForce(projectileSpeed);
        if(explosionCount == 10)
        {
            newBullet.isExplosion = true;
            explosionCount = 0;
        }
    }

    public override void ActiveWeaponSkill()
    {
        currentWeapon.ActiveWeaponSkill();
    }

    public void LaunchGrenade()
    {
        Rigidbody newGrenade = Instantiate(grenadePrefab, currentWeapon.fireStartPoint.position
                                                        , currentWeapon.fireStartPoint.rotation);

        newGrenade.AddForce(newGrenade.transform.forward * grenadeSpeed, ForceMode.Impulse);
    }

}

