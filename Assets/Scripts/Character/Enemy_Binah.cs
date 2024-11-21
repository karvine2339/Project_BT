using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy_Binah : EnemyCharacter
{
    public static event Action<Enemy_Binah, int> OnEnemyBinahDamaged;

    public string enemyName = "ºñ³ª";

    public GameObject exParticalObject = null;

    private float rotationSpeed = 10.0f;
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<Collider>();
    }

    public override void Update()
    {
        if (exParticalObject != null)
        {
            exParticalObject.transform.position = fireStartPosition.position;
            exParticalObject.transform.rotation = fireStartPosition.rotation;
        }
        if(isSkill)
        {
            Rotate();
        }
    }

    public void Rotate()
    {
        Vector3 dir = PlayerCharacter.Instance.transform.position - transform.position;
        dir.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
    public override void OnDamaged(Vector3 contactPoint, float damage, float criticalDamage)
    {
        if (isDead)
            return;

        float criticalHit = Random.Range(0.0f, 100.0f);
        if (criticalHit <= PlayerStat.Instance.CriticalProbability)
        {
            damage *= (1.5f * criticalDamage);
            curHp -= (int)damage;


            DamageTextCtrl.Instance.CreateCriPopup(contactPoint, damage.ToString("N0"));
        }
        else
        {
            curHp -= (int)damage;
            DamageTextCtrl.Instance.CreatePopup(contactPoint, damage.ToString("N0"));
        }

        OnEnemyBinahDamaged?.Invoke(this, (int)damage);

        if (curHp <= 0 && isDead == false)
        {
            isDead = true;
            //DropWeapon(new Vector3(transform.position.x, transform.position.y + 0.35f, transform.position.z));
            CoinDropper.Instance.DropCoins(transform.position);
            //Die();
        }

    }

    public void Ex1_Start()
    {
        isSkill = true;
        exParticalObject = Instantiate(Resources.Load(Constant.BinahFlameStreamResourcePath) as GameObject);
    }

    public void Ex1_End()
    {
        isSkill = false;
        Destroy(exParticalObject.gameObject);
        exParticalObject = null;
    }

}
