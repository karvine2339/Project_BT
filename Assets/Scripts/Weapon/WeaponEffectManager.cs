using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponData;

public class WeaponEffectManager : MonoBehaviour
{
    public JsonManager jsonManager;

    public JsonManager.Root effectData;

    public static WeaponEffectManager Instance;

    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        effectData = JsonManager.Instance.effectData;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    public void InitWeaponEffect(EffectType[] effectType, float[] effectVal, string[] effectString)
    {
        List<JsonManager.Effect> selectedEffects = new List<JsonManager.Effect>();

        for (int i = 0; i < 3; i++)
        {
            JsonManager.EffectTable randomTable = effectData.effectTables[Random.Range(0, effectData.effectTables.Count)];
            JsonManager.Effect effect = randomTable.effects[Random.Range(0, randomTable.effects.Count)];

            if (Random.value <= effect.chance)
            {
                selectedEffects.Add(effect);
                effectType[i] = effect.effectType;

                switch (effect.effectType)
                {
                    case EffectType.DamageIncrease:
                        effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                        effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                        break;

                    case EffectType.FireRateIncrease:
                        effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                        effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                        break;

                    case EffectType.CriticalProbabilityIncrease:
                        effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                        effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                        break;

                    case EffectType.CriticalDamageIncrease:
                        effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                        effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"));
                        break;

                    case EffectType.DamageAndRecoilIncrease:
                        effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                        effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"))
                                                         .Replace("recoilValue", (effectVal[i] / 2).ToString("N0"));
                        break;

                    case EffectType.FireRateIncreaseAndDamageDecrease:
                        effectVal[i] = Random.Range(effect.minValue, effect.maxValue);
                        effectString[i] = effect.InfoString.Replace("value", effectVal[i].ToString("N0"))
                                                    .Replace("damageValue", (effectVal[i] / 4).ToString("N0"));
                        break;

                }
            }
        }
    }
}
