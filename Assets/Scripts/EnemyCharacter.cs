
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCharacter : ChrBase
{
    public static event Action<EnemyCharacter, int> OnEnemyDamaged;

    public override void OnDamaged(float damage)  
    {
        curHp -= (int)damage;

        DamageTextCtrl.Instance.CreatePopup(new Vector3(transform.position.x + Random.Range(-0.2f, 0.2f),
                                                           transform.position.y + Random.Range(-0.2f, 0.2f),
                                                           transform.position.z), damage.ToString("N0"));
        OnEnemyDamaged?.Invoke(this,(int)damage); 
        if (curHp <= 0)
        {
            Die();
        }


    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
