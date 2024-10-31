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
        List<OopartsData> RemainDatas = datas.ToList();

        int count = 0;

        while (count < 3)
        {
            int rand = Random.Range(0, datas.Count);

            if (RemainDatas.Count == 0)
                break;

            if (RemainDatas.Contains(datas[rand]))
            {

                if (!OopartsActiveManager.Instance.oopartsActive[datas[rand].oopartsIndex])
                {
                    GameObject oopartsObject = Instantiate(ooparts);
                    oopartsObject.gameObject.GetComponent<Ooparts>().GetOopartsData(datas[rand]);
                    oopartsObject.transform.SetParent(oopartsSelectGroup.transform, false);

                    RemainDatas.Remove(datas[rand]);
                    count++;
                }
            }
            else
            {
                RemainDatas.Remove(datas[rand]);
                continue;
            }
        }
    }
}
