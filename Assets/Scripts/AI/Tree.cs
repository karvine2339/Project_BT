using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class Tree : MonoBehaviour
{
    private Node rootNode;

    public bool isDead = false;

    protected void Start()
    {
        rootNode = SetupBehaviorTree();
    }

    protected void Update()
    {
        if (rootNode is null)
            return;

        if (!isDead)
        {
            rootNode.Evaluate();
        }
    }

    protected abstract Node SetupBehaviorTree();

}
