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

    private float _criticalProbability;

    public float CriticalProbability
    {
        get { return _criticalProbability; }
        set
        {
            if (_criticalProbability != value)
            {
                _criticalProbability = value;
                OnCriticalChanged?.Invoke(_criticalProbability);
            }
        }
    }

    private float _criticalDamage;
    public float CriticalDamage
    {
        get { return _criticalDamage; }
        set
        {
            if (_criticalDamage != value)
            {
                _criticalDamage = value;
                OnCriticalDamageChanged?.Invoke(_criticalDamage);
            }
        }
    }

    public float bulletDamage;

    public delegate void DamageChanged(float newDamage);
    public static event DamageChanged OnDamageChanged;

    public delegate void FireRateChanged(float newFireRate);
    public static event FireRateChanged OnFireRateChanged;

    public delegate void CriticalChanged(float newCriticalProbability);
    public static event CriticalChanged OnCriticalChanged;

    public delegate void CriticalDamageChanged(float newCriticalDamage);
    public static event CriticalDamageChanged OnCriticalDamageChanged;

    public static PlayerStat Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CriticalProbability = 20f;
        CriticalDamage = 1.0f;
    }
}
