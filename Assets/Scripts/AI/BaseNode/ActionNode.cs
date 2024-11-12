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

}