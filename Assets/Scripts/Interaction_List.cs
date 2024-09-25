using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interaction_List : MonoBehaviour
{
    public string Message
    {
        get
        {
            return messageText.text;
        }
        set
        {
            messageText.text = value;
        }
    }

    public bool IsSelected
    {
        get
        {
            return selection.activeSelf;
        }
        set
        {
            selection.SetActive(value);
        }
    }

    public IInteractable InteractableData
    {
        get
        {
            return interactableData;
        }
        set
        {
            interactableData = value;
        }
    }

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject selection;

    private IInteractable interactableData;
}

