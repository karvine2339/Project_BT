using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour, IInteractable
{
    public bool IsAutoInteract => false;
    public string Message => gameObject.name.ToString();

    public void Interact(ChrBase playerCharacter)
    {
        Debug.Log("ChestBox Interact");

        Interaction_UI.Instance.RemoveInteractionData(this);

        Destroy(gameObject);
    }
}