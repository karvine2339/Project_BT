using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleNode : Node
{
    private Animator anim;

    public EnemyIdleNode(Transform transform)
    {
        anim = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        anim.SetFloat("Move", 0);

        return state = NodeState.Running;
    }
}
