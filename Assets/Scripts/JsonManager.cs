using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class JsonManager : MonoBehaviour
{
    public static JsonManager Instance { get; private set; }

    private string jsonFilePath = "Assets/Data/WeaponEffectData.json";
    public Root effectData;

    public GameObject[] weaponPrefab;
    public Transform SpawnPos;

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
            Debug.LogError("JSON 파일을 찾을 수 없습니다: " + jsonFilePath);
        }
    }

    public GameObject DropWeapon(Vector3 dropPos)
    {
        GameObject droppedWeapon = Instantiate(weaponPrefab[Random.Range(0, weaponPrefab.Length)], dropPos, Quaternion.Euler(-30, -90, 90));

        DroppedWeapon prefabComponent = droppedWeapon.GetComponent<DroppedWeapon>();

        WeaponData weaponData = prefabComponent.weaponData;

        DroppedWeapon weaponComponent = droppedWeapon.GetComponent<DroppedWeapon>();
        weaponComponent.InitWeaponData(weaponData);

        return droppedWeapon;
    }

    private void Start()
    {
        DropWeapon(SpawnPos.position);
    }
}