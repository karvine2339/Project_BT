using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : UIBase
{
    public static HUDManager Instance;

    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponAmmoText;

    public Image curWeaponImage;
    public TextMeshProUGUI weaponIndex;

    public GameObject firstWeapon;
    public GameObject secondWeapon;

    public Image hpBar;
    public Image shieldBar;

    public TextMeshProUGUI creditText;
    public TextMeshProUGUI increaseCreditText;

    public float creditTextTime = 5.0f;
    public bool isIncreaseCredit = false;



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
