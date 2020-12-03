using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShurikenAnimator : MonoBehaviour
{

    [SerializeField] private AbilityShurikens abilityShurikens;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        abilityShurikens.UseEvent.AddListener(() => animator.SetTrigger("Throw"));
    }

}