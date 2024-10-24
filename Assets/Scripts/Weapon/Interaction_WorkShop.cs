using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interaction_WorkShop : MonoBehaviour,IInteractable
{
    public GameObject workShopObject;


    [Header("--- First Weapon ---")]
    public TextMeshProUGUI weaponName1;
    public TextMeshProUGUI weaponDamage1;
    public TextMeshProUGUI weaponFireRate1;
    public TextMeshProUGUI weaponEffect1_1;
    public TextMeshProUGUI weaponEffect1_2;
    public TextMeshProUGUI weaponEffect1_3;
    public Image weaponImg1;
    public GameObject weaponInfoBox1;

    [Header("--- Second Weapon ---")]
    public TextMeshProUGUI weaponName2;
    public TextMeshProUGUI weaponDamage2;
    public TextMeshProUGUI weaponFireRate2;
    public TextMeshProUGUI weaponEffect2_1;
    public TextMeshProUGUI weaponEffect2_2;
    public TextMeshProUGUI weaponEffect2_3;
    public Image weaponImg2;
    public GameObject weaponInfoBox2;

    public bool IsAutoInteract => false;

    public string Message => "워크샵";

    public void Interact(PlayerCharacter playerCharacter)
    {
        Time.timeScale = 0.0f;
        CursorSystem.Instance.SetCursorState(true);
        workShopObject.SetActive(true);

        InitFirstWeapon();
        InitSecondWeapon();

        BTInputSystem.Instance.isWorkShop = true;

    }

    public void InitFirstWeapon()
    {
        Weapon weapon = PlayerCharacter.Instance.weapons[0];

        if (weapon == null)
        {
            weaponInfoBox1.SetActive(false);
            return;
        }
        else
        {
            weaponInfoBox1.SetActive(true);
        }

        if(weapon.weaponUpgradeCount == 0)
        {
            weaponName1.text = weapon.weaponName;
        }
        else
        {
            weaponName1.text = "+" + weapon.weaponUpgradeCount + " " + weapon.weaponName;
        }

        weaponImg1.sprite = weapon.weaponImage;
        weaponDamage1.text = (weapon.minDamage * Mathf.Pow(1.1f, weapon.weaponUpgradeCount)).ToString("N0") + " ~ " +
                             (weapon.maxDamage * Mathf.Pow(1.1f, weapon.weaponUpgradeCount)).ToString("N0");
        weaponFireRate1.text = weapon.fireRate.ToString("N2") + "초 / 발";
        weaponEffect1_1.text = weapon.effectString[0];
        weaponEffect1_2.text = weapon.effectString[1];
        weaponEffect1_3.text = weapon.effectString[2];
    }

    public void InitSecondWeapon()
    {
        Weapon weapon = PlayerCharacter.Instance.weapons[1];

        if (weapon == null)
        {
            weaponInfoBox2.SetActive(false);
            return;
        }
        else
        {   
            weaponInfoBox2.SetActive(true);
        }

        if (weapon.weaponUpgradeCount == 0)
        {
            weaponName2.text = weapon.weaponName;
        }
        else
        {
            weaponName2.text = "+" + weapon.weaponUpgradeCount + " " + weapon.weaponName;
        }

        weaponImg2.sprite = weapon.weaponImage;
        weaponDamage2.text = (weapon.minDamage * Mathf.Pow(1.1f, weapon.weaponUpgradeCount)).ToString("N0") + " ~ " +
                             (weapon.maxDamage * Mathf.Pow(1.1f, weapon.weaponUpgradeCount)).ToString("N0");
        weaponFireRate2.text = weapon.fireRate.ToString("N2") + "초 / 발";
        weaponEffect2_1.text = weapon.effectString[0];
        weaponEffect2_2.text = weapon.effectString[1];
        weaponEffect2_3.text = weapon.effectString[2];
    }

    public void BackButtonClick()
    {
        CursorSystem.Instance.SetCursorState(false);
        workShopObject.SetActive(false);
        BTInputSystem.Instance.isWorkShop = false;
        Time.timeScale = 1.0f;
    }

    public void Weapon1UpgradeButtonClick()
    {
        Weapon weapon = PlayerCharacter.Instance.weapons[0];

        weapon.weaponUpgradeCount++;
        weapon.InitFirstWeaponUI();
        weapon.InitWeaponStat();
        this.InitFirstWeapon();
    }
    public void Weapon2UpgradeButtonClick()
    {
        Weapon weapon = PlayerCharacter.Instance.weapons[1];

        weapon.weaponUpgradeCount++;
        weapon.InitSecondWeaponUI();
        weapon.InitWeaponStat();
        this.InitSecondWeapon();
    }
}
