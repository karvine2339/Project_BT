using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class DamageTextCtrl : MonoBehaviour
{
    public static DamageTextCtrl Instance;
    public GameObject damageText;
    public GameObject criticalText;

    public Text temp;
    private void Awake()
    {
        Instance = this;
    }

    public void CreatePopup(Vector3 position, string text)
    {
        var popup = Instantiate(damageText,position,Quaternion.identity);
        temp = popup.transform.GetChild(0).GetComponent<Text>();

        //SetSpriteText(text);

        temp.text = text;

        Destroy(popup,1f);  
    }

    public void CreateCriPopup(Vector3 position, string text)
    {
        var popup = Instantiate(criticalText, position,Quaternion.identity);
        temp = popup.transform.GetChild(0).GetComponent<Text>();

        //SetSpriteText(text);

        temp.text = text;

        Destroy(popup, 1f);
    }

    //public void SetSpriteText(string input)
    //{
    //    string spriteText = "";
    //    foreach (char c in input)
    //    {
    //        if (c >= '0' && c <= '9') 
    //        {
    //            int spriteIndex = c - '0'; 
    //            spriteText += $"<color=#00FF22><sprite={spriteIndex}></color>"; 
    //        }
    //    }

    //    temp.text = spriteText; 
    //}
}
