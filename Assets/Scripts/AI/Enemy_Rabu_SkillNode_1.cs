using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rabu_SkillNode_1 : ActionNode
{
    public Enemy_Rabu_SkillNode_1(Enemy_Rabu enemyRabu)
          : base(() =>
            {
                if (enemyRabu.skillTime >= 10.0f)
                {
                    enemyRabu.SetSkillState();
                    return NodeState.Running;
                }
                
                else if(enemyRabu.isSkill == true)
                {
                    return NodeState.Success;
                }
                else
                {
                    return NodeState.Failure;
                }
            })
    {
        this.enemyRabu = enemyRabu;
    }
}
