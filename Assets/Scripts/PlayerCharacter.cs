using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;


public class PlayerCharacter : ChrBase
{
    public Projectile projectilePrefab;
    public Transform fireStartPoint;
    public float projectileSpeed = 10f;

    protected float aiming = 0f;
    private float aimingBlend = 0f;

    public float fireRate = 0.1f;
    public float reloadTime = 1.3f;
    public bool isReload = false;

    public float credit;
    public float curCredit;

    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private LayerMask targetPointLayerMask = new LayerMask();
    [SerializeField] private Vector3 mouseWorldPosition;
    [SerializeField] private Transform aimPointPosition;
    [SerializeField] private Vector3 targetPointPosition;
    private Rig aimRig;
    private RigBuilder rigBuilder;

    private float aimRigWeight;

    public static PlayerCharacter Instance { get; private set; }

    private InteractionSensor interactionSensor;

    private List<IInteractable> currentInteractionItems = new List<IInteractable>();

    public Transform weaponRoot;
    public Weapon[] weapons = new Weapon[2];

    public Weapon currentWeapon = null;

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

    public void ShowInfoBox()
    {
        if(currentInteractionItems.Count <= 0)
        {
            return;
        }

        Interaction_UI.Instance.ShowInfoBox();
    }

    public void HideInfoBox()
    {
        if(currentInteractionItems.Count <= 0)
        {
            return;
        }

        Interaction_UI.Instance.HideInfoBox();
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

        aimRig = GetComponentInChildren<Rig>();
        rigBuilder = GetComponentInChildren<RigBuilder>();

        aimColliderLayerMask = LayerMask.GetMask("Wall");
        targetPointLayerMask = ~LayerMask.GetMask("Wall");

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

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask, QueryTriggerInteraction.Ignore))
        {
            mouseWorldPosition = raycastHit.point;
            aimPointPosition.position = raycastHit.point;
        }

        if (Physics.Raycast(ray, out RaycastHit targetRaycastHit, 999f, targetPointLayerMask, QueryTriggerInteraction.Ignore))
        {
            targetPointPosition = targetRaycastHit.point;
        }
        aimRigWeight = isStrafe ? 0.8f : 0f;
        aimRig.weight = Mathf.Clamp(aimRig.weight, 0f, 0.8f);
        //aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);
        aimRig.weight = isStrafe ? 0.8f : 0f;

        if(Input.GetKeyDown(KeyCode.R))
        {
            if (isReload)
                return;

            if (currentWeapon.curAmmo == 30)
                return;

            StartCoroutine(Reload());
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
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
        if(isEngaging == false)
        {
            curShield += Time.deltaTime * 10;
            HUDManager.Instance.shieldBar.fillAmount = curShield / maxShield;
            if (curShield >= maxShield)
                curShield = maxShield;

        }

        CreditIncrease();

    }

    public override void Fire()
    {
        if (isStrafe)
        {
            fireRate -= Time.deltaTime;
            if (fireRate <= 0.0f)
            {
                if (currentWeapon.curAmmo == 0)
                {
                    if (isReload)
                        return;

                    StartCoroutine(Reload());
                    return;
                }
                if (isReload == true)
                    return;

                characterAnimator.SetTrigger("Fire");

                Muzzle();
                Vector3 aimDir = (targetPointPosition - currentWeapon.fireStartPoint.position).normalized;
                Projectile newBullet = Instantiate(projectilePrefab, currentWeapon.fireStartPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
                newBullet.gameObject.SetActive(true);
                newBullet.SetForce(projectileSpeed);
                currentWeapon.curAmmo--;
                PlayerStat.Instance.bulletDamage = Random.Range(PlayerStat.Instance.BulletMinDamage, PlayerStat.Instance.BulletMaxDamage);
                fireRate = PlayerStat.Instance.FireRate;
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
                if (weapons[1] != null)
                {
                    HUDManager.Instance.secondWeapon.GetComponent<Image>().sprite = weapons[1].weaponImage;
                    HUDManager.Instance.secondWeapon.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                    HUDManager.Instance.secondWeapon.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
                }

                HUDManager.Instance.weaponIndex.text = "[1]";

                if (weapons[1] != null)
                {
                    weapons[1].gameObject.SetActive(false);
                }

                Debug.Log($"WeaponChanged. Current Weapon = {weapons[0].name}");
            }

            currentWeapon.InitWeaponStat();
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

                if (weapons[0] != null)
                {
                    HUDManager.Instance.firstWeapon.GetComponent<Image>().sprite = weapons[0].weaponImage;
                    HUDManager.Instance.firstWeapon.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                    HUDManager.Instance.firstWeapon.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
        
                }

                HUDManager.Instance.weaponIndex.text = "[2]";

                if (weapons[0] != null)
                {
                    weapons[0].gameObject.SetActive(false);
                }

                Debug.Log($"WeaponChanged. Current Weapon = {weapons[1].name}");
            }
        }
        currentWeapon.InitWeaponStat();
        characterAnimator.SetTrigger("ChangeWeapon");
    }

    IEnumerator Reload()
    {
        isReload = true;
        characterAnimator.SetBool("IsReload",true);
        yield return new WaitForSeconds(reloadTime);
        currentWeapon.curAmmo = currentWeapon.maxAmmo;
        HUDManager.Instance.SetWeaponAmmo(currentWeapon.curAmmo, currentWeapon.maxAmmo);
        characterAnimator.SetBool("IsReload", false);
        isReload = false;
    }

    public void SetStartWeapon()
    {
        currentWeapon = weapons[0];
        weapons[0].minDamage = 50;
        weapons[0].maxDamage = 100;
        weapons[0].criticalProbability = 20f;
        weapons[0].criticalDamage = 1f;
        weapons[0].fireRate = 0.2f;
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

    public void CreditIncrease()
    {
        if (curCredit >= credit)
            return;
        curCredit += Time.deltaTime * 200;
        HUDManager.Instance.UpdateCredit();
        if (curCredit > credit)
            curCredit = credit;
    }
}

