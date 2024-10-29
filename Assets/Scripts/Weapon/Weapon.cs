using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using static WeaponData;

public enum EffectType
{
    DamageIncrease = 0,
    FireRateIncrease = 1,
    CriticalProbabilityIncrease = 2,
    CriticalDamageIncrease = 3,
    DamageAndRecoilIncrease = 4,
    FireRateIncreaseAndDamageDecrease = 5
}

public enum WeaponType
{
    Shiroko = 0,
    Serika = 1,
    Akari = 2,
    Saori = 3
}

public class Weapon : MonoBehaviour
{
    public int CurrentAmmo => curAmmo;
    public int MaxAmmo => maxAmmo;

    public int maxAmmo = 30;
    public int curAmmo = 30;

    public string weaponName;

    public string[] effectString = new string[3];
    public EffectType[] effectType = new EffectType[3];
    public float[] effectVal;

    private Coroutine activeSkillCoroutine;

    [HideInInspector] public float minDamage;
    [HideInInspector] public float maxDamage;
    [HideInInspector] public float fireRate;
    [HideInInspector] public float criticalProbability;
    [HideInInspector] public float criticalDamage;
    [HideInInspector] public float weaponRecoilAmount;
    [HideInInspector] public int weaponUpgradeCount;
    [HideInInspector] public WeaponType weaponType;
    [HideInInspector] public float skillCoolDown;
    [HideInInspector] public float skillCoolDuration;

    //초깃값
    [HideInInspector] public float baseMinDamage;
    [HideInInspector] public float baseMaxDamage;
    [HideInInspector] public float baseFireRate;
    [HideInInspector] public float baseCriticalProbability;
    [HideInInspector] public float baseCriticalDamage;
    [HideInInspector] public float baseWeaponRecoilAmount;
    [HideInInspector] public float baseSkillCoolDown;

    public Sprite weaponImage;
    public Sprite skillImage;

    

    public Projectile projectilePrefab;
    public Transform fireStartPoint;

    public GameObject muzzleFlash;

    //bool isReload = false;

    private void Start()
    {
        ApplyEffects();
    }

    private void Update()
    {
        CheckCoolDown();
    }

    public void InitWeaponStat()
    {
        PlayerStat playerStat = PlayerStat.Instance;
        playerStat.BulletMinDamage = minDamage * Mathf.Pow(1.1f, weaponUpgradeCount);
        playerStat.BulletMaxDamage = maxDamage * Mathf.Pow(1.1f, weaponUpgradeCount);
        playerStat.FireRate = fireRate;
        playerStat.CriticalProbability = criticalProbability;
        playerStat.CriticalDamage = criticalDamage;
        playerStat.RecoilAmount = weaponRecoilAmount;
    }

    public void InitFirstWeaponUI()
    {

        InventoryManager inventoryManager = InventoryManager.Instance;

        if (weaponUpgradeCount == 0)
        {
            inventoryManager.weaponName1.text = weaponName;
        }
        else
        {
            inventoryManager.weaponName1.text = "+" + weaponUpgradeCount + " " + weaponName;
        }

        inventoryManager.weaponImg1.sprite = weaponImage;
        inventoryManager.weaponDamage1.text = (baseMinDamage * Mathf.Pow(1.1f,weaponUpgradeCount)).ToString("N0") + " ~ " + 
                                        (baseMaxDamage * Mathf.Pow(1.1f,weaponUpgradeCount)).ToString("N0");
        inventoryManager.weaponFireRate1.text = baseFireRate.ToString("N2") + "초 / 발";
        inventoryManager.weaponEffect1_1.text = effectString[0];
        inventoryManager.weaponEffect1_2.text = effectString[1];
        inventoryManager.weaponEffect1_3.text = effectString[2];
    }

    public void InitSecondWeaponUI()
    {
        InventoryManager inventoryManager = InventoryManager.Instance;

        if (weaponUpgradeCount == 0)
        {
            inventoryManager.weaponName2.text = weaponName;
        }
        else
        {
            inventoryManager.weaponName2.text = "+" + weaponUpgradeCount + " " + weaponName;
        }

        inventoryManager.weaponImg2.sprite = weaponImage;
        inventoryManager.weaponDamage2.text = (baseMinDamage * Mathf.Pow(1.1f, weaponUpgradeCount)).ToString("N0") + " ~ " +
                                        (baseMaxDamage * Mathf.Pow(1.1f, weaponUpgradeCount)).ToString("N0");
        inventoryManager.weaponFireRate2.text = baseFireRate.ToString("N2") + "초 / 발";
        inventoryManager.weaponEffect2_1.text = effectString[0];
        inventoryManager.weaponEffect2_2.text = effectString[1];
        inventoryManager.weaponEffect2_3.text = effectString[2];
    }

    public void ApplyEffects()
    {
        BaseStats();

        
        for (int i = 0; i < effectType.Length; i++)
        {
            ApplyEffectType(effectType[i], effectVal[i]);
        }

        InitWeaponStat();
    }

