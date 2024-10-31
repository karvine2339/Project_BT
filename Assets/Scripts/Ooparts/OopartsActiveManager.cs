using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OopartsActiveManager : MonoBehaviour
{
    public static OopartsActiveManager Instance;

    public bool[] oopartsActive = new bool[(int)OopartsType.Count];

    #region Awake
    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    #endregion

    public void ActiveOoparts(int index)
    {
        oopartsActive[index] = true;
    }
}
