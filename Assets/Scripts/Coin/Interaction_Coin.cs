using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Coin : MonoBehaviour, IInteractable
{
    public bool IsAutoInteract => true;

    private int credit;
    public string Message => "";

    public void Interact(PlayerCharacter playerCharacter)
    {
        credit = Random.Range(50, 100);
        playerCharacter.credit += playerCharacter.CalculateOopartsValue(credit,playerCharacter.IncreaseGainCoin);
        HUDManager.Instance.UpdateCredit(credit);
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
}
