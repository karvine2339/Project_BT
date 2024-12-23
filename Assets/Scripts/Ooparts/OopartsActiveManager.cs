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

        if (index == 1)
        {
            PlayerCharacter.Instance.DecreaseCoolDown.Add(30);
        }
        if (index == 2)
        {
            PlayerCharacter.Instance.DecreaseDamage.Add(-50);
            PlayerCharacter.Instance.IncreaseDamage.Add(-50);
        }
        if (index == 4)
        {
            PlayerCharacter.Instance.DecreaseDamage.Add(30);
        }
        if (index == 6)
        {
            PlayerCharacter.Instance.IncreaseFireRate.Add(50);
            PlayerCharacter.Instance.IncreaseRecoil.Add(50);
        }
        if (index == 7)
        {
            PlayerCharacter.Instance.DecreaseShopPrice.Add(30);
        }
        if (index == 8)
        {
            PlayerCharacter.Instance.IncreaseGainCoin.Add(30);
        }
        if(index == 9)
        {
            PlayerCharacter.Instance.IncreaseShield.Add(50);
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
