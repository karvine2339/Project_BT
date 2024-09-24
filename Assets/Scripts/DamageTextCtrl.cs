using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextCtrl : MonoBehaviour
{
    public static DamageTextCtrl Instance;
    public GameObject damageText;

    private void Awake()
    {
        Instance = this;
    }

    public void CreatePopup(Vector3 position, string text)
    {
        var popup = Instantiate(damageText,position,Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<Text>();
        temp.text = text;

        //Destroy Timer
        Destroy(popup,1f);  
    }
}
