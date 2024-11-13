using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReloadNode : ActionNode
{
    public EnemyReloadNode(EnemyCharacter enemyCharacter)
          : base(() =>
          {
              if (enemyCharacter.attackCount >= 5)
              {
                  enemyCharacter.SetReloadAnimState();
                  return NodeState.Running;
              }
              else if(enemyCharacter.isReload == true)
              {
                  return NodeState.Success;
              }
             
              else
              {
                  return NodeState.Failure;
              }
          })
    {
        this.enemyCharacter = enemyCharacter;
    }
}