    public void BaseStats()
    {
        minDamage = baseMinDamage;
        maxDamage = baseMaxDamage;
        fireRate = baseFireRate;
        criticalProbability = baseCriticalProbability;
        criticalDamage = baseCriticalDamage;
        weaponRecoilAmount = baseWeaponRecoilAmount;
        skillCoolDown = baseSkillCoolDown;
    }

    public void ApplyEffectType(EffectType effectType, float value)
    {
        switch(effectType)
        {
            case EffectType.DamageIncrease:
                minDamage *= 1 + (value / 100);
                maxDamage *= 1 + (value / 100);
                break;

            case EffectType.FireRateIncrease:
                fireRate /= 1 + value / 100;
                break;

            case EffectType.CriticalProbabilityIncrease:
                criticalProbability += value;
                break;

            case EffectType.CriticalDamageIncrease:
                criticalDamage += value / 100;
                break;

            case EffectType.DamageAndRecoilIncrease:
                minDamage *= 1 + (value / 100);
                maxDamage *= 1 + (value / 100);
                weaponRecoilAmount += (value / 2) / 100;
                break;

            case EffectType.FireRateIncreaseAndDamageDecrease:
                minDamage *= 1 - (value / 100) / 4;
                maxDamage *= 1 - (value / 100) / 4;
                fireRate /= 1 + value / 100;
                break;

        }
    }

    public void ActiveWeaponSkill()
    {
        if (skillCoolDuration > 0)
            return;

        skillCoolDuration = skillCoolDown;
        HUDManager.Instance.skillImageMask.fillAmount = 1.0f;

        switch (weaponType)
        {
            case WeaponType.Shiroko:
                activeSkillCoroutine = StartCoroutine(DroneBoost(0.1f, 10));
                break;

            case WeaponType.Serika:
                activeSkillCoroutine = StartCoroutine(DamageBoost(100, 10));
                break;

            case WeaponType.Akari:
               PlayerCharacter.Instance.isGrenade = !PlayerCharacter.Instance.isGrenade;

                break;

            case WeaponType.Saori:
                activeSkillCoroutine = StartCoroutine(CriticalBoost(100,10));
                break;

        }
    }


    public void CheckCoolDown()
    {
        if (skillCoolDuration > 0.0f)
        {
            if (skillCoolDuration <= 0.0f)
                return;

            skillCoolDuration -= Time.deltaTime;

            HUDManager.Instance.skillImageMask.fillAmount = skillCoolDuration / skillCoolDown;
        }
    }

    private IEnumerator DamageBoost(float damageBoost,float duration)
    {
        float originalMinDamage = minDamage;
        float originalMaxDamage = maxDamage;

        minDamage *= 1 + (damageBoost / 100);
        maxDamage *= 1 + (damageBoost / 100);

        InitWeaponStat();

        yield return new WaitForSeconds(duration);

        ResetDamage(originalMinDamage,originalMaxDamage);

        activeSkillCoroutine = null;
    }

    private IEnumerator CriticalBoost(float criticalBoost,float duration)
    {
        float originalCriticalProbability = criticalProbability;

        criticalProbability += criticalBoost;

        InitWeaponStat();

        yield return new WaitForSeconds(duration);

        ResetCritical(originalCriticalProbability);

        activeSkillCoroutine = null;
    }

    private IEnumerator DroneBoost(float droneRocketRate,float duration)
    {
        float originalDroneRocketRate = DronCtrl.Instance.newRocketRate;

        DronCtrl.Instance.newRocketRate = droneRocketRate;
        DronCtrl.Instance.maxRocketDelay = 0.0f;

        yield return new WaitForSeconds(duration);

        DronCtrl.Instance.newRocketRate = originalDroneRocketRate;
        DronCtrl.Instance.maxRocketDelay = 3.0f;

        activeSkillCoroutine = null;
    }

    public void ResetCritical(float originalCriticalProbability)
    {
        criticalProbability = originalCriticalProbability;
        InitWeaponStat();
    }
    public void ResetDamage(float originalMinDamage, float originalMaxDamage)
    {
        minDamage = originalMinDamage;
        maxDamage = originalMaxDamage;

        InitWeaponStat();
    }

    private void OnEnable()
    {
        if (skillCoolDuration <= 0)
        {
            HUDManager.Instance.skillImageMask.fillAmount = 0.0f;
        }

    }
    private void OnDisable()
    {
        if (activeSkillCoroutine != null)
        {
            StopCoroutine(activeSkillCoroutine);
            ResetCritical(baseCriticalProbability);
            ResetDamage(baseMinDamage, baseMaxDamage);
            DronCtrl.Instance.newRocketRate = 0.2f;
            DronCtrl.Instance.maxRocketDelay = 3.0f;
        }
    }

    private void OnDestroy()
    {
        if (activeSkillCoroutine != null)
        {
            StopCoroutine(activeSkillCoroutine);
            ResetCritical(baseCriticalProbability);
            ResetDamage(baseMinDamage, baseMaxDamage);
            DronCtrl.Instance.newRocketRate = 0.2f;
            DronCtrl.Instance.maxRocketDelay = 3.0f;
        }
    }
}
