using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using AC;

public class SetTarget : StateMachineBehaviour
{
    public bool isPlayer;
    public GameObject interactionObject;
    public GameObject interactionSystem;
    public string interactionObjectName;
    public bool interrupt;
    private FullBodyBipedEffector effector;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (interactionObject | interactionSystem == null){ 
            Debug.Log("Object not Assigned!!");
            
        }

        interactionObject = GameObject.Find(interactionObjectName);
        if (!interactionObject) { Debug.Log("Interaction Object not Found!"); }

        if (isPlayer)
        {
            Player _player = KickStarter.player;

            if (_player)
            {   interactionSystem = _player.gameObject; }
            else
            {
                interactionSystem = null;
                Debug.Log("Player not Found!");
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
        /*if (interactionSystem && interactionObject)
        {
            InteractionSystem interactionSystemScript = interactionSystem.GetComponent<InteractionSystem>();
            InteractionObject interactionObjectScript = interactionObject.GetComponent<InteractionObject>();
            if (interactionSystemScript && interactionObjectScript)
            {
                interactionSystemScript.StartInteraction(effector, interactionObjectScript, interrupt);
            }
        }*/

        float reach = animator.GetFloat("RightHandReach");


        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
        animator.SetIKPosition(AvatarIKGoal.RightHand, interactionObject.transform.position);

    }
}
