
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCharacter : ChrBase
{
    public static event Action<EnemyCharacter, int> OnEnemyDamaged;

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
            Debug.Log("Critical Hit!");
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
            Die();
            characterAnimator.SetTrigger("Death");
        }


    }

    private void Die()
    {
        //Destroy(gameObject);
    }
}
