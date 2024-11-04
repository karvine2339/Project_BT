using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Shop : MonoBehaviour, IInteractable
{
    public bool IsAutoInteract => false;

    public string Message => "����";

    public void Interact(PlayerCharacter playerCharacter)
    {
        var shopManager = ShopManager.Instance;

        Time.timeScale = 0.0f;
        CursorSystem.Instance.SetCursorState(true);
        ShopManager.Instance.shopObject.SetActive(true);

        //shopManager.UpdateCredit();

        BTInputSystem.Instance.isShop = true;

        ShopManager.Instance.SetOoparts();
    }

}
