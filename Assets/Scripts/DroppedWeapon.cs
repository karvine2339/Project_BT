using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Random = UnityEngine.Random;
using static DroppedWeapon;

public class DroppedWeapon : MonoBehaviour, IInteractable
{
    private string jsonFilePath = "Assets/Data/WeaponEffectData.json";

    [Serializable]
    public class Effect
    {
        public string effectName;
        public int effectType;
        public float damageMinValue;
        public float damageMaxValue;
        public float minValue;     
        public float maxValue;       
        public float chance;
        public string InfoString;
    }

    [Serializable]
    public class EffectTable
    {
        public string name;        
        public List<Effect> effects; 
    }

    [Serializable]
    public class Root
    {
        public List<EffectTable> effectTables;  
    }

    private Root effectData;
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

    [SerializeField] private float weaponMinDamage;
    [SerializeField] private float weaponMaxDamage;
    [SerializeField] private float weaponFireRate;
    [SerializeField] private float weaponCriticalProbability = 20.0f;
    [SerializeField] private float weaponCriticalDamage = 1.5f;
    [SerializeField] private float[] effectVal = new float[3];
    [SerializeField] private int[] effectType = new int[3];
    private string[] weaponEffect = new string[3];
    private void Start()
    {
        weaponInfoBox = Interaction_UI.Instance.weaponInfoBox;

        InitWeaponData(weaponData);

        string json = System.IO.File.ReadAllText(jsonFilePath);
        effectData = JsonConvert.DeserializeObject<Root>(json);

        InitWeaponEffect();

        weaponInfo = weaponInfoBox.GetComponent<WeaponInfoBox>();

        //Root parsedData = JsonConvert.DeserializeObject<Root>(json);

        //foreach (var table in parsedData.effectTables)
        //{
        //    Debug.Log("테이블 이름: " + table.name);
        //    foreach (var effect in table.effects)
        //    {
        //        Debug.Log("효과 이름: " + effect.effectName);
        //        Debug.Log("최소값: " + effect.minValue);
        //        Debug.Log("최대값: " + effect.maxValue);
        //        Debug.Log("확률: " + effect.chance);
        //    }
        //}
    }


    public void Interact(PlayerCharacter playerCharacter)
    {
        string prefabPaths = weaponPrefabPaths[this.gameObject.name];
        weaponObject = Resources.Load(prefabPaths) as GameObject;

        GameObject newWeaponInstance = Instantiate(weaponObject, playerCharacter.weaponRoot);
        Weapon newWeapon = newWeaponInstance.GetComponent<Weapon>();

        if (playerCharacter.weapons[0] == null || playerCharacter.weapons[1] == null)
        {
            if (playerCharacter.weapons[0] == null)
            {
                playerCharacter.weapons[0] = newWeapon;
                playerCharacter.currentWeapon = playerCharacter.weapons[0];
                playerCharacter.ChangedPrimaryWeapon();
                ApplyWeaponStats(playerCharacter);
                newWeapon.InitWeaponStat();
                newWeapon.InitFirstWeaponUI();
            }
            else if (playerCharacter.weapons[1] == null)
            {
                playerCharacter.weapons[1] = newWeapon;
                playerCharacter.currentWeapon = playerCharacter.weapons[1];
                playerCharacter.ChangedSecondaryWeapon();
                ApplyWeaponStats(playerCharacter);
                newWeapon.InitWeaponStat();
                newWeapon.InitSecondWeaponUI();
            }

  
        }

        else
        {
            if (playerCharacter.currentWeapon == playerCharacter.weapons[0])
            {
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
                Destroy(playerCharacter.weapons[1].gameObject);
                playerCharacter.weapons[1] = newWeapon;
                playerCharacter.currentWeapon = playerCharacter.weapons[1];
                playerCharacter.ChangedSecondaryWeapon();
                ApplyWeaponStats(playerCharacter);
                newWeapon.InitWeaponStat();
                newWeapon.InitSecondWeaponUI();
            }

        }
        Interaction_UI.Instance.RemoveInteractionData(this);

        HUDManager.Instance.curWeaponImage.sprite = weaponImg;

        Destroy(gameObject);

    }

    public void ShowInfoBox(PlayerCharacter playerCharacter)
    {
        if(weaponInfoBox != null)
        {
            weaponInfoBox.gameObject.SetActive(true);
            weaponInfo.weaponDamageText.text = weaponMinDamage.ToString("N0") + " ~ " + weaponMaxDamage.ToString("N0");
            weaponInfo.weaponFireRateText.text = weaponFireRate.ToString("N2") + "초 / 발";
            weaponInfo.weaponNameText.text = weaponName;
            weaponInfo.weaponImg.sprite = weaponImg;
            weaponInfo.effect[0].text = weaponEffect[0];
            weaponInfo.effect[1].text = weaponEffect[1];
            weaponInfo.effect[2].text = weaponEffect[2];

        }
        Debug.Log("Show Info Box");
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
        weaponMinDamage = weaponData.minDamage;
        weaponMaxDamage = weaponData.maxDamage;
        weaponFireRate = Random.Range(weaponData.minfireRate,weaponData.maxfireRate);
        weaponImg = weaponData.weaponImg;
    }

    private void InitWeaponEffect()
    {
        List<Effect> selectedEffects = new List<Effect>();

        for (int i = 0; i < 3; i++)
        {
            EffectTable randomTable = effectData.effectTables[Random.Range(0, effectData.effectTables.Count)];
            Effect effect = randomTable.effects[Random.Range(0, randomTable.effects.Count)];

            if (Random.value <= effect.chance)
            {
                selectedEffects.Add(effect);
                Debug.Log("Effect Applied: " + effect.effectName);

                if (effect.effectType == 1)
                {
                    effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                    weaponMinDamage *= 1 + (effectVal[i] / 100);
                    weaponMaxDamage *= 1 + (effectVal[i] / 100);
                    weaponEffect[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                }

                else if (effect.effectType == 2)
                {
                    effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                    weaponFireRate /= 1 + effectVal[i] / 100;
                    weaponEffect[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                }

                else if (effect.effectType == 3)
                {
                    effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                    weaponCriticalProbability += effectVal[i];
                    weaponEffect[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                }
                else if (effect.effectType == 4)
                {
                    effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                    weaponCriticalDamage += effectVal[i] / 100;
                    weaponEffect[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                }
                else if (effect.effectType == 5)
                {
                    effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                    weaponMinDamage *= 1 + (effectVal[i] / 100);
                    weaponMaxDamage *= 1 + (effectVal[i] / 100);
                    weaponEffect[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"))
                                                     .Replace("recoilValue", effectVal[i].ToString("N0"));
                }

            }
        }
    }

    private void ApplyWeaponStats(PlayerCharacter playerCharacter)
    {
        playerCharacter.currentWeapon.weaponName = weaponName;
        playerCharacter.currentWeapon.minDamage = weaponMinDamage;
        playerCharacter.currentWeapon.maxDamage = weaponMaxDamage;
        playerCharacter.currentWeapon.fireRate = weaponFireRate;
        playerCharacter.currentWeapon.criticalProbability = weaponCriticalProbability;
        playerCharacter.currentWeapon.criticalDamage = weaponCriticalDamage;
        playerCharacter.currentWeapon.effectString = weaponEffect;
    }
}
