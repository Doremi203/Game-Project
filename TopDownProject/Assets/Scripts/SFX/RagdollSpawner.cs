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
        owner = this.GetComponent<Actor>();
        agent = this.GetComponent<NavMeshAgent>();
        if (!agent)
            characterController = this.GetComponent<CharacterController>();
        owner.DeathEvent.AddListener(ActorDeath);
    }

    private void Update()
    {   
        if (owner.IsDead) return;
        if (agent)
            lastVelocity = agent.velocity;
        else
            lastVelocity = characterController.velocity;
    }

    private void ActorDeath()
    {
        bool lostHead = owner.LastDamageInfo.DamageType == DamageType.Melee && Random.Range(0, 5) == 0;

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
            item.velocity = lastVelocity;
        }

        Destroy(actualModel);
    }

}