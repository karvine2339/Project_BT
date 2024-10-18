using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "New TacticalManul Data", menuName = "TactialManual/TacticalManual Data")]
public class TacticalManualData : ScriptableObject
{
    public string EffectName;
    public int level;
    public float[] value;
    public int index;

    public Sprite tacticalManualIcon;
    public Sprite tacticalManualTypeIcon;

}
