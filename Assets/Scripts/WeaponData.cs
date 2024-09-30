using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{ 
    public string effectName;
    public int minValue;
    public int maxValue;
    public float chance;

    public class EffectTable
    {
        public string name;
        public List<WeaponData> effects;
    }

    public class EffectData
    {
        public List<EffectTable> table;
    }

    private EffectData effectData;

    public string weaponName;
    public string weaponType;
    public float minDamage;
    public float maxDamage;
    public float minfireRate;
    public float maxfireRate;

    public Sprite weaponImg;
}
