using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeadNode : Node
{
    private Tree tree;

    private bool isDead = false;
    public EnemyDeadNode(Animator animator, NavMeshAgent agent, EnemyCharacter enemyCharacter,Tree tree)
    : base(animator, agent, enemyCharacter)
    {
        this.tree = tree;
    }

    public override NodeState Evaluate()
    {
        if (enemyCharacter.curHp <= 0 && isDead == false)
        {
            agent.isStopped = true;
            animator.SetTrigger("Death");
            tree.isDead = true;
            isDead = true;
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
