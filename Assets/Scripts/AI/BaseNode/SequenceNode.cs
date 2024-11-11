using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : Node
{
    public SequenceNode() : base() { }

    public SequenceNode(List<Node> children) : base(children) { }

    public override NodeState Evaluate()
    {
        bool bNowRunning = false;
        foreach (Node node in childrenNode)
        {
            switch (node.Evaluate())
            {
                case NodeState.Failure:
                    return state = NodeState.Failure;
                case NodeState.Success:
                    continue;
                case NodeState.Running:
                    bNowRunning = true;
                    continue;
                default:
                    continue;
            }
        }

        return state = bNowRunning ? NodeState.Running : NodeState.Success;
    }
}
