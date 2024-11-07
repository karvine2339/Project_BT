using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticalIcon : MonoBehaviour
{
    public Image tacticalIconImage;
    public GameObject[] tacticalLevelSymbol;
    public int Index;
    [HideInInspector] public string[] tacticalString = new string[2];
    [HideInInspector] public int tacticalLevel;
    public float[] tacticalValue;

    private TacticalManualData tacticalManualData;

    public TacticalManualData GetTacticalManualData(TacticalManualData data)
    {
        tacticalManualData = data;

        InitTacticalManualData();

        return tacticalManualData;
    }

    public void InitTacticalManualData()
    {
        tacticalString[0] = tacticalManualData.EffectName;
        tacticalString[1] = tacticalManualData.EffectName;
        tacticalIconImage.sprite = tacticalManualData.tacticalManualIcon;
        tacticalLevel = tacticalManualData.level;
        tacticalValue = tacticalManualData.value;

    }
    private void Start()
    {
        RefreshTacticalIconData();
        TacticalManual.OnRefreshTacticalIcons += RefreshTacticalIconData;
    }

    private void OnDestroy()
    {
        TacticalManual.OnRefreshTacticalIcons -= RefreshTacticalIconData;
    }


    public void RefreshTacticalIconData()
    {
        Debug.Log("Refresh");

        GetTacticalManualData(TacticalManager.Instance.tacticalManualDatas[Index]);
        switch (tacticalLevel)
        {
            case 0:
                break;
            case 1:
                tacticalLevelSymbol[0].SetActive(true);
                break;
            case 2:
                tacticalLevelSymbol[1].SetActive(true);
                break;
            case 3:
                tacticalLevelSymbol[2].SetActive(true);
                break;
        }
    }
}
