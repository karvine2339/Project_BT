using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIBT : Tree
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform enemy;

    protected override Node SetupBehaviorTree()
    {
        Node root = new SelectorNode(new List<Node>
        {
            new SequenceNode(new List<Node>
            {
                new EnemySensorNode(enemy),
                new EnemyIdleNode(enemy)
            }),
            new EnemyMoveNode(player,enemy)
        });
        return root;
    }
}
