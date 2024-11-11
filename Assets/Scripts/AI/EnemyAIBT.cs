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

             new SequenceNode(new List<Node> // Sequence: 센서 감지 후 이동 및 공격 실행
             {
                 new EnemySensorNode(enemy, player),           // 20의 범위로 센서 감지
                 new SelectorNode(new List<Node>                   // 공격 범위(10)를 체크하기 위한 Selector
                 {
                     new SequenceNode(new List<Node>               // 공격 범위 내일 때
                     {
                           new EnemyAttackNode(player, enemy, enemyAnimator, enemyNavMeshAgent, enemyCharacter)
                     }),
                     new EnemyMoveNode(player, enemy, enemyAnimator, enemyNavMeshAgent) // 공격 범위 밖일 때 이동
                 })
             }),
             new EnemyIdleNode(enemyAnimator, enemyNavMeshAgent) // 이동이나 공격을 하지 않을 경우 Idle 상태 유지
        });
        return root;
    }
}

