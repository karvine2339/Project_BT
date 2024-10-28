using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Random = UnityEngine.Random;
using static UnityEngine.Rendering.DebugUI;

public class DroppedWeapon : MonoBehaviour
{
    public JsonManager jsonManager;

    public JsonManager.Root effectData;

    public bool IsAutoInteract => false;
    public string Message => weaponName;

    public string weaponName;

    public WeaponData weaponData;

    public Sprite weaponImg;

    private GameObject weaponInfoBox;
    WeaponInfoBox weaponInfo;

    private float rotationSpeed = 100;

    public GameObject weaponObject;
    Dictionary<string, string> weaponPrefabPaths = new Dictionary<string, string>()
    {
        {"Weapon_Shiroko_Dropped(Clone)" , "Weapon_Shiroko" },
        {"Weapon_Serika_Dropped(Clone)", "Weapon_Serika" }
    };

    [HideInInspector] public float weaponMinDamage;
    [HideInInspector] public float weaponMaxDamage;
    [HideInInspector] public float weaponFireRate;
    [HideInInspector] public float weaponCriticalProbability = 10.0f;
    [HideInInspector] public float weaponCriticalDamage = 1.5f;
    [HideInInspector] public float[] effectVal = new float[3];
    [HideInInspector] public EffectType[] effectType = new EffectType[3];
    [HideInInspector] public string[] effectString = new string[3];
    [HideInInspector] public int effectIndex;
    [HideInInspector] public WeaponType weaponType;
    [HideInInspector] public int weaponUpgradeCount;
    [HideInInspector] public float weaponRecoilAmount = 1.0f;

    [HideInInspector] public bool isThrow = false;


    private void Awake()
    {
        effectData = JsonManager.Instance.effectData;   
    }
    private void Start()
    {
        weaponInfoBox = Interaction_UI.Instance.weaponInfoBox;
        weaponInfo = weaponInfoBox.GetComponent<WeaponInfoBox>();

        if (isThrow == false)
        {
            WeaponEffectManager.Instance.InitWeaponEffect(effectType,effectVal,effectString);
            InitWeaponUpgrade();
        }

        
    }

