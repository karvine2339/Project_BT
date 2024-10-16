using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalManualCanvas : MonoBehaviour
{
    public GameObject tacticalManual;

    public static TacticalManualCanvas Instance;

    private void Awake()
    {
        Instance = this;
    }
}
