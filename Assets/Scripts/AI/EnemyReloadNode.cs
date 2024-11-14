using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyReloadNode : ActionNode
{
    public EnemyReloadNode(EnemyCharacter enemyCharacter)
        : base(() =>
        {
            if (enemyCharacter.attackCount >= 5 && !enemyCharacter.reloadStarted)
            {
                enemyCharacter.SetReloadAnimState();
                enemyCharacter.reloadStarted = true; 
            }

            if (enemyCharacter.isReload)
            {
                return NodeState.Running;
            }

            else
            {
                enemyCharacter.reloadStarted = false;
                return NodeState.Success;
            }
        })
    {
        this.enemyCharacter = enemyCharacter;
    }
}
