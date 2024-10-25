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

    public Image curWeaponImage;
    public TextMeshProUGUI weaponIndex;

    public GameObject inventoryUI;

    public GameObject weaponUI1;
    public GameObject weaponUI2;

    public GameObject firstWeapon;
    public GameObject secondWeapon;

    public Image hpBar;
    public Image shieldBar;

    public TextMeshProUGUI creditText;
    public TextMeshProUGUI increaseCreditText;

    public float creditTextTime = 5.0f;
    public bool isIncreaseCredit = false;

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
    }

    public void Start()
    {
        hpBar.fillAmount = 1.0f;
        shieldBar.fillAmount = 1.0f;
        creditText.text = PlayerCharacter.Instance.curCredit.ToString("N0");
        secondWeapon.gameObject.SetActive(false);
    }
    public void OnDestroy()
    {
        Instance = null;
    }


    private void Update()
    {
        if(creditTextTime >= 5.0f)
        {
            creditTextTime -= Time.deltaTime;
            if (creditTextTime <= 0.0f)
            {
                increaseCreditText.text = "";
            }
        }

        if(isIncreaseCredit)
        {
            IncreaseCredit();
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

    public void OpenInventory()
    {
        if (inventoryUI.gameObject.activeSelf == false)
        {
            inventoryUI.gameObject.SetActive(true);
            BTInputSystem.Instance.isTab = true;
            CursorSystem.Instance.SetCursorState(true);
            Time.timeScale = 0.0f;

            if (PlayerCharacter.Instance.weapons[0] == null)
            {
                weaponUI1.gameObject.SetActive(false);
            }
            else
            {
                weaponUI1.gameObject.SetActive(true);
            }
            
            if(PlayerCharacter.Instance.weapons[1] == null)
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

    public void UpdateCredit(int credit)
    {
        creditTextTime = 5.0f;
        increaseCreditText.text = "+" + credit;
        isIncreaseCredit = true;
    }

    public void IncreaseCredit()
    {
        if (PlayerCharacter.Instance.curCredit >= PlayerCharacter.Instance.credit)
            return;

        PlayerCharacter.Instance.curCredit += Time.unscaledDeltaTime * 250;

        creditText.text = PlayerCharacter.Instance.curCredit.ToString("N0");

        if (PlayerCharacter.Instance.curCredit > PlayerCharacter.Instance.credit)
        {
            PlayerCharacter.Instance.curCredit = PlayerCharacter.Instance.credit;
            isIncreaseCredit = false;
        }
    }

}
