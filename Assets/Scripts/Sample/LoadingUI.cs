using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : UIBase
{ 
    
    public float LoadingProgress
    {
        set
        {
            loadingBar.fillAmount = value;
            loadingText.text = $"{value * 100f :0.0} %";
        }
    }

    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Image loadingBar;
}
