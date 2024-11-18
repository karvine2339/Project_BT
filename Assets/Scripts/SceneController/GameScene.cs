using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : SceneBase
{
    public override float SceneLoadingProgress { get; protected set; }

    public override IEnumerator SceneStart()
    {
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameScene");

        while (!async.isDone)
        {
            yield return null;
            SceneLoadingProgress = async.progress;
        }
    }
    public override IEnumerator SceneEnd()
    {

        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("GameScene");
        while (!async.isDone)
        {
            yield return null;
            SceneLoadingProgress = async.progress;
        }
    }
}
