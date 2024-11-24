using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Binah_AI_BT : Tree
{
    private Transform player;
    private Transform enemy;
    private Enemy_Binah enemyBinah;
    [SerializeField] private Animator enemyBinahAnimator;
    [SerializeField] private LayerMask obstacleMask;

    protected override Node SetupBehaviorTree()
    {
        player = PlayerCharacter.Instance.GetComponent<Transform>();
        enemy = this.transform;
        enemyBinah = GetComponent<Enemy_Binah>();

        Node root = new SelectorNode(new List<Node>
        {
            new RandomSelectorNode(new List<Node>
            {
                new Enemy_Binah_SkillNode_1(enemyBinah),
                new Enemy_Binah_SkillNode_2(enemyBinah)
            })
        });
        return root;
    }
}
