using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OopartsManager : MonoBehaviour
{
    public static OopartsManager Instance { get; private set; }

    public GameObject oopartsCanvasObject;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
