using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Binah_SkillNode_2 : ActionNode
{
    public Enemy_Binah_SkillNode_2(Enemy_Binah enemyBinah)
      : base(() =>
      {
          enemyBinah.SetSkillState(2);

          return NodeState.Success;

      })
    {
        this.enemyBinah = enemyBinah;
    }
}
