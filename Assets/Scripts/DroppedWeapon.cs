using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedWeapon : MonoBehaviour, IInteractable
{
    public bool IsAutoInteract => false;
    public string Message => weaponName;

    public string weaponName;

    public WeaponData weaponData;

    [SerializeField] private float weaponMinDamage;
    [SerializeField] private float weaponMaxDamage;
    [SerializeField] private float weaponFireRate;

    private void Start()
    {
        InitWeaponData(weaponData);
    }


    public void Interact(ChrBase playerCharacter)
    {

        PlayerStat.Instance.BulletMinDamage = weaponMinDamage;
        PlayerStat.Instance.BulletMaxDamage = weaponMaxDamage;
        PlayerStat.Instance.FireRate = weaponFireRate;

        Interaction_UI.Instance.RemoveInteractionData(this);

        Destroy(gameObject);

    }

    public void ShowInfo(ChrBase playerCharacter)
    {
        Debug.Log(weaponName);
        Debug.Log(weaponData.maxDamage);
        Debug.Log(weaponFireRate);
    }

    public void InitWeaponData(WeaponData weaponData)
    {
        weaponName = weaponData.weaponName;
        weaponMinDamage = weaponData.minDamage;
        weaponMaxDamage = weaponData.maxDamage;
        weaponFireRate = Random.Range(weaponData.minfireRate,weaponData.maxfireRate);
    }
}
