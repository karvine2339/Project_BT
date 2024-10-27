using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCharacter : MonoBehaviour
{ 
    public static event Action<EnemyCharacter, int> OnEnemyDamaged;

    public List<WeaponData> weaponList;
    public GameObject[] weaponPrefab;

    public bool isDead = false;

    public int curHp = 100;
    public int maxHp = 100;

    public void OnDamaged(float damage,float criticalDamage)  
    {
        float criticalHit = Random.Range(0.0f, 100.0f);
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
        if (curHp <= 0 && isDead == false)
        {
            isDead = true;
            DropWeapon(new Vector3(transform.position.x, transform.position.y + 0.35f, transform.position.z));
            CoinDropper.Instance.DropCoins(transform.position);
            Die();
        }


    }

    public GameObject DropWeapon(Vector3 dropPos)
    {
        GameObject droppedWeapon = Instantiate(weaponPrefab[Random.Range(0,weaponPrefab.Length)], dropPos, Quaternion.Euler(-30,-90,90));    

        DroppedWeapon prefabComponent = droppedWeapon.GetComponent<DroppedWeapon>();

        WeaponData weaponData = prefabComponent.weaponData;

        DroppedWeapon weaponComponent = droppedWeapon.GetComponent<DroppedWeapon>();
        weaponComponent.InitWeaponData(weaponData);

        return droppedWeapon;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
