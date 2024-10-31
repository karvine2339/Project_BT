using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OopartsIcon : MonoBehaviour
{
    [HideInInspector] public Image[] oopartsimages;
    [HideInInspector] public TextMeshProUGUI[] oopartsString;
    [HideInInspector] public int oopartsIndex;

    public bool isActive = false;

    private void Awake()
    {
        oopartsimages = GetComponentsInChildren<Image>(true);
        oopartsString = GetComponentsInChildren<TextMeshProUGUI>(true); 

    }

    private void Start()
    {

    }
}
