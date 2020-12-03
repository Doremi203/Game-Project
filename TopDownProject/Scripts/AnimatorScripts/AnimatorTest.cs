using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTest : StateMachineBehaviour
{

    [SerializeField] private GameObject prefab;

    private GameObject currentObject;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        currentObject = Instantiate(prefab, animator.transform.position, animator.transform.rotation);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }

}