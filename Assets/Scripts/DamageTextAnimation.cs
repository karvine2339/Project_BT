using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Damage
{
    damage,
    critical
}
public class DamageTextAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;

    private Text tmp;
    private float time = 0;
    private Vector3 origin;


    private void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<Text>();
        origin = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "DefaultDamage")
        {
            tmp.color = new Color(255, 255, 255, opacityCurve.Evaluate(time));
        }
        else if(gameObject.tag == "CriticalDamage")
        {
            tmp.color = new Color(1, 0.5f, 0, opacityCurve.Evaluate(time));
        }
        transform.localScale = Vector3.one *scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(0, 1 + heightCurve.Evaluate(time), 0);

        time += Time.deltaTime;
    }
}
