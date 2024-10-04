using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool IsAutoInteract { get; }
    public string Message { get; }


    public void Interact(PlayerCharacter playerCharacter);

    public void ShowInfoBox(PlayerCharacter playerCharacter);

    public void HideInfoBox(PlayerCharacter playerCharacter);
}

