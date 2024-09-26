using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponAmmoText;

    public void Awake()
    {
        Instance = this; 
    }

    public void OnDestroy()
    {
        Instance = null;
    }

    public void SetWeaponInfo(string weaponName, int currentAmmo, int maxAmmo)
    {
        weaponNameText.text = weaponName;
        SetWeaponAmmo(currentAmmo, maxAmmo);
    }

    public void SetWeaponAmmo(int currentAmmo, int maxAmmo)
    {
        weaponAmmoText.text = string.Format("{0} / {1}", currentAmmo, maxAmmo);
    }
}
