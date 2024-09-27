using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public string weaponType;
    public float minDamage;
    public float maxDamage;
    public float minfireRate;
    public float maxfireRate;

}
