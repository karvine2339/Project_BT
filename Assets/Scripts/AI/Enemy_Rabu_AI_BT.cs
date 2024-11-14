using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Rabu_AI_BT : Tree
{
    private Transform player;
    private Transform enemy;
    private Enemy_Rabu enemyRabu;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private NavMeshAgent enemyNavMeshAgent;
    [SerializeField] private LayerMask obstacleMask;

    protected override Node SetupBehaviorTree()
    {
        player = PlayerCharacter.Instance.GetComponent<Transform>();
        enemy = this.transform;
        enemyRabu = GetComponent<Enemy_Rabu>();

        Node root = new SelectorNode(new List<Node>
        {
             new EnemyDeadNode(enemyAnimator, enemyNavMeshAgent, enemyRabu, this),

             new SequenceNode(new List<Node>
             {
                 new EnemyReloadNode(enemyRabu),

                 new SequenceNode(new List<Node>
                 {
                     new EnemySensorNode(enemy, player),
                     new Enemy_Rabu_SkillNode_1(enemyRabu),

                     new SelectorNode(new List<Node>
                     {
                           new EnemyAttackNode(player, enemy, enemyRabu, obstacleMask),
                           new EnemyMoveNode(player,enemy,enemyAnimator,enemyNavMeshAgent,enemyRabu)
                     }),
                 })
             }),
             new EnemyIdleNode(enemyRabu)
        });
        return root;
    }
}

