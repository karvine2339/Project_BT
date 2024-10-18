using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TacticalManual : MonoBehaviour
{
    private TacticalManualData tacticalManualData;
    public int level;
    public int effectIndex;

    private Image icon;
    public TextMeshProUGUI effectName;
    public TextMeshProUGUI effectLevel;
    public TextMeshProUGUI effectVal;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[0];
    }

    public TacticalManualData GetTacticalManualData(TacticalManualData data)
    {
        tacticalManualData = data;

        return tacticalManualData;
    }

    public void InitTacticalData()
    {
        //icon.sprite = tacticalManualData.tacticalManualIcon;
        //level = tacticalManualData.level[TacticalLevel.DroneDamageLevel];
        level = tacticalManualData.level;
        effectIndex = tacticalManualData.index;
        effectName.text = tacticalManualData.EffectName;
        effectLevel.text = "Level " + (level + 1).ToString();
        effectVal.text = tacticalManualData.value[0].ToString() + " / " + tacticalManualData.value[1].ToString() + " / "
             + tacticalManualData.value[2].ToString();
    }

    public void OnClick()
    {
        TacticalManager.Instance.tacticalManualCanvasObject.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        CursorSystem.Instance.SetCursorState(false);
        tacticalManualData.level++;
        if(effectIndex == 1)
        {
            if(level == 0)
            {
                PlayerStat.Instance.AdditionalBulletDamage += tacticalManualData.value[0] / 100;
            }
            else if(level == 1)
            {
                PlayerStat.Instance.AdditionalBulletDamage += tacticalManualData.value[1] / 100;
            }
            else if(level == 2)
            {
                PlayerStat.Instance.AdditionalBulletDamage += tacticalManualData.value[2] / 100;
            }
        }
        else if(effectIndex == 2)
        {
            if (level == 0)
            { 
                PlayerStat.Instance.FireRate /= 1 + tacticalManualData.value[0] / 100;
            }
            if(level == 1)
            {
                PlayerStat.Instance.FireRate /= 1 + tacticalManualData.value[1] / 100;
            }
            if(level == 2)
            {
                PlayerStat.Instance.FireRate /= 1 + tacticalManualData.value[2] / 100;
            }
        }
        Debug.Log(PlayerStat.Instance.AdditionalBulletDamage);
        
        BTInputSystem.Instance.isTab = false;
    }

    public void OnDisable()
    {
        Destroy(gameObject);
    }
}
