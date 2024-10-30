using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : UIBase
{
    public static InventoryManager Instance;

    public GameObject inventoryUI;

    public GameObject weaponUI1;
    public GameObject weaponUI2;

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

    public GameObject weaponInventory;
    public GameObject tacticalManualInventory;
    public GameObject oopartsInventory;

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleInventory(GameObject toggleInventory)
    {
        weaponInventory.SetActive(false);
        tacticalManualInventory.SetActive(false);
        oopartsInventory.SetActive(false);

        toggleInventory.SetActive(true);
    }
    public void OpenWeaponInvetory()
    {
        ToggleInventory(weaponInventory);
    }

    public void OpenTacticalManualInventory()
    {
        ToggleInventory(tacticalManualInventory);
    }

    public void OpenOopartsInventory()
    {
        ToggleInventory(oopartsInventory);
    }

    public void OpenInventory()
    {
        if (inventoryUI.gameObject.activeSelf == false)
        {
            inventoryUI.gameObject.SetActive(true);
            BTInputSystem.Instance.isTab = true;
            CursorSystem.Instance.SetCursorState(true);
            OpenWeaponInvetory();
            Time.timeScale = 0.0f;

            if (PlayerCharacter.Instance.weapons[0] == null)
            {
                weaponUI1.gameObject.SetActive(false);
            }
            else
            {
                weaponUI1.gameObject.SetActive(true);
            }

            if (PlayerCharacter.Instance.weapons[1] == null)
            {
                weaponUI2.gameObject.SetActive(false);
            }
            else
            {
                weaponUI2.gameObject.SetActive(true);
            }
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
