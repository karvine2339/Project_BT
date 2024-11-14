using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackNode : ActionNode
{
    private Transform player;
    private Transform enemy;

    private LayerMask obstacleMask;

    public EnemyAttackNode(Transform player, Transform enemy, EnemyCharacter enemyCharacter,LayerMask obstacleMask)
        : base(() =>
        {
            float distanceToPlayer = Vector3.Distance(enemy.position, player.position);
            float attackRange = 10.0f;
            if (distanceToPlayer > attackRange)
            {
                return NodeState.Failure; 
            }

            Vector3 directionToPlayer = (player.position - enemy.position).normalized;

            if (Physics.Raycast(enemyCharacter.raycastPosition.position, directionToPlayer, distanceToPlayer, obstacleMask))
            { 
                return NodeState.Failure; 
            }

            enemyCharacter.SetAttackAnimState();

            if (enemyCharacter.isAttackAnimate == true)
            {
                return NodeState.Running;
            }
            else
            {
                return NodeState.Success;
            }
        })
    {
        this.player = player;
        this.enemy = enemy;
        this.enemyCharacter = enemyCharacter;
        this.obstacleMask = obstacleMask;
    }
}