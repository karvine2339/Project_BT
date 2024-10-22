using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool IsAutoInteract { get; }
    public string Message { get; }


    public void Interact(PlayerCharacter playerCharacter);
}

