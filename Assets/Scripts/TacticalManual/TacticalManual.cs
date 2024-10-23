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
        icon = GetComponentsInChildren<Image>()[1];
    }

    public TacticalManualData GetTacticalManualData(TacticalManualData data)
    {
        tacticalManualData = data;

        return tacticalManualData;
    }

    public void InitTacticalData()
    {
        icon.sprite = tacticalManualData.tacticalManualIcon;
        //level = tacticalManualData.level[TacticalLevel.DroneDamageLevel];
        level = tacticalManualData.level;
        effectIndex = tacticalManualData.index;
        effectName.text = tacticalManualData.EffectName;
        effectLevel.text = "Level " + (level + 1).ToString();
        effectVal.text = tacticalManualData.value[0].ToString() + " / "
            + (tacticalManualData.value[0] + tacticalManualData.value[1]).ToString() + " / "
             + (tacticalManualData.value[0] + tacticalManualData.value[1] + tacticalManualData.value[2]).ToString();
    }

    public void OnClick()
    {
        TacticalManager.Instance.tacticalManualCanvasObject.SetActive(false);
        Time.timeScale = 1.0f;
        CursorSystem.Instance.SetCursorState(false);
        tacticalManualData.level++;

        float value = tacticalManualData.value[level];

        Dictionary<int, System.Action> effectActions = new Dictionary<int, System.Action>()
    {
        { 1, () => PlayerStat.Instance.AdditionalBulletDamage += value / 100 },
        { 2, () => PlayerStat.Instance.FireRate /= 1 + value / 100 },
        { 3, () => PlayerStat.Instance.DroneDamage += value / 100 },
        { 4, () => DronCtrl.Instance.maxRocketCount += (int)value },
        { 5, () => DronCtrl.Instance.maxRocketDelay -= value }
    };

        if (effectActions.ContainsKey(effectIndex))
        {
            effectActions[effectIndex].Invoke();
        }

        BTInputSystem.Instance.isTab = false;
    }

    public void OnDisable()
    {
        Destroy(gameObject);
    }
}
