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

        if(index == 1)
        {
            PlayerCharacter.Instance.DecreaseCoolDown.Add(30);
        }
        if (index == 2)
        {
            PlayerCharacter.Instance.DecreaseDamage.Add(-50);
            PlayerCharacter.Instance.IncreaseDamage.Add(50);
        }
        if (index == 4)
        {
            PlayerCharacter.Instance.DecreaseDamage.Add(30);
        }
    }

    public List<bool> CheckOoparts()
    {
        List<bool> falseValues = new List<bool>();
        foreach(var ooparts in oopartsActive)
        {
            if (ooparts == false)
            {
                falseValues.Add(ooparts);
            }
        }

        return falseValues;

    }
}
