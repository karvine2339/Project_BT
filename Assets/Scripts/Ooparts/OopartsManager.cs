using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OopartsType
{
    Boots = 0,
    Clock = 1,
    CursedDoll = 2,
    Dice = 3,
    Medal = 4,
    Count = 5
}
public class OopartsManager : UIBase
{
    public bool[] oopartsActive = new bool[(int)OopartsType.Count];
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
            int rand = Random.Range(0, datas.Count - 1);

            GameObject oopartsObject = Instantiate(ooparts);

            if (!oopartsActive[rand])
            {
                oopartsObject.gameObject.GetComponent<Ooparts>().GetOopartsData(datas[rand]);
                oopartsObject.transform.SetParent(oopartsSelectGroup.transform, false);

                datas.RemoveAt(rand);

            }
            else
            {
                i--;
            }

        }
    }
}
