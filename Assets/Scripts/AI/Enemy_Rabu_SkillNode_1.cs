using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Rabu_SkillNode_1 : ActionNode
{
    public Enemy_Rabu_SkillNode_1(Enemy_Rabu enemyRabu)
          : base(() =>
            {
                if (enemyRabu.skillTime >= 10.0f)
                {
                    enemyRabu.SetSkillState();
                }
                
                if(enemyRabu.isSkill)
                {
                    return NodeState.Running;
                }
                else
                {
                    return NodeState.Success;
                }    

            })
    {
        this.enemyRabu = enemyRabu;
    }
}
