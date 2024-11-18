using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;

public class TitleManager : MonoBehaviour
{

    public void StartButtonClick()
    {
        Main.Instance.LoadScene(SceneType.GameScene);
    }

    public void ExitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
