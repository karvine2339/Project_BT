using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum OopartsType
{
    Boots = 0,
    Clock = 1,
    CursedDoll = 2,
    Dice = 3,
    Medal = 4,
    Card = 5,
    Glasses = 6,
    Volt = 7,
    Jar = 8,
    Ether = 9,
    Charm_Oopart = 10,
    Charm_Weapon = 11,
    Count
}
public class OopartsManager : UIBase
{
    public static OopartsManager Instance { get; private set; }

    public GameObject oopartsCanvasObject;

    public GameObject oopartsSelectGroup;

    public OopartsData[] oopartsDatas;

    public GameObject ooparts;

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


    public void SetOoparts()
    {
        List<OopartsData> datas = new List<OopartsData>(oopartsDatas);

        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0, datas.Count);

            if (!OopartsActiveManager.Instance.oopartsActive[datas[rand].oopartsIndex])
            {
                GameObject oopartsObject = Instantiate(ooparts);
                oopartsObject.gameObject.GetComponent<Ooparts>().GetOopartsData(datas[rand]);
                oopartsObject.transform.SetParent(oopartsSelectGroup.transform, false);

                datas.Remove(datas[rand]);
            }
            else
            {
                datas.Remove(datas[rand]);
                i--;
            }
        }
    }
}
