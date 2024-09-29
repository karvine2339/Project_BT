using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextCtrl : MonoBehaviour
{
    public static DamageTextCtrl Instance;
    public GameObject damageText;
    public GameObject criticalText;

    public TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        Instance = this;
    }

    public void CreatePopup(Vector3 position, string text)
    {
        var popup = Instantiate(damageText,position,Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<Text>();
        temp.text = text;

        SetSpriteText(text);

        //Destroy Timer
        Destroy(popup,1f);  
    }

    public void CreateCriPopup(Vector3 position, string text)
    {
        var popup = Instantiate(criticalText, position,Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<Text>();
        temp.text = text;

        //Destroy Timer
        Destroy(popup, 1f);
    }

    public void SetSpriteText(string input)
    {
        string spriteText = "";
        foreach (char c in input)
        {
            if (c >= '0' && c <= '9') // 숫자일 경우
            {
                int spriteIndex = c - '0'; // '0'을 빼서 인덱스를 계산
                spriteText += $"<sprite={spriteIndex}>"; // 해당 스프라이트 태그 추가
            }
        }

        textMeshPro.text = spriteText; // 변환된 텍스트 설정
    }
}
