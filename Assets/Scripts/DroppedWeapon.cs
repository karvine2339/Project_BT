using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedWeapon : MonoBehaviour, IInteractable
{
    public bool IsAutoInteract => false;
    public string Message => gameObject.name.ToString();

    [SerializeField] private int weaponMinDamage;
    [SerializeField] private int weaponMaxDamage;
    [SerializeField] private float weaponFireRate;

    private void Start()
    {
        weaponMinDamage = Random.Range(50, 100);
        weaponMaxDamage = Random.Range(150, 200);
        weaponFireRate = Random.Range(0.07f, 0.2f);
    }


    public void Interact(ChrBase playerCharacter)
    {

        PlayerStat.Instance.BulletMinDamage = weaponMinDamage;
        PlayerStat.Instance.BulletMaxDamage = weaponMaxDamage;
        PlayerStat.Instance.FireRate = weaponFireRate;

        Interaction_UI.Instance.RemoveInteractionData(this);

        Destroy(gameObject);

    }
}
