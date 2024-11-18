using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : SceneBase
{
    public override float SceneLoadingProgress { get; protected set; }

    public override IEnumerator SceneStart()
    {
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("TitleScene");

        while(!async.isDone)
        {
            yield return null;
            SceneLoadingProgress = async.progress;
        }

    }              
    public override IEnumerator SceneEnd()
    {

        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("TitleScene");
        while(!async.isDone)
        {
            yield return null;
            SceneLoadingProgress = async.progress;
        }
    }

}
