using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool IsAutoInteract { get; }
    public string Message { get; }


    public void Interact(ChrBase playerCharacter);

    public void ShowInfoBox(ChrBase playerCharacter);

    public void HideInfoBox(ChrBase playerCharacter);
}

