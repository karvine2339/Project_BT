using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interaction_List : MonoBehaviour
{
    public string selectedMessage
    {
        get
        {
            return selectedMessageText.text;
        }
        set
        {
            selectedMessageText.text = value;
        }
    }

    public bool IsSelected
    {
        get
        {
            return outline.enabled;
        }
        set
        {
            outline.enabled = value;
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

    [SerializeField] private TextMeshProUGUI selectedMessageText;
    [SerializeField] private Outline outline;
    [SerializeField] private RectTransform rect;

    private IInteractable interactableData;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }
}

