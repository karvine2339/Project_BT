using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int CurrentAmmo => curAmmo;
    public int MaxAmmo => maxAmmo;

    public int maxAmmo;
    public int curAmmo;

    public Projectile projectilePrefab;
    public Transform fireStartPoint;
}
