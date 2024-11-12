using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveNode : ActionNode
{ 
    private Transform player;
    private Transform enemy;

    public EnemyMoveNode(Transform player, Transform enemy, Animator animator, NavMeshAgent agent) : base(() =>
    {
        if (Vector3.Distance(enemy.position, player.position) > 5)
        {

            enemy.LookAt(player);
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetFloat("Move", 1);
            animator.SetFloat("Attack", 0);
            animator.SetFloat("Idle", 0);
            Debug.Log("Move");
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
