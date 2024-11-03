using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    // Main 클래스는 프로젝트의 시작점으로.
    // #1. 각종 시스템의 초기화.
    // #2. Scene을 관리하는 기능을 내포하고 있다.

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
        // BootStrapper 클래스에서 처리하기 힘든 시스템 구조인것들을 초기화.
        // 예시 => Resources 에서 Instantiate를 해서 prefab을 생성해야되는 시스템 이라던지..?

        // Next Workflow ? 
        // Game Scene[ex: Title Scene?] 으로 이동.

        LoadScene(SceneType.GameScene);
        
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
        // Loading UI를 띄워놓자.

        LoadingUI loadingUI = UIManager.Show<LoadingUI>(UIList.LoadingUI);
        loadingUI.LoadingProgress = 0;

        AsyncOperation emptySceneLoad = SceneManager.LoadSceneAsync("Empty", LoadSceneMode.Additive);
        while(!emptySceneLoad.isDone)
        {
            yield return null;
        }

        // 만약에 기존에 불러다놓은 Scene이 있다면?
        // => 기존 Scene의 SceneEnd를 호출하고 제거한다.

        if (currentSceneController != null)
        {
            yield return StartCoroutine(currentSceneController.SceneEnd());
            loadingUI.LoadingProgress = 0.4f;
            yield return new WaitForSeconds(1f);
        }

        //기존 Scene이 제거되고 나면, 새로운 Scene을 로드한다.
        yield return StartCoroutine(newSceneController.SceneStart());
        loadingUI.LoadingProgress = 0.8f;
        yield return new WaitForSeconds(1f);
        currentSceneController = newSceneController;

        // Loading UI를 다시 닫아주자.

        loadingUI.LoadingProgress = 1f;

        yield return new WaitForSeconds(1f);

        UIManager.Hide<LoadingUI>(UIList.LoadingUI);
    }
}
