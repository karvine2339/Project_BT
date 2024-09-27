
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCharacter : ChrBase
{
    public static event Action<EnemyCharacter, int> OnEnemyDamaged;

    public List<WeaponData> weaponList;
    public GameObject weaponPrefab;

    public override void OnDamaged(float damage, float criticalHit,float criticalDamage)  
    {
        criticalHit = Random.Range(0.0f, 100.0f);
        if(criticalHit <= PlayerStat.Instance.CriticalProbability)
        {
            damage *= (1.5f * criticalDamage);
            curHp -= (int)damage;

            DamageTextCtrl.Instance.CreateCriPopup(new Vector3(transform.position.x + Random.Range(-0.2f, 0.2f),
                                                               transform.position.y + Random.Range(-0.2f, 0.2f),
                                                               transform.position.z), damage.ToString("N0"));
        }
        else
        {
            curHp -= (int)damage;
            DamageTextCtrl.Instance.CreatePopup(new Vector3(transform.position.x + Random.Range(-0.2f, 0.2f),
                                                               transform.position.y + Random.Range(-0.2f, 0.2f),
                                                               transform.position.z), damage.ToString("N0"));
        }

        OnEnemyDamaged?.Invoke(this,(int)damage); 
        if (curHp <= 0)
        {
            DropWeapon(transform.position); 
            Die();
        }


    }

    public GameObject DropWeapon(Vector3 dropPos)
    {
        WeaponData randWeaponData = weaponList[Random.Range(0, weaponList.Count)];

        GameObject droppedWeapon = Instantiate(weaponPrefab, dropPos, Quaternion.identity);

        DroppedWeapon weaponComponent = droppedWeapon.GetComponent<DroppedWeapon>();

        weaponComponent.InitWeaponData(randWeaponData);

        return droppedWeapon;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
