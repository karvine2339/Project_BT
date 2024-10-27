using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interatcion_Ooparts : MonoBehaviour, IInteractable
{
    public bool IsAutoInteract => false;

    public string Message => "¿ÀÆÄÃ÷";

    public void Interact(PlayerCharacter playerCharacter)
    {
        OopartsManager.Instance.oopartsCanvasObject.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
        CursorSystem.Instance.SetCursorState(true);
        BTInputSystem.Instance.isOp = true;
        //Destroy(gameObject);
        Interaction_UI.Instance.RemoveInteractionData(this);
    }
}
