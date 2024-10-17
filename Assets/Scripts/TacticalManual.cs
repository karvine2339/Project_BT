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

    private Image icon;
    public TextMeshProUGUI effectName;

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
        //level = tacticalManualData.level[0];
        //level = tacticalManualData.level[TacticalLevel.DroneDamageLevel];
        effectName.text = tacticalManualData.EffectName;
    }

    public void OnClick()
    {
        TacticalManager.Instance.tacticalManualCanvasObject.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        CursorSystem.Instance.SetCursorState(false);
        BTInputSystem.Instance.isTab = false;
    }

    public void OnDisable()
    {
        Destroy(gameObject);
    }
}
