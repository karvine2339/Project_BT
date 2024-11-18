using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootStrapper : MonoBehaviour
{
    public static bool IsSystemLoaded { get; private set; } = false;

    private static List<string> AutoBootStrapperScenes = new List<string>()
    {
        "MainScene",
        "SampleScene", // <- Add Scene
    };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void SystemBoot()
    {
        IsSystemLoaded = false;

#if UNITY_EDITOR
        var activeScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
        for(int i = 0; i < AutoBootStrapperScenes.Count; i++)
        {
            if (activeScene.name.Equals(AutoBootStrapperScenes[i]))
            {
                InternalSystemBoot();
                break;
            }
        }
        IsSystemLoaded = true;
#else
        InternalSystemBoot();
        IsSystemLoaded = true;
#endif
    }

    private static void InternalSystemBoot()
    {
        // UI System ÃÊ±âÈ­
        UIManager.Singleton.Initialize();
    }
}
