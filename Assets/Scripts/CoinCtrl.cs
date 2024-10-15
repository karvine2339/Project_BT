using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCtrl : MonoBehaviour, IInteractable
{
    public bool IsAutoInteract => true;

    public string Message => "";

    public void Interact(PlayerCharacter playerCharacter)
    {
        playerCharacter.credit += Random.Range(50, 100);
        HUDManager.Instance.UpdateCredit();
        Destroy(this.gameObject);
    }

    private float rotationSpeed = 100f;

    public int minCoinVal;
    public int maxCoinVal;
    // Update is called once per frame
    void Update()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationAmount, 0);
    }



    public void ShowInfoBox(PlayerCharacter playerCharacter)
    {

    }

    public void HideInfoBox(PlayerCharacter playerCharacter)
    {
  
    }
}
