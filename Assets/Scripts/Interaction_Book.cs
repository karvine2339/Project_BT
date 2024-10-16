using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Book : MonoBehaviour, IInteractable
{
    public bool IsAutoInteract => false;

    public string Message => "전술 교본";

    public void HideInfoBox(PlayerCharacter playerCharacter)
    {

    }

    public void Interact(PlayerCharacter playerCharacter)
    {
        Destroy(gameObject);
        Interaction_UI.Instance.RemoveInteractionData(this);
    }

    public void ShowInfoBox(PlayerCharacter playerCharacter)
    {

    }
}
