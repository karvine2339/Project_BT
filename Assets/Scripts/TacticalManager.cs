using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalLevel
{
    public static int[] tacticalLevel;
}
public class TacticalManager : MonoBehaviour
{
    public static TacticalManager Instance;

    public GameObject tacticalManualGroup;
    public GameObject tacticalManualCanvasObject;

    Dictionary<int,int> tacticalManualDic = new Dictionary<int,int>();


    private void Awake()
    {
        Instance = this;
    }

    public TacticalManualData[] tacticalManualDatas;

    public GameObject TacticalManual;

    public void SetTacticalManual()
    {
        List<TacticalManualData> datas = new List<TacticalManualData>();

        for(int i = 0; i < tacticalManualDatas.Length; i++)
        {
            datas.Add(tacticalManualDatas[i]);
        }
        for(int i = 0; i < 3; i++)
        {
            GameObject tacticalObject = Instantiate(TacticalManual);
            int RandVal = Random.Range(0, datas.Count);
            tacticalObject.gameObject.GetComponent<TacticalManual>().GetTacticalManualData(datas[RandVal]);
            tacticalObject.gameObject.GetComponent<TacticalManual>().InitTacticalData();
            datas.RemoveAt(RandVal);
            tacticalObject.transform.SetParent(tacticalManualGroup.transform, false);
        }
    }
}
