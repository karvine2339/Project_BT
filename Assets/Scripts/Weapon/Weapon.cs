using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public int CurrentAmmo => curAmmo;
    public int MaxAmmo => maxAmmo;

    public int maxAmmo = 30;
    public int curAmmo = 30;

    public string weaponName;

    public string[] effectString = new string[3];
    public int[] effectType;
    public float[] effectVal;

    public float minDamage;
    public float maxDamage;
    public float fireRate;
    public float criticalProbability;
    public float criticalDamage;
    public float weaponRecoilAmount;
    public int weaponUpgradeCount;
    public int weaponType;

    public Sprite weaponImage;

    public Projectile projectilePrefab;
    public Transform fireStartPoint;

    public GameObject muzzleFlash;

    //bool isReload = false;

    private void Start()
    {

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

    public void ApplyUpgrade()
    {

    }
    public void InitFirstWeaponUI()
    {

        HUDManager hudManager = HUDManager.Instance;

        if (weaponUpgradeCount == 0)
        {
            hudManager.weaponName1.text = weaponName;
        }
        else
        {
            hudManager.weaponName1.text = "+" + weaponUpgradeCount + " " + weaponName;
        }

        hudManager.weaponImg1.sprite = weaponImage;
        hudManager.weaponDamage1.text = (minDamage * Mathf.Pow(1.1f,weaponUpgradeCount)).ToString("N0") + " ~ " + 
                                        (maxDamage * Mathf.Pow(1.1f,weaponUpgradeCount)).ToString("N0");
        hudManager.weaponFireRate1.text = fireRate.ToString("N2") + "초 / 발";
        hudManager.weaponEffect1_1.text = effectString[0];
        hudManager.weaponEffect1_2.text = effectString[1];
        hudManager.weaponEffect1_3.text = effectString[2];
    }

    public void InitSecondWeaponUI()
    {
        HUDManager hudManager = HUDManager.Instance;

        if (weaponUpgradeCount == 0)
        {
            hudManager.weaponName2.text = weaponName;
        }
        else
        {
            hudManager.weaponName2.text = "+" + weaponUpgradeCount + " " + weaponName;
        }

        hudManager.weaponImg2.sprite = weaponImage;
        hudManager.weaponDamage2.text = (minDamage * Mathf.Pow(1.1f, weaponUpgradeCount)).ToString("N0") + " ~ " +
                                        (maxDamage * Mathf.Pow(1.1f, weaponUpgradeCount)).ToString("N0");
        hudManager.weaponFireRate2.text = fireRate.ToString("N2") + "초 / 발";
        hudManager.weaponEffect2_1.text = effectString[0];
        hudManager.weaponEffect2_2.text = effectString[1];
        hudManager.weaponEffect2_3.text = effectString[2];
    }

}
