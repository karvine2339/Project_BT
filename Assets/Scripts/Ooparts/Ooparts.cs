using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ooparts : MonoBehaviour
{
    public void OnClick()
    {
        OopartsManager.Instance.oopartsCanvasObject.SetActive(false);
        BTInputSystem.Instance.isOp = false;
        Time.timeScale = 1.0f;
        CursorSystem.Instance.SetCursorState(false);

        BTInputSystem.Instance.isOp = false;
    }

}
