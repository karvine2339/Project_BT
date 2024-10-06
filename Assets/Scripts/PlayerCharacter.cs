using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
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
    bool isReload = false;

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
        aimRig = GetComponentInChildren<Rig>();
        rigBuilder = GetComponentInChildren<RigBuilder>();

        aimColliderLayerMask = LayerMask.GetMask("Wall");
        targetPointLayerMask = ~LayerMask.GetMask("Wall");

        weapons[0] = GetComponentInChildren<Weapon>();
        currentWeapon = weapons[0];

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
            StartCoroutine(Reload());
        }
        
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

            if (weapons[0].gameObject != null && weapons[0].gameObject.activeSelf == false)
            {
                if (weapons[0].gameObject.activeSelf == false)
                {
                    weapons[0].gameObject.SetActive(true);
                    currentWeapon = weapons[0];
                }

                HUDManager.Instance.SetWeaponInfo(weapons[0].weaponName, weapons[0].CurrentAmmo, weapons[0].MaxAmmo);
                HUDManager.Instance.SetWeaponAmmo(weapons[0].CurrentAmmo, weapons[0].MaxAmmo);
                HUDManager.Instance.weaponImage.sprite = weapons[0].weaponImage;

                if (weapons[1].gameObject.activeSelf == true)
                {
                    weapons[1].gameObject.SetActive(false);
                }

                Debug.Log($"WeaponChanged. Current Weapon = {weapons[0].name}");
            }
        }
    }



    public override void ChangedSecondaryWeapon()
    {
        if (weapons[1] != null)
        {
            if (isReload)
                return;

            weapons[1].gameObject.SetActive(true);
            currentWeapon = weapons[1];

            if (weapons[0].gameObject.activeSelf == true)
            {
                weapons[0].gameObject.SetActive(false);
            }
            HUDManager.Instance.SetWeaponInfo(weapons[1].weaponName, weapons[1].CurrentAmmo, weapons[1].MaxAmmo);
            HUDManager.Instance.SetWeaponAmmo(weapons[1].CurrentAmmo, weapons[1].MaxAmmo);
            HUDManager.Instance.weaponImage.sprite = weapons[1].weaponImage;

            Debug.Log($"WeaponChanged. Current Weapon = {weapons[1].name}");
        }
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
}

