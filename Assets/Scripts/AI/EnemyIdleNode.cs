using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleNode : ActionNode
{
    public EnemyIdleNode(EnemyCharacter enemyCharacter) : base(() =>
    {
        enemyCharacter.SetIdleAnimState();
        return NodeState.Running;
    })
    {

    }
}
