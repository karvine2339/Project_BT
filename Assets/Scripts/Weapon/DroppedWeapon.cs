using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Random = UnityEngine.Random;

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

    public GameObject weaponObject;
    Dictionary<string, string> weaponPrefabPaths = new Dictionary<string, string>()
    {
        {"Weapon_Shiroko_Dropped(Clone)" , "Weapon_Shiroko" },
        {"Weapon_Serika_Dropped(Clone)", "Weapon_Serika" }
    };

    [HideInInspector] public float weaponMinDamage;
    [HideInInspector] public float  weaponMaxDamage;
    [HideInInspector] public float  weaponFireRate;
    [HideInInspector] public float  weaponCriticalProbability = 10.0f;
    [HideInInspector] public float  weaponCriticalDamage = 1.5f;
    [HideInInspector] public float[] effectVal = new float[3];
    [HideInInspector] public int[] effectType = new int[3];
    [HideInInspector] public string[] effectString = new string[3];
    [HideInInspector] public int weaponType;
    [HideInInspector] public float weaponRecoilAmount;

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
            InitWeaponEffect();
        }
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
                playerCharacter.ChangedPrimaryWeapon();
                HUDManager.Instance.firstWeapon.SetActive(true);
                ApplyWeaponStats(playerCharacter);
                newWeapon.InitWeaponStat();
                newWeapon.InitFirstWeaponUI();
            }
            else if (playerCharacter.weapons[1] == null)
            {
                playerCharacter.weapons[1] = newWeapon;
                playerCharacter.currentWeapon = playerCharacter.weapons[1];
                playerCharacter.ChangedSecondaryWeapon();
                HUDManager.Instance.secondWeapon.SetActive(true);
                ApplyWeaponStats(playerCharacter);
                newWeapon.InitWeaponStat();
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
                playerCharacter.ChangedPrimaryWeapon();
                ApplyWeaponStats(playerCharacter);
                newWeapon.InitWeaponStat();
                newWeapon.InitFirstWeaponUI();
     
            }
            else if (playerCharacter.currentWeapon == playerCharacter.weapons[1])
            {
                WeaponDropManager.Instance.ThrowEquippedWeapon(this.transform.position, 1);
                Destroy(playerCharacter.weapons[1].gameObject);
                playerCharacter.weapons[1] = newWeapon;
                playerCharacter.currentWeapon = playerCharacter.weapons[1];
                playerCharacter.ChangedSecondaryWeapon();
                ApplyWeaponStats(playerCharacter);
                newWeapon.InitWeaponStat();
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
            weaponInfo.weaponDamageText.text = weaponMinDamage.ToString("N0") + " ~ " + weaponMaxDamage.ToString("N0");
            weaponInfo.weaponFireRateText.text = weaponFireRate.ToString("N2") + "√  / πﬂ";
            weaponInfo.weaponNameText.text = weaponName;
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
        weaponType = weaponData.weaponType;
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
                Debug.Log("Effect Applied: " + effect.effectName);

                if (effect.effectType == 0)
                {
                    effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                    weaponMinDamage *= 1 + (effectVal[i] / 100);
                    weaponMaxDamage *= 1 + (effectVal[i] / 100);
                    effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                }

                else if (effect.effectType == 1)
                {
                    effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                    weaponFireRate /= 1 + effectVal[i] / 100;
                    effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                }

                else if (effect.effectType == 2)
                {
                    effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                    weaponCriticalProbability += effectVal[i];
                    effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                }
                else if (effect.effectType == 3)
                {
                    effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                    weaponCriticalDamage += effectVal[i] / 100;
                    effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                }
                else if (effect.effectType == 4)
                {
                    effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                    weaponMinDamage *= 1 + (effectVal[i] / 100);
                    weaponMaxDamage *= 1 + (effectVal[i] / 100);
                    weaponRecoilAmount = 1 + (effectVal[i] / 2) / 100;
                    
                    effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"))
                                                     .Replace("recoilValue", (effectVal[i]/2).ToString("N0"));
                }
                else if (effect.effectType == 5)
                {
                    effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                    weaponMinDamage *= 1 - (effectVal[i] / 100) / 4;
                    weaponMaxDamage *= 1 - (effectVal[i] / 100) / 4;
                    weaponFireRate /= 1 + effectVal[i] / 100;

                    effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"))
                                                .Replace("damageValue", (effectVal[i] / 4).ToString("N0"));
                }
                else if(effect.effectType == 6)
                {
                    effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                    
                }

            }
        }
    }

    private void ApplyWeaponStats(PlayerCharacter playerCharacter)
    {
        playerCharacter.currentWeapon.weaponName = weaponName;
        playerCharacter.currentWeapon.weaponType = weaponType;
        playerCharacter.currentWeapon.minDamage = weaponMinDamage;
        playerCharacter.currentWeapon.maxDamage = weaponMaxDamage;
        playerCharacter.currentWeapon.fireRate = weaponFireRate;
        playerCharacter.currentWeapon.criticalProbability = weaponCriticalProbability;
        playerCharacter.currentWeapon.criticalDamage = weaponCriticalDamage;
        playerCharacter.currentWeapon.effectString = effectString;
        playerCharacter.currentWeapon.effectType = effectType;
        playerCharacter.currentWeapon.effectVal = effectVal;
        playerCharacter.currentWeapon.weaponRecoilAmount = weaponRecoilAmount;
    }
}
