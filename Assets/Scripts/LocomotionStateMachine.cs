using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionStateMachine : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ChrBase uckCharacterController = animator.GetComponent<ChrBase>();
        //uckCharacterController.IsPossibleMovement = true;
        //uckCharacterController.IsPossibleAttack = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ChrBase uckCharacterController = animator.GetComponent<ChrBase>();
        //uckCharacterController.IsPossibleMovement = false;
        //uckCharacterController.IsPossibleAttack = false;
    }
}

