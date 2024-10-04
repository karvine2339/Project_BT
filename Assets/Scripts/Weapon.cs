using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int CurrentAmmo => curAmmo;
    public int MaxAmmo => maxAmmo;

    public int maxAmmo = 30;
    public int curAmmo = 30;

    public string weaponName;

    public float minDamage;
    public float maxDamage;
    public float fireRate;
    public float criticalProbability;
    public float criticalDamage;

    public Sprite weaponImage;

    public Projectile projectilePrefab;
    public Transform fireStartPoint;

    //bool isReload = false;

    private void Start()
    {
        curAmmo = maxAmmo;
    }

    public void InitWeaponStat()
    {
        PlayerStat.Instance.BulletMinDamage = minDamage;
        PlayerStat.Instance.BulletMaxDamage = maxDamage;
        PlayerStat.Instance.FireRate = fireRate;
        PlayerStat.Instance.CriticalProbability = criticalProbability;
        PlayerStat.Instance.CriticalDamage = criticalDamage;
    }

}
