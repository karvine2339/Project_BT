using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveNode : ActionNode
{ 
    private Transform player;
    private Transform enemy;

    public EnemyMoveNode(Transform player, Transform enemy, Animator animator
        , NavMeshAgent agent,EnemyCharacter enemyCharacter) : base(() =>
    {
        if (!(Vector3.Distance(enemy.position, player.position) >= 20))
        {

            if (!(enemyCharacter.isReload || enemyCharacter.isDead || enemyCharacter.isAttackAnimate || enemyCharacter.isSkill))
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);

                enemyCharacter.SetMoveAnimState();
            }
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                return NodeState.Running;
            }
            else
            {
                return NodeState.Success;
            }
        }
        else
        {
            agent.isStopped = true;
            return NodeState.Failure;
        }

    })
    {
        this.player = player;
        this.enemy = enemy;
        this.animator = animator;
        this.agent = agent;
    }
}
