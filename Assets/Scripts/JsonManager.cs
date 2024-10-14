using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class JsonManager : MonoBehaviour
{
    public static JsonManager Instance { get; private set; }

    private string jsonFilePath = "Assets/Data/WeaponEffectData.json";
    public Root effectData;  

    [System.Serializable]
    public class Effect
    {
        public string effectName;
        public int effectType;
        public float minValue;
        public float maxValue;
        public float minValue2;
        public float maxValue2;
        public float chance;
        public string InfoString;
    }

    [System.Serializable]
    public class EffectTable
    {
        public string name;
        public List<Effect> effects;
    }

    [System.Serializable]
    public class Root
    {
        public List<EffectTable> effectTables;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject); 
            LoadEffectData(); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void LoadEffectData()
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            effectData = JsonConvert.DeserializeObject<Root>(json);
        }
        else
        {
            Debug.LogError("JSON ������ ã�� �� �����ϴ�: " + jsonFilePath);
        }
    }
}