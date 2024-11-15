using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkShopManager : UIBase
{
    public static WorkShopManager Instance;

    public GameObject workShopObject;

    public EffectType newEffectType;

    private float upgradePrice = 5000;
    private float effectChangePrice = 2500;

    [Header("--- First Weapon ---")]
    public TextMeshProUGUI weaponName1;
    public TextMeshProUGUI weaponDamage1;
    public TextMeshProUGUI weaponFireRate1;
    public TextMeshProUGUI weaponEffect1_1;
    public TextMeshProUGUI weaponEffect1_2;
    public TextMeshProUGUI weaponEffect1_3;
    public Image weaponImg1;
    public GameObject weaponInfoBox1;
    public TextMeshProUGUI weapon1UpgradePriceText;
    public TextMeshProUGUI weapon1EffectChangePriceText;

    [Header("--- Second Weapon ---")]
    public TextMeshProUGUI weaponName2;
    public TextMeshProUGUI weaponDamage2;
    public TextMeshProUGUI weaponFireRate2;
    public TextMeshProUGUI weaponEffect2_1;
    public TextMeshProUGUI weaponEffect2_2;
    public TextMeshProUGUI weaponEffect2_3;
    public Image weaponImg2;
    public GameObject weaponInfoBox2;
    public TextMeshProUGUI weapon2UpgradePriceText;
    public TextMeshProUGUI weapon2EffectChangePriceText;

    public TextMeshProUGUI credit;

    private void Awake()
    {
        Instance = this;
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

        if (weapon.weaponUpgradeCount == 0)
        {
            weaponName1.text = weapon.weaponName;
        }
        else
        {
            weaponName1.text = "+" + weapon.weaponUpgradeCount + " " + weapon.weaponName;
        }

        weaponImg1.sprite = weapon.weaponImage;
        weaponDamage1.text = (weapon.baseMinDamage * Mathf.Pow(1.1f, weapon.weaponUpgradeCount)).ToString("N0") + " ~ " +
                             (weapon.baseMaxDamage * Mathf.Pow(1.1f, weapon.weaponUpgradeCount)).ToString("N0");
        weaponFireRate1.text = weapon.baseFireRate.ToString("N2") + "초 / 발";
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
        weaponDamage2.text = (weapon.baseMinDamage * Mathf.Pow(1.1f, weapon.weaponUpgradeCount)).ToString("N0") + " ~ " +
                             (weapon.baseMaxDamage * Mathf.Pow(1.1f, weapon.weaponUpgradeCount)).ToString("N0");
        weaponFireRate2.text = weapon.baseFireRate.ToString("N2") + "초 / 발";
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

    private void Update()
    {

    }

    public void UpdateCredit()
    {
        credit.text = PlayerCharacter.Instance.credit.ToString();
    }
    public void Weapon1UpgradeButtonClick()
    {
        if (PlayerCharacter.Instance.credit < PlayerCharacter.Instance.CalculateOopartsValue(upgradePrice,
                                                                PlayerCharacter.Instance.DecreaseShopPrice))
        {

            return;
        }

        PlayerCharacter.Instance.UpdateCredit(-PlayerCharacter.Instance.CalculateOopartsValue(upgradePrice,
                                                                PlayerCharacter.Instance.DecreaseShopPrice));

        this.UpdateCredit();

        Weapon weapon = PlayerCharacter.Instance.weapons[0];

        weapon.weaponUpgradeCount++;
        weapon.InitFirstWeaponUI();
        weapon.InitWeaponStat();
        this.InitFirstWeapon();
    }
    public void Weapon2UpgradeButtonClick()
    {
        if (PlayerCharacter.Instance.credit < PlayerCharacter.Instance.CalculateOopartsValue(upgradePrice,
                                                                PlayerCharacter.Instance.DecreaseShopPrice))
        {

            return;
        }

        PlayerCharacter.Instance.UpdateCredit(-PlayerCharacter.Instance.CalculateOopartsValue(upgradePrice,
                                                                PlayerCharacter.Instance.DecreaseShopPrice));

        this.UpdateCredit();

        Weapon weapon = PlayerCharacter.Instance.weapons[1];

        weapon.weaponUpgradeCount++;
        weapon.InitSecondWeaponUI();
        weapon.InitWeaponStat();
        this.InitSecondWeapon();
    }

    public void Weapon1ChangeEffectButtonClick()
    {
        if (PlayerCharacter.Instance.credit < PlayerCharacter.Instance.CalculateOopartsValue(effectChangePrice,
                                                                PlayerCharacter.Instance.DecreaseShopPrice))
        {

            return;
        }

        PlayerCharacter.Instance.UpdateCredit(-PlayerCharacter.Instance.CalculateOopartsValue(effectChangePrice,
                                                                PlayerCharacter.Instance.DecreaseShopPrice));

        this.UpdateCredit();

        Weapon weapon = PlayerCharacter.Instance.weapons[0];

        if (weapon.effectType.Length < 3)
            return;

        WeaponEffectManager.Instance.InitWeaponEffect(weapon.effectType, weapon.effectVal, weapon.effectString);
        weapon.InitFirstWeaponUI();
        weapon.InitWeaponStat();
        weapon.ApplyEffects();
        this.InitFirstWeapon();
    }

    public void Weapon2ChangeEffectButtonClick()
    {
        if (PlayerCharacter.Instance.credit < PlayerCharacter.Instance.CalculateOopartsValue(effectChangePrice,
                                                                PlayerCharacter.Instance.DecreaseShopPrice))
        {

            return;
        }

        PlayerCharacter.Instance.UpdateCredit(-PlayerCharacter.Instance.CalculateOopartsValue(effectChangePrice,
                                                                PlayerCharacter.Instance.DecreaseShopPrice));
        this.UpdateCredit();

        Weapon weapon = PlayerCharacter.Instance.weapons[1];

        if (weapon.effectType.Length < 3)
            return;

        WeaponEffectManager.Instance.InitWeaponEffect(weapon.effectType, weapon.effectVal, weapon.effectString);
        weapon.InitSecondWeaponUI();
        weapon.InitWeaponStat();
        weapon.ApplyEffects();
        this.InitSecondWeapon();
    }

    public void SetPriceText()
    {
        weapon1UpgradePriceText.text = PlayerCharacter.Instance.CalculateOopartsValue(upgradePrice,
                                                                PlayerCharacter.Instance.DecreaseShopPrice).ToString();
        weapon1EffectChangePriceText.text = PlayerCharacter.Instance.CalculateOopartsValue(effectChangePrice,
                                                                PlayerCharacter.Instance.DecreaseShopPrice).ToString();
        weapon2UpgradePriceText.text = PlayerCharacter.Instance.CalculateOopartsValue(upgradePrice,
                                                                PlayerCharacter.Instance.DecreaseShopPrice).ToString();
        weapon2EffectChangePriceText.text = PlayerCharacter.Instance.CalculateOopartsValue(effectChangePrice,
                                                                PlayerCharacter.Instance.DecreaseShopPrice).ToString();

    }
}
