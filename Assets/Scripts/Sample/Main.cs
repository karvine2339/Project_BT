using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public static Main Instance { get; private set; } = null;

    public enum SceneType
    {
        TitleScene,
        GameScene,
    }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        LoadScene(SceneType.TitleScene);   
    }

    private SceneBase currentSceneController = null;
    public void LoadScene(SceneType sceneType)
    {
        GameObject sceneController = new GameObject($"{sceneType}_Controller");
        sceneController.transform.SetParent(transform);
        SceneBase newSceneController = null;
        switch (sceneType)
        {
            case SceneType.TitleScene:
                newSceneController = sceneController.AddComponent<TitleScene>();
                break;

            case SceneType.GameScene:
                newSceneController = sceneController.AddComponent<GameScene>();
                break;

        }

        StartCoroutine(SceneLoadCoroutine(newSceneController));
    }

    private IEnumerator SceneLoadCoroutine<T>(T newSceneController) where T : SceneBase
    {

        LoadingUI loadingUI = UIManager.Show<LoadingUI>(UIList.LoadingUI);
        loadingUI.LoadingProgress = 0;

        AsyncOperation emptySceneLoad = SceneManager.LoadSceneAsync("Empty", LoadSceneMode.Additive);
        while(!emptySceneLoad.isDone)
        {
            yield return null;
        }

        if (currentSceneController != null)
        {
            yield return StartCoroutine(currentSceneController.SceneEnd());
            loadingUI.LoadingProgress = 0.4f;
            yield return new WaitForSeconds(0.5f);
        }

        yield return StartCoroutine(newSceneController.SceneStart());
        loadingUI.LoadingProgress = 0.8f;
        yield return new WaitForSeconds(0.5f);
        currentSceneController = newSceneController;

        loadingUI.LoadingProgress = 1f;

        yield return new WaitForSeconds(0.5f);

        UIManager.Hide<LoadingUI>(UIList.LoadingUI);
    }
}
