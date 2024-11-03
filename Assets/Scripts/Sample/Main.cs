using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    // Main Ŭ������ ������Ʈ�� ����������.
    // #1. ���� �ý����� �ʱ�ȭ.
    // #2. Scene�� �����ϴ� ����� �����ϰ� �ִ�.

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
        // BootStrapper Ŭ�������� ó���ϱ� ���� �ý��� �����ΰ͵��� �ʱ�ȭ.
        // ���� => Resources ���� Instantiate�� �ؼ� prefab�� �����ؾߵǴ� �ý��� �̶����..?

        // Next Workflow ? 
        // Game Scene[ex: Title Scene?] ���� �̵�.

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
        // Loading UI�� �������.

        LoadingUI loadingUI = UIManager.Show<LoadingUI>(UIList.LoadingUI);
        loadingUI.LoadingProgress = 0;

        AsyncOperation emptySceneLoad = SceneManager.LoadSceneAsync("Empty", LoadSceneMode.Additive);
        while(!emptySceneLoad.isDone)
        {
            yield return null;
        }

        // ���࿡ ������ �ҷ��ٳ��� Scene�� �ִٸ�?
        // => ���� Scene�� SceneEnd�� ȣ���ϰ� �����Ѵ�.

        if (currentSceneController != null)
        {
            yield return StartCoroutine(currentSceneController.SceneEnd());
            loadingUI.LoadingProgress = 0.4f;
            yield return new WaitForSeconds(1f);
        }

        //���� Scene�� ���ŵǰ� ����, ���ο� Scene�� �ε��Ѵ�.
        yield return StartCoroutine(newSceneController.SceneStart());
        loadingUI.LoadingProgress = 0.8f;
        yield return new WaitForSeconds(1f);
        currentSceneController = newSceneController;

        // Loading UI�� �ٽ� �ݾ�����.

        loadingUI.LoadingProgress = 1f;

        yield return new WaitForSeconds(1f);

        UIManager.Hide<LoadingUI>(UIList.LoadingUI);
    }
}
