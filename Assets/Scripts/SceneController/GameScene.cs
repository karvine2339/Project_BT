using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : SceneBase
{
    public override float SceneLoadingProgress { get; protected set; }

    public override IEnumerator SceneStart()
    {
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("SampleScene");

        while (!async.isDone)
        {
            yield return null;
            SceneLoadingProgress = async.progress;
        }

        // Ingame에서 사용할 UI를 띄워준다.
        // UIManager.Show<IngameUI>(UIList.IngameUI);
        // UIManager.Show<IndicatorUI>(UIList.IndicatorUI);
    }
    public override IEnumerator SceneEnd()
    {
        // Ingame에서 사용했던 UI를 숨겨준다.
        // UIManager.Hide<IngameUI>(UIList.IngameUI);

        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("SampleScene");
        while (!async.isDone)
        {
            yield return null;
            SceneLoadingProgress = async.progress;
        }
    }
}
