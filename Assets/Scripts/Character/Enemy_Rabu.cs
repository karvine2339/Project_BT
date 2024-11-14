using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class Enemy_Rabu : EnemyCharacter
{
    public float skillTime = 0.0f;

    private bool isGrenade = false;

    public EnemyGrenade grenade;
    public Transform grenadeStartPosition;


    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (!isDead)
        {
            skillTime += Time.deltaTime;
        }
        
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
            EnemyGrenade go = Instantiate(grenade, grenadeStartPosition.position, Quaternion.LookRotation(grenadeDir));
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

}
