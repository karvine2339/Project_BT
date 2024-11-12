using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIBT : Tree
{
    private Transform player;
    private Transform enemy;
    private EnemyCharacter enemyCharacter;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private NavMeshAgent enemyNavMeshAgent;

    protected override Node SetupBehaviorTree()
    {
        player = PlayerCharacter.Instance.GetComponent<Transform>();
        enemy = this.transform;
        enemyCharacter = GetComponent<EnemyCharacter>();

        Node root = new SelectorNode(new List<Node>
        {
             new EnemyDeadNode(enemyAnimator, enemyNavMeshAgent, enemyCharacter, this), 

             new SequenceNode(new List<Node>
             {
                 new EnemySensorNode(enemy, player),           
                 new SelectorNode(new List<Node>                   
                 {
                     new SequenceNode(new List<Node>              
                     {
                           new EnemyAttackNode(player, enemy, enemyAnimator, enemyNavMeshAgent, enemyCharacter)
                     }),
                     new EnemyMoveNode(player, enemy, enemyAnimator, enemyNavMeshAgent) 
                 })
             }),
             new EnemyIdleNode(enemyAnimator, enemyNavMeshAgent) 
        });
        return root;
    }
}

