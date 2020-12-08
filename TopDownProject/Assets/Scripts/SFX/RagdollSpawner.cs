using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Actor))]
public class RagdollSpawner : MonoBehaviour
{

    [SerializeField] private GameObject actualModel;
    [SerializeField] private GameObject ragdollPrefab;
    [SerializeField] private GameObject headlessRagdollPrefab;
    [SerializeField] private GameObject headPrefab;
    [SerializeField] private Transform headBone;

    private Actor owner;
    private NavMeshAgent agent;
    private CharacterController characterController;
    private Vector3 lastVelocity;

    private void Awake()
    {
        owner = GetComponent<Actor>();
        agent = GetComponent<NavMeshAgent>();
        if (!agent)
            characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        owner.HealthComponent.Died += Death;
    }

    private void Update()
    {   
        if (owner.HealthComponent.IsDead) return;
        if (agent)
            lastVelocity = agent.velocity;
        else
            lastVelocity = characterController.velocity;
    }

    private void Death(DamageInfo lastDamageInfo)
    {
        bool lostHead = lastDamageInfo.DamageType == DamageType.Melee && UnityEngine.Random.Range(0, 5) == 0;

        GameObject ragdollModel = default;

        if (lostHead)
        {
            ragdollModel = Instantiate(headlessRagdollPrefab, actualModel.transform.position, actualModel.transform.rotation);

            GameObject head = Instantiate(headPrefab, owner.transform.position + Vector3.down, headBone.transform.rotation);
            head.GetComponent<Rigidbody>().velocity = lastVelocity;
        }
        else
        {
            ragdollModel = Instantiate(ragdollPrefab, actualModel.transform.position, actualModel.transform.rotation);
        }

        foreach (var item in ragdollModel.GetComponentsInChildren<Rigidbody>())
        {
            item.velocity = lastVelocity + lastDamageInfo.Direction * 6f;
        }

        Destroy(actualModel);
    }

}