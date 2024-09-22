using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{

    [SerializeField] private int weaponMinDamage;
    [SerializeField] private int weaponMaxDamage;
    [SerializeField] private float weaponFireRate;

    void Update()
    {

    }

    private void Start()
    {
        weaponMinDamage = Random.Range(50, 100);
        weaponMaxDamage = Random.Range(150, 200);
        weaponFireRate = Random.Range(0.07f, 0.2f);
    }

    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if(collision.transform.TryGetComponent(out PlayerCharacter player))
        {
            PlayerStat.Inst.BulletMinDamage = weaponMinDamage;
            PlayerStat.Inst.BulletMaxDamage = weaponMaxDamage;

            Destroy(gameObject);
        }
    }

    public int GetMinDamage()
    {
        return weaponMinDamage;
    }

    public int GetMaxDamage()
    {
        return weaponMaxDamage;
    }

    public float GetFireRate()
    {
        return weaponFireRate;
    }
}
