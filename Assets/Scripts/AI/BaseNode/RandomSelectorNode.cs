using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelectorNode : Node
{
    private System.Random random = new System.Random();

    public RandomSelectorNode(List<Node> children) : base(children) { }

    public override NodeState Evaluate()
    {
        List<Node> nodes = new List<Node>(childrenNode);

        while (nodes.Count > 0)
        {
            int index = random.Next(nodes.Count);
            Node node = nodes[index];

            switch (node.Evaluate())
            {
                case NodeState.Failure:
                    nodes.RemoveAt(index);
                    continue;
                case NodeState.Success:
                    return state = NodeState.Success;
                case NodeState.Running:
                    return state = NodeState.Running;
            }
        }

        return state = NodeState.Failure;
    }
}