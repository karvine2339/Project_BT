using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_WorkShop : MonoBehaviour,IInteractable
{
    public GameObject workShopObject;

    public bool IsAutoInteract => false;

    public string Message => "��ũ��";

    public void Interact(PlayerCharacter playerCharacter)
    {
        
    }
}
