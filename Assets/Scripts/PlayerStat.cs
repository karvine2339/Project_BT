using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private int _bulletMinDamage;
    private int _bulletMaxDamage;
    public int BulletMinDamage
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

    public int BulletMaxDamage
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

    public int bulletDamage;

    public delegate void DamageChanged(int newDamage);
    public static event DamageChanged OnDamageChanged;

    public delegate void FireRateChanged(float newFireRate);
    public static event FireRateChanged OnFireRateChanged;

    public static PlayerStat Inst;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
     
    }
}
