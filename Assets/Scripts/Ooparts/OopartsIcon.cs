using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OopartsIcon : MonoBehaviour
{
    [HideInInspector] public Image[] oopartsimages;
    [HideInInspector] public string[] oopartsString = new string[2];
    [HideInInspector] public int oopartsIndex;

    public bool isActive = false;

    private OopartsData oopartsData;
    public OopartsData GetOopartsData(OopartsData data)
    {
        oopartsData = data;

        InitOopartsData();

        return oopartsData;
    }

    public void InitOopartsData()
    {
        oopartsString[0] = oopartsData.oopartsName;
        oopartsString[1] = oopartsData.effectString;
        oopartsIndex = oopartsData.oopartsIndex;
        oopartsimages[0].sprite = oopartsData.oopartsBackIcon;
        oopartsimages[1].sprite = oopartsData.oopartsIcon;

    }



    private void Awake()
    {
        oopartsimages = GetComponentsInChildren<Image>(false);
        oopartsString = new string[2];
        oopartsIndex = -1;
    }

    private void Start()
    {

    }
}
