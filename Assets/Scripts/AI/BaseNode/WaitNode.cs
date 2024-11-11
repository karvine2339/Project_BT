using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : Node
{
    private float waitTime;
    private float startTime;

    public WaitNode(float time)
    {
        waitTime = time;
        startTime = -1f;
    }

    public override NodeState Evaluate()
    {
        if (startTime < 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime >= waitTime)
        {
            startTime = -1f;
            return state = NodeState.Success;
        }

        return state = NodeState.Running;
    }
    // Node waitNode = new WaitNode(3f); // 3초 동안 대기
}