    private void Update()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationAmount, 0,Space.World);
    }
    public void InteractWeapon(RaycastHit hit,PlayerCharacter playerCharacter)
    {
        Debug.Log(playerCharacter.currentWeapon);
    }
    public void Interact(PlayerCharacter playerCharacter)
    {
        string prefabPaths = weaponPrefabPaths[this.gameObject.name];
        weaponObject = Resources.Load(prefabPaths) as GameObject;

        GameObject newWeaponInstance = Instantiate(weaponObject, playerCharacter.weaponRoot);
        Weapon newWeapon = newWeaponInstance.GetComponent<Weapon>();

        newWeapon.curAmmo = 0;

        if (playerCharacter.weapons[0] == null || playerCharacter.weapons[1] == null)
        {
            if (playerCharacter.weapons[0] == null)
            {
                playerCharacter.weapons[0] = newWeapon;
                playerCharacter.currentWeapon = playerCharacter.weapons[0];
                ApplyWeaponStats(playerCharacter);
                playerCharacter.ChangedPrimaryWeapon();
                HUDManager.Instance.firstWeapon.SetActive(true);

                newWeapon.InitFirstWeaponUI();
            }
            else if (playerCharacter.weapons[1] == null)
            {
                playerCharacter.weapons[1] = newWeapon;
                playerCharacter.currentWeapon = playerCharacter.weapons[1];
                ApplyWeaponStats(playerCharacter);
                playerCharacter.ChangedSecondaryWeapon();
                HUDManager.Instance.secondWeapon.SetActive(true);
                newWeapon.InitSecondWeaponUI();
            }

  
        }

        else
        {
            if (playerCharacter.currentWeapon == playerCharacter.weapons[0])
            {
                WeaponDropManager.Instance.ThrowEquippedWeapon(this.transform.position, 0);
                Destroy(playerCharacter.weapons[0].gameObject);
                playerCharacter.weapons[0] = newWeapon;
                playerCharacter.currentWeapon = playerCharacter.weapons[0];
                ApplyWeaponStats(playerCharacter);
                playerCharacter.ChangedPrimaryWeapon();
                newWeapon.InitFirstWeaponUI();
     
            }
            else if (playerCharacter.currentWeapon == playerCharacter.weapons[1])
            {
                WeaponDropManager.Instance.ThrowEquippedWeapon(this.transform.position, 1);
                Destroy(playerCharacter.weapons[1].gameObject);
                playerCharacter.weapons[1] = newWeapon;
                playerCharacter.currentWeapon = playerCharacter.weapons[1];
                ApplyWeaponStats(playerCharacter);
                playerCharacter.ChangedSecondaryWeapon();
                newWeapon.InitSecondWeaponUI();

            }

        }

        Destroy(gameObject);

    }

    public void ShowInfoBox(PlayerCharacter playerCharacter)
    {
        if(weaponInfoBox != null && weaponInfoBox.gameObject.activeSelf == false)
        {
            weaponInfoBox.gameObject.SetActive(true);
            weaponInfo.weaponDamageText.text = (weaponMinDamage * Mathf.Pow(1.1f, weaponUpgradeCount)).ToString("N0") + " ~ " +
                                               (weaponMaxDamage * Mathf.Pow(1.1f, weaponUpgradeCount)).ToString("N0");
            weaponInfo.weaponFireRateText.text = weaponFireRate.ToString("N2") + "√  / πﬂ";

            if (weaponUpgradeCount == 0)
            {
                weaponInfo.weaponNameText.text = weaponName;
            }
            else
            {
                weaponInfo.weaponNameText.text = "+" + weaponUpgradeCount + " " + weaponName;
            }

            weaponInfo.weaponImg.sprite = weaponImg;
            weaponInfo.effect[0].text = effectString[0];
            weaponInfo.effect[1].text = effectString[1];
            weaponInfo.effect[2].text = effectString[2];

            Debug.Log("Show Info Box");
        }

    }

    public void HideInfoBox(PlayerCharacter playerCharacter)
    {
        if (weaponInfoBox != null)
        {
            weaponInfoBox.gameObject.SetActive(false);
        }
        Debug.Log("Hide Info Box");
    }

    public void InitWeaponData(WeaponData weaponData)
    {
        weaponName = weaponData.weaponName;
        weaponType = (WeaponType)weaponData.weaponType;
        weaponMinDamage = Random.Range(weaponData.minMinDamage,weaponData.minMaxDamage);
        weaponMaxDamage = Random.Range(weaponData.maxMinDamage, weaponData.maxMaxDamage);
        weaponFireRate = Random.Range(weaponData.minfireRate,weaponData.maxfireRate);
        weaponImg = weaponData.weaponImg;
    }
    public void InitWeaponEffect()
    {
        List<JsonManager.Effect> selectedEffects = new List<JsonManager.Effect>();

        for (int i = 0; i < 3; i++)
        {
            JsonManager.EffectTable randomTable = effectData.effectTables[Random.Range(0, effectData.effectTables.Count)];
            JsonManager.Effect effect = randomTable.effects[Random.Range(0, randomTable.effects.Count)];

            if (Random.value <= effect.chance)
            {
                selectedEffects.Add(effect);
                effectType[i] = effect.effectType;

                switch (effect.effectType)
                {
                    case EffectType.DamageIncrease:
                        effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                        effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                        break;

                    case EffectType.FireRateIncrease:
                        effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                        effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                        break;

                    case EffectType.CriticalProbabilityIncrease:
                        effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                        effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                        break;

                    case EffectType.CriticalDamageIncrease:
                        effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                        effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                        break;

                    case EffectType.DamageAndRecoilIncrease:
                        effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                        effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"))
                                                         .Replace("recoilValue", (effectVal[i] / 2).ToString("N0"));
                        break;

                    case EffectType.FireRateIncreaseAndDamageDecrease:
                        effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                        effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"))
                                                    .Replace("damageValue", (effectVal[i] / 4).ToString("N0"));
                        break;

                }
            }
        }
    }

    public void InitWeaponUpgrade()
    {
        weaponUpgradeCount = Random.Range(0, 4);
    }

    private void ApplyWeaponStats(PlayerCharacter playerCharacter)
    {
        playerCharacter.currentWeapon.weaponName = weaponName;
        playerCharacter.currentWeapon.weaponType = weaponType;
        playerCharacter.currentWeapon.weaponUpgradeCount = weaponUpgradeCount;
        playerCharacter.currentWeapon.baseMinDamage = weaponMinDamage;
        playerCharacter.currentWeapon.baseMaxDamage = weaponMaxDamage;
        playerCharacter.currentWeapon.baseFireRate = weaponFireRate;
        playerCharacter.currentWeapon.baseCriticalProbability = weaponCriticalProbability;
        playerCharacter.currentWeapon.baseCriticalDamage = weaponCriticalDamage;
        playerCharacter.currentWeapon.effectString = effectString;
        playerCharacter.currentWeapon.effectType = effectType;
        playerCharacter.currentWeapon.effectVal = effectVal;
        playerCharacter.currentWeapon.baseWeaponRecoilAmount = weaponRecoilAmount;
    }
}
