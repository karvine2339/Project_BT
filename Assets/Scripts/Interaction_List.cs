using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interaction_List : MonoBehaviour
{
    public string nonSelectedMessage
    {
        get
        {
            return nonSelectedMessageText.text;
        }
        set
        {
            nonSelectedMessageText.text = value;
        }
    }

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
    public bool IsSelectedObject
    {
        get
        {
            return selectedObject.activeSelf;
        }
        set
        {
            selectedObject.SetActive(value);
            if (value == true)
            {
                rect.sizeDelta = new Vector2(350, 100);
            }
            else
            {
                rect.sizeDelta = new Vector2(350, 50);
            }
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
    [SerializeField] private TextMeshProUGUI nonSelectedMessageText;
    [SerializeField] private Outline outline;
    [SerializeField] private GameObject selectedObject;
    [SerializeField] private RectTransform rect;

    private IInteractable interactableData;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }
}

