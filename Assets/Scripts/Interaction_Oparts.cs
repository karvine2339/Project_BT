using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Oparts : MonoBehaviour, IInteractable
{
    public bool IsAutoInteract => false;

    public string Message => "������";

    public void Interact(PlayerCharacter playerCharacter)
    {
        
    }
}
