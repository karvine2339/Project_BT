using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TacticalManager : UIBase
{
    public static TacticalManager Instance;

    public GameObject tacticalManualGroup;
    public GameObject tacticalManualCanvasObject;

    public int[] tacticalManualLevel;

    List<TacticalManualData> datas = new List<TacticalManualData>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < tacticalManualDatas.Length; i++)
        {
            tacticalManualDatas[i].level = 0;
        }
    }

    public TacticalManualData[] tacticalManualDatas;

    public GameObject TacticalManual;

    public void SetTacticalManual()
    {
        List<TacticalManualData> datas = new List<TacticalManualData>(tacticalManualDatas);

        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0, datas.Count);

            if (datas[rand].level != 3)
            {
                GameObject tacticalObject = Instantiate(TacticalManual);

                tacticalObject.gameObject.GetComponent<TacticalManual>().GetTacticalManualData(datas[rand]);
                tacticalObject.gameObject.GetComponent<TacticalManual>().InitTacticalData();

                datas.RemoveAt(rand);

                tacticalObject.transform.SetParent(tacticalManualGroup.transform, false);

            }
            else
            {
                datas.RemoveAt(rand);
                i--;
            }
        }
    }
}
