using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleNode : ActionNode
{
    public EnemyIdleNode(Animator animator, NavMeshAgent agent) : base(() =>
    {
        animator.SetFloat("Move", 0);
        animator.SetFloat("Attack", 1);
        animator.SetFloat("Idle", 1);

        agent.isStopped = true;

        return NodeState.Running;
    })
    {
        this.animator = animator;
        this.agent = agent;
    }
}
