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
        oopartsIndex = oopartsData.oopartsIndex;
        icon.sprite = oopartsData.oopartsIcon;
        BackIcon.sprite = oopartsData.oopartsBackIcon;

    }
    public void OnClick()
    {
        OopartsActiveManager.Instance.ActiveOoparts(oopartsIndex);
        OopartsManager.Instance.oopartsCanvasObject.SetActive(false);

        InventoryManager.Instance.AddOoparts(oopartsData.oopartsIndex, oopartsData.oopartsName, oopartsData.effectString
            ,oopartsData.oopartsBackIcon , oopartsData.oopartsIcon); 

        Time.timeScale = 1.0f;
        CursorSystem.Instance.SetCursorState(false);
        BTInputSystem.Instance.isOp = false;

    }
    public void OnDisable()
    {
        Destroy(gameObject);
    }
}
