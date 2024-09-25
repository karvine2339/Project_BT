using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private float _bulletMinDamage;
    private float _bulletMaxDamage;
    public float BulletMinDamage
    {
        get { return _bulletMinDamage; }
        set
        {
            if (_bulletMinDamage != value)
            {
                _bulletMinDamage = value;
                OnDamageChanged?.Invoke(_bulletMinDamage);
            }
        }
    }

    public float BulletMaxDamage
    {
        get { return _bulletMaxDamage + 1; }
        set
        {
            if (_bulletMaxDamage != value)
            {
                _bulletMaxDamage = value;
                OnDamageChanged?.Invoke(_bulletMaxDamage);
            }
        }
    }



    private float _fireRate;

    public float FireRate
    {
        get { return _fireRate; }
        set
        {
            if(_fireRate != value)
            {
                _fireRate = value;
                OnFireRateChanged?.Invoke(_fireRate);
            }
        }
    }

    public float bulletDamage;

    public delegate void DamageChanged(float newDamage);
    public static event DamageChanged OnDamageChanged;

    public delegate void FireRateChanged(float newFireRate);
    public static event FireRateChanged OnFireRateChanged;

    public static PlayerStat Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
     
    }
}
