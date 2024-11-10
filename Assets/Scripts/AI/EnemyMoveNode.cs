using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveNode : Node
{
    private Transform player;
    private Transform transform;
    private Animator anim;
    private NavMeshAgent agent;

    public EnemyMoveNode(Transform player, Transform transform)
    {
        this.player = player;
        this.transform = transform;
        anim = transform.GetComponent<Animator>();
        agent = transform.GetComponent<NavMeshAgent>();
    }

    public override NodeState Evaluate()
    {
        transform.LookAt(player);
        agent.SetDestination(player.position);
        anim.SetFloat("Move", 1);

        return state = NodeState.Running;
    }
}
