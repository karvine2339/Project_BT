using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Binah_SkillNode_1 : ActionNode
{
    public Enemy_Binah_SkillNode_1(Enemy_Binah enemyBinah)
          : base(() =>
          {
              enemyBinah.SetSkillState(1);

              return NodeState.Success;

          })
    {
        this.enemyBinah = enemyBinah;
    }
}
