using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalNode : Node
{
    private Node childNode;
    private Func<bool> condition;

    public ConditionalNode(Node child, Func<bool> condition)
    {
        childNode = child;
        this.condition = condition;
    }

    public override NodeState Evaluate()
    {
        if (condition())
        {
            return childNode.Evaluate();
        }
        return state = NodeState.Failure;
    }

    // Node someConditionNode = new ConditionalNode(new ActionNode(), () => playerHealth > 50);

}
