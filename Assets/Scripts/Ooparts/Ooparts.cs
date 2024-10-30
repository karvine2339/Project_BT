using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ooparts : MonoBehaviour
{

    public TextMeshProUGUI oopartsName;
    public TextMeshProUGUI oopartsEffectString;
    public Image icon;
    public Image BackIcon;

    private int oopartsIndex;

    private OopartsData oopartsData;
    public OopartsData GetOopartsData(OopartsData data)
    {
        oopartsData = data;

        InitOopartsData();

        return oopartsData;
    }

    public void InitOopartsData()
    {
        oopartsName.text = oopartsData.oopartsName;
        oopartsEffectString.text = oopartsData.effectString;
        icon.sprite = oopartsData.oopartsIcon;
        BackIcon.sprite = oopartsData.oopartsBackIcon;

    }
    public void OnClick()
    {
        OopartsManager.Instance.oopartsCanvasObject.SetActive(false);
        BTInputSystem.Instance.isOp = false;
        Time.timeScale = 1.0f;
        CursorSystem.Instance.SetCursorState(false);
        BTInputSystem.Instance.isOp = false;
    }

}
