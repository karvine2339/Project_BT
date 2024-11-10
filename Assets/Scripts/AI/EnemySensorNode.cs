using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySensorNode : Node
{
    private static int playerLayerMask = 1 << 9;
    private Transform transform;

    private Animator anim;
    private NavMeshAgent agent;

    public EnemySensorNode(Transform transform)
    {
        this.transform = transform;
        anim = transform.GetComponent<Animator>();
        agent = transform.GetComponent<NavMeshAgent>();
    }

    public override NodeState Evaluate()
    {
        var collider = Physics.OverlapSphere(transform.position, 5.0f, playerLayerMask);
        if (collider.Length <= 0)
            return state = NodeState.Failure;

        return state = NodeState.Success;
    }
}
