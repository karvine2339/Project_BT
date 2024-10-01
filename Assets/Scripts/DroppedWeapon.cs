using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Random = UnityEngine.Random;

public class DroppedWeapon : MonoBehaviour, IInteractable
{
    private string jsonFilePath = "Assets/Data/WeaponEffectData.json";

    [Serializable]
    public class Effect
    {
        public string effectName;  
        public float minValue;     
        public float maxValue;       
        public float chance;      
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

    [SerializeField] private float weaponMinDamage;
    [SerializeField] private float weaponMaxDamage;
    [SerializeField] private float weaponFireRate;
    [SerializeField] private float weaponCriticalProbability;
    [SerializeField] private float weaponCriticalDamage;

    private void Start()
    {
        InitWeaponData(weaponData);

        string json = System.IO.File.ReadAllText(jsonFilePath);



        effectData = JsonConvert.DeserializeObject<Root>(json);

        InitWeaponEffect();


        //Root parsedData = JsonConvert.DeserializeObject<Root>(json);

        //foreach (var table in parsedData.effectTables)
        //{
        //    Debug.Log("���̺� �̸�: " + table.name);
        //    foreach (var effect in table.effects)
        //    {
        //        Debug.Log("ȿ�� �̸�: " + effect.effectName);
        //        Debug.Log("�ּҰ�: " + effect.minValue);
        //        Debug.Log("�ִ밪: " + effect.maxValue);
        //        Debug.Log("Ȯ��: " + effect.chance);
        //    }
        //}
    }


    public void Interact(ChrBase playerCharacter)
    {

        PlayerStat.Instance.BulletMinDamage = weaponMinDamage;
        PlayerStat.Instance.BulletMaxDamage = weaponMaxDamage;
        PlayerStat.Instance.FireRate = weaponFireRate;
        PlayerStat.Instance.CriticalProbability += weaponCriticalProbability;

        Interaction_UI.Instance.RemoveInteractionData(this);

        HUDManager.Instance.weaponImage.sprite = weaponImg;

        Destroy(gameObject);

    }

    public void ShowInfo(ChrBase playerCharacter)
    {
        Debug.Log(weaponName);
        Debug.Log(weaponMaxDamage);
        Debug.Log(weaponFireRate);
        Debug.Log(weaponCriticalProbability);
        Debug.Log(weaponCriticalDamage);
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
            Effect randomEffect = randomTable.effects[Random.Range(0, randomTable.effects.Count)];

            if (Random.value <= randomEffect.chance)
            {
                selectedEffects.Add(randomEffect);
                Debug.Log("Effect Applied: " + randomEffect.effectName);
            }
        }

        foreach (var effect in selectedEffects)
        {
            if (effect.effectName == "������ ����")
            {
                float randDmg = Random.Range(effect.minValue, effect.maxValue);
                weaponMinDamage *=  1 + (randDmg / 100);
                weaponMaxDamage *= 1 + (randDmg / 100);
            }

            else if (effect.effectName == "ũ��Ƽ�� Ȯ�� ����")
            {
                weaponCriticalProbability += Random.Range(effect.minValue, effect.maxValue);
            }

            else if (effect.effectName == "����ӵ� ����")
            {
                weaponFireRate /= 1 + Random.Range(effect.minValue / 100, effect.maxValue / 100);
            }
            else if (effect.effectName == "ũ��Ƽ�� ������ ����")
            {
                weaponCriticalDamage += Random.Range(effect.minValue, effect.maxValue);
            }

        }
    }
}
