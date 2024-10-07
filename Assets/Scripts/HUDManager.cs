using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
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


    [Header("--- Inventory UI --- ")]
    public TextMeshProUGUI weaponName1;
    public TextMeshProUGUI weaponName2;
    public TextMeshProUGUI weaponDamage1;
    public TextMeshProUGUI weaponDamage2;
    public TextMeshProUGUI weaponFireRate1;
    public TextMeshProUGUI weaponFireRate2;
    public TextMeshProUGUI weaponEffect1_1;
    public TextMeshProUGUI weaponEffect1_2;
    public TextMeshProUGUI weaponEffect1_3;
    public TextMeshProUGUI weaponEffect2_1;
    public TextMeshProUGUI weaponEffect2_2;
    public TextMeshProUGUI weaponEffect2_3;
    public Image weaponImg1;
    public Image weaponImg2;

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

    public void OpenInventory()
    {
        if (inventoryUI.gameObject.activeSelf == false)
        {
            inventoryUI.gameObject.SetActive(true);
            BTInputSystem.Instance.isTab = true;
            CursorSystem.Instance.SetCursorState(true);
            Time.timeScale = 0.0f;
        }
        else
        {
            inventoryUI.gameObject.SetActive(false);
            BTInputSystem.Instance.isTab = false;
            CursorSystem.Instance.SetCursorState(false);
            Time.timeScale = 1.0f;
        }
    }
}
