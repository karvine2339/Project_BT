using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OopartsIcon : MonoBehaviour
{
    [HideInInspector] public Image[] oopartsimages;
    [HideInInspector] public string[] oopartsString = new string[2];
    [HideInInspector] public int oopartsIndex;

    public bool isActive = false;

    private void Awake()
    {
        oopartsimages = GetComponentsInChildren<Image>(false);
        oopartsString = new string[2];
        oopartsIndex = -1;
    }

    private void Start()
    {

    }
}
