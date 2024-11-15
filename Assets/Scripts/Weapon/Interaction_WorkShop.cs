using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interaction_WorkShop : MonoBehaviour, IInteractable
{
    public bool IsAutoInteract => false;

    public string Message => "¿öÅ©¼¥";

    public void Interact(PlayerCharacter playerCharacter)
    {
        var workShopManager = WorkShopManager.Instance;

        Time.timeScale = 0.0f;
        CursorSystem.Instance.SetCursorState(true);
        WorkShopManager.Instance.workShopObject.SetActive(true);

        workShopManager.UpdateCredit();
        workShopManager.InitFirstWeapon();
        workShopManager.InitSecondWeapon();

        workShopManager.SetPriceText();

        BTInputSystem.Instance.isWorkShop = true;
    }

}
