using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySensorNode : ActionNode
{
    private static int playerLayerMask = 1 << 9;
    private Transform enemy;
    private Transform player;

    public EnemySensorNode(Transform enemy,Transform player) : base(()=>
    {

        var collider = Physics.OverlapSphere(enemy.position, 20.0f, playerLayerMask);
        if (collider.Length <= 0)
        {
            return NodeState.Failure;
        }
        else
        { 
            return NodeState.Success;
        }
    })
    {
        this.enemy = enemy;
        this.player = player;
    }
}
