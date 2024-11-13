using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NodeState
{
    Running,
    Failure,
    Success
}

public abstract class Node
{
    protected NodeState state;
    public Node parentNode;
    protected List<Node> childrenNode = new List<Node>();

    protected Animator animator;
    protected NavMeshAgent agent;
    protected EnemyCharacter enemyCharacter;
    protected Enemy_Rabu enemyRabu;

    public Node(Animator animator, NavMeshAgent agent, EnemyCharacter enemyCharacter)
    {
        this.animator = animator;
        this.agent = agent;
        this.enemyCharacter = enemyCharacter;   
    }
    public Node(Enemy_Rabu enemyRabu)
    {
        this.enemyRabu = enemyRabu;
    }
    public Node()
    {
        parentNode = null;
    }

    public Node(List<Node> children)
    {
        foreach (var child in children)
        {
            AttatchChild(child);
        }
    }

    public void AttatchChild(Node child)
    {
        childrenNode.Add(child);
        child.parentNode = this;
    }

    public abstract NodeState Evaluate();
}