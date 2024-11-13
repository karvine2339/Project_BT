using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Rabu : EnemyCharacter
{
    public float skillTime = 0.0f;

    public bool isSkill = false;
    private bool isGrenade = false;

    public Projectile grenade;
    public Transform grenadeStartPosition;


    protected override void Start()
    {
       animator = GetComponent<Animator>();
       agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!isDead)
        {
            skillTime += Time.deltaTime;
        }

        Debug.Log(skillTime);
        
    }

    public void SetSkillState()
    {
        animator.SetTrigger("Skill1");

        if (isGrenade == false)
        {
            isGrenade = true;
            Projectile go = Instantiate(grenade);
            go.transform.position = grenadeStartPosition.position;
            go.SetForce(100);
        }
        
        
        isSkill = true;
    }

    public void EndSkill()
    {
        skillTime = 0.0f;
        isSkill = false;
    }
}
