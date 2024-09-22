
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCharacter : ChrBase
{
    public static event Action<EnemyCharacter, int> OnEnemyDamaged;

    public override void OnDamaged(int damage)  
    {
        curHp -= damage;

        DamageTextCtrl.Instance.CreatePopup(new Vector3(transform.position.x + Random.Range(-0.2f, 0.2f),
                                                           transform.position.y + Random.Range(-0.2f, 0.2f),
                                                           transform.position.z), damage.ToString());
        OnEnemyDamaged?.Invoke(this,damage); 
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
