using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TacticalManualEffect
{
    BulletDamageIncrease,
    FireRateIncrease,
    DroneDamageIncrease,
    DroneRocketCountIncrease,
    DroneDelayDecrease
}
public class TacticalManual : MonoBehaviour
{
    private TacticalManualData tacticalManualData;
    public int level;
    public TacticalManualEffect effectIndex;

    private Image icon;
    public TextMeshProUGUI effectName;
    public TextMeshProUGUI effectLevel;
    public TextMeshProUGUI effectVal;

    public static event System.Action OnRefreshTacticalIcons;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
    }

    private void Start()
    {
        //PlayerStat.OnAdditionalBulletDamageChanged += HandleAdditionalBulletDamageChange;
    }

    public TacticalManualData GetTacticalManualData(TacticalManualData data)
    {
        tacticalManualData = data;

        return tacticalManualData;
    }

    public void InitTacticalData()
    {
        icon.sprite = tacticalManualData.tacticalManualIcon;
        level = tacticalManualData.level;
        effectIndex = (TacticalManualEffect)tacticalManualData.index;
        effectName.text = tacticalManualData.EffectName;
        effectLevel.text = "Level " + (level + 1).ToString();
        effectVal.text = tacticalManualData.value[0].ToString() + " / "
            + (tacticalManualData.value[0] + tacticalManualData.value[1]).ToString() + " / "
             + (tacticalManualData.value[0] + tacticalManualData.value[1] + tacticalManualData.value[2]).ToString();
    }

    public void OnClick()
    {
        TacticalManager.Instance.tacticalManualCanvasObject.SetActive(false);
        BTInputSystem.Instance.isTac = false;
        Time.timeScale = 1.0f;
        CursorSystem.Instance.SetCursorState(false);
        tacticalManualData.level++;

        float value = tacticalManualData.value[level];

        Dictionary<TacticalManualEffect, System.Action> effectActions = new Dictionary<TacticalManualEffect, System.Action>()
    {
        { TacticalManualEffect.BulletDamageIncrease, () => PlayerStat.Instance.AdditionalBulletDamage += value / 100 },
        { TacticalManualEffect.FireRateIncrease, () => PlayerStat.Instance.FireRate /= 1 + value / 100 },
        { TacticalManualEffect.DroneDamageIncrease, () => PlayerStat.Instance.DroneDamage += value / 100 },
        { TacticalManualEffect.DroneRocketCountIncrease, () => DronCtrl.Instance.maxRocketCount += (int)value },
        { TacticalManualEffect.DroneDelayDecrease, () => DronCtrl.Instance.maxRocketDelay -= value }
    };

        if (effectActions.ContainsKey(effectIndex))
        {
            effectActions[effectIndex].Invoke();
        }

        BTInputSystem.Instance.isTab = false;

        if(BTInputSystem.Instance.isShop)
        {
            ShopManager.Instance.shopObject.SetActive(true);
            Time.timeScale = 0.0f;
            CursorSystem.Instance.SetCursorState(true);
            ShopManager.Instance.UpdateCredit();
        }

        OnRefreshTacticalIcons?.Invoke();
    }

    public void OnDisable()
    {
        Destroy(gameObject);
    }

    public void HandleAdditionalBulletDamageChange(float value)
    {
        PlayerStat.Instance.AdditionalBulletDamage += value;
    }
}
