using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponAmmoText;

    public Image weaponImage;

    public GameObject inventoryUI;
    public GameObject weaponBox1;
    public GameObject weaponBox2;

    public void Awake()
    {
        Instance = this; 

        weaponImage = GetComponentInChildren<Image>();
    }
    public void OnDestroy()
    {
        Instance = null;
    }


    private void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            OpenInventory();
        }
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

    private void OpenInventory()
    {
        if (inventoryUI.gameObject.activeSelf == false)
        {
            inventoryUI.gameObject.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else
        {
            inventoryUI.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
}
