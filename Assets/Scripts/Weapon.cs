using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public int weaponType;

    public Sprite weaponImage;

    public Projectile projectilePrefab;
    public Transform fireStartPoint;

    //bool isReload = false;

    private void Start()
    {

    }

    public void InitWeaponStat()
    {
        PlayerStat.Instance.BulletMinDamage = minDamage;
        PlayerStat.Instance.BulletMaxDamage = maxDamage;
        PlayerStat.Instance.FireRate = fireRate;
        PlayerStat.Instance.CriticalProbability = criticalProbability;
        PlayerStat.Instance.CriticalDamage = criticalDamage;
        PlayerStat.Instance.RecoilAmount = weaponRecoilAmount;
    }
    public void InitFirstWeaponUI()
    {
        HUDManager.Instance.weaponName1.text = weaponName;
        HUDManager.Instance.weaponImg1.sprite = weaponImage;
        HUDManager.Instance.weaponDamage1.text = minDamage.ToString("N0") + " ~ " + maxDamage.ToString("N0");
        HUDManager.Instance.weaponFireRate1.text = fireRate.ToString("N2") + "초 / 발";
        HUDManager.Instance.weaponEffect1_1.text = effectString[0];
        HUDManager.Instance.weaponEffect1_2.text = effectString[1];
        HUDManager.Instance.weaponEffect1_3.text = effectString[2];
    }

    public void InitSecondWeaponUI()
    {
        HUDManager.Instance.weaponName2.text = weaponName;
        HUDManager.Instance.weaponImg2.sprite = weaponImage;
        HUDManager.Instance.weaponDamage2.text = minDamage.ToString("N0") + " ~ " + maxDamage.ToString("N0");
        HUDManager.Instance.weaponFireRate2.text = fireRate.ToString("N2") + "초 / 발";
        HUDManager.Instance.weaponEffect2_1.text = effectString[0];
        HUDManager.Instance.weaponEffect2_2.text = effectString[1];
        HUDManager.Instance.weaponEffect2_3.text = effectString[2];
    }

}
