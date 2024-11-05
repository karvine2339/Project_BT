using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Shop : MonoBehaviour, IInteractable
{
    public bool IsAutoInteract => false;

    public string Message => "ªÛ¡°";

    private bool isInteract = false;

    public void Interact(PlayerCharacter playerCharacter)
    {
        var shopManager = ShopManager.Instance;

        Time.timeScale = 0.0f;
        CursorSystem.Instance.SetCursorState(true);
        ShopManager.Instance.shopObject.SetActive(true);

        //shopManager.UpdateCredit();

        BTInputSystem.Instance.isShop = true;

        if (false == isInteract)
        {
            ShopManager.Instance.SetOoparts();
        }

        isInteract = true;
    }

}
