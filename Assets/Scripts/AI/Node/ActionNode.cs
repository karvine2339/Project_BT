using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : Node
{
    private Func<NodeState> action;

    public ActionNode(Func<NodeState> action)
    {
        this.action = action;
    }

    public override NodeState Evaluate()
    {
        if (action != null)
        {
            return state = action.Invoke();
        }
        return state = NodeState.Failure;
    }

    //Node actionNode = new ActionNode(() =>
    //{
    //    // 특정 동작을 수행
    //    Debug.Log("Executing action");
    //    return NodeState.Success;
    //});
}