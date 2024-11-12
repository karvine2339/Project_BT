using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackNode : ActionNode
{
    private Transform player;
    private Transform enemy;
    private static int playerLayerMask = 1 << 9;

    public EnemyAttackNode(Transform player, Transform enemy, Animator animator,
            NavMeshAgent agent, EnemyCharacter enemyCharacter) : base(()=>
    {
        var collider = Physics.OverlapSphere(enemy.position, 5.0f, playerLayerMask);
        if (collider.Length <= 0)
        {
            Debug.Log("Attack Failure");
            return NodeState.Failure;
        }
        else
        {
            animator.SetFloat("Attack", 1);
            animator.SetFloat("Idle", 0);
            animator.SetFloat("Move", 0);
            agent.isStopped = true;
            Debug.Log("attack");

            return NodeState.Success;
        }

    })
    {
        this.player = player;
        this.enemy = enemy;
        this.animator = animator;
        this.agent = agent;
    }

}
