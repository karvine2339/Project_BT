using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RightClickButton : MonoBehaviour, IPointerDownHandler , IPointerUpHandler
{
    public Button button;

    public bool isHolding = false;
    public float holdingTime = 1.0f;
    public float holdingTimer = 0.0f;


    private void Update()
    {
        if (isHolding)
        {
            holdingTimer += Time.unscaledDeltaTime;

            if (holdingTimer > holdingTime) 
            {
                isHolding = false;
                if (button.gameObject.name.Contains("1"))
                {

                    WeaponDropManager.Instance.ThrowFirstWeapon();
                    Debug.Log("Throw 1");
                }
                else
                {
                    WeaponDropManager.Instance.ThrowSecondWeapon();
                    Debug.Log("Throw 2");
                }

                holdingTimer = 0.0f;
            }
        }

        Debug.Log(holdingTimer);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isHolding = true;
            holdingTimer = 0.0f;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            holdingTimer = 0.0f;
            isHolding = false;
        }
    }
}
