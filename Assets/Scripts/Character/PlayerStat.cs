using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private float _bulletMinDamage;
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

    private float _bulletMaxDamage;
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
            if (_fireRate != value)
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

    private float _recoilAmount;
    public float RecoilAmount
    {
        get { return _recoilAmount; }
        set
        {
            if (_recoilAmount != value)
            {
                _recoilAmount = value;
                OnRecoilAmountChanged?.Invoke(_recoilAmount);
            }
        }
    }

    private float _additionalDamage;
    public float AdditionalDamage
    {
        get { return _additionalDamage; }
        set
        {
            if (_additionalDamage != value)
            {
                _additionalDamage = value;
                OnAdditionalDamageChanged?.Invoke(_additionalDamage);
            }
        }
    }
    private float _additionalBulletDamage;
    public float AdditionalBulletDamage
    {
        get { return _additionalBulletDamage; }
        set
        {
            if (_additionalBulletDamage != value)
            {
                _additionalBulletDamage = value;
                OnAdditionalBulletDamageChanged?.Invoke(_additionalBulletDamage);
            }
        }
    }

    private float _droneDamage;
    public float DroneDamage
    {
        get { return _droneDamage; }
        set
        {
            if (_droneDamage != value)
            {
                _droneDamage = value;
                OnDroneDamageChanged?.Invoke(_droneDamage);
            }
        }
    }

    private float _oopartsDamage;

    public float OopartsDamage
    {
        get { return _oopartsDamage; }
        set
        {
            if(_oopartsDamage != value)
            {
                _oopartsDamage = value;
                OnOopartsDamageChanged?.Invoke(_oopartsDamage);
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

    public delegate void RecoilAmountChanged(float newRecoilAmount);
    public static event RecoilAmountChanged OnRecoilAmountChanged;

    public delegate void AdditionalDamageChanged(float newAdditionalDamage);
    public static event AdditionalDamageChanged OnAdditionalDamageChanged;

    public delegate void AdditionalBulletDamageChanged(float newAddtionalBulletDamage);
    public static event AdditionalBulletDamageChanged OnAdditionalBulletDamageChanged;

    public delegate void DroneDamageChanged(float newDroneDamage);
    public static event DroneDamageChanged OnDroneDamageChanged;

    public delegate void OopartsDamageChanged(float newOopartsDamage);
    public static event OopartsDamageChanged OnOopartsDamageChanged;

    public static PlayerStat Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        BulletMinDamage = 0;
        BulletMaxDamage = 0;
        FireRate = 0;
        CriticalProbability = 10;
        CriticalDamage = 1.5f;
        RecoilAmount = 0;
        AdditionalDamage = 0;
        AdditionalBulletDamage = 1.0f;
        DroneDamage = 1.0f;
        OopartsDamage = 1.0f;
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}
