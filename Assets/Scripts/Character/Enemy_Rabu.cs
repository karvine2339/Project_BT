using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy_Rabu : EnemyCharacter
{
    public float skillTime = 0.0f;

    private bool isGrenade = false;

    public string enemyName = "¶óºê";
    public EnemyProjectile grenade;
    public Transform grenadeStartPosition;
    public static event Action<Enemy_Rabu, int> OnEnemyRabuDamaged;

    protected override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (!isDead)
        {
            skillTime += Time.deltaTime;
        }

        //if(isSkill)
        //{
        //    transform.rotation = Quaternion.LookRotation(player.transform.position);
        //}
        
    }

    public void SetSkillState()
    {
        if (isSkill == false)
        {
            agent.isStopped = true;
            animator.SetTrigger("Skill1");
            isSkill = true;
        }
    }

    public void StartSkill()
    { 
        if (isGrenade == false)
        {
            Vector3 grenadeDir = new Vector3(player.transform.position.x,player.transform.position.y + 0.5f, player.transform.position.z)
                                                                                                    - grenadeStartPosition.position;
            isGrenade = true;
            EnemyProjectile go = Instantiate(grenade, grenadeStartPosition.position, Quaternion.LookRotation(grenadeDir));
            go.GetComponent<Rigidbody>().velocity = grenadeDir * 3;
        }
    }

    public void EndSkill()
    {
        agent.isStopped = false;
        skillTime = 0.0f;
        isSkill = false;
        isGrenade = false;
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

        UpdateHpBar();

        OnEnemyRabuDamaged?.Invoke(this, (int)damage);

        if (curHp <= 0 && isDead == false)
        {
            isDead = true;
            DropWeapon(new Vector3(transform.position.x, transform.position.y + 0.35f, transform.position.z));
            CoinDropper.Instance.DropCoins(transform.position);
            Die();
        }

    }


}
