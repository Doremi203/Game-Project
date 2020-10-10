using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Actor))]
public class NewRagdollTest : MonoBehaviour
{

    [SerializeField] private GameObject actualModel;
    [SerializeField] private GameObject ragdollPrefab;
    [SerializeField] private GameObject headPrefab;
    [SerializeField] private Transform headBone;

    private Actor owner;
    private NavMeshAgent agent;
    private Vector3 lastVelocity;

    private void Awake()
    {
        owner = this.GetComponent<Actor>();
        agent = owner.GetComponent<NavMeshAgent>();
        owner.DeathEvent.AddListener(ActorDeath);
    }

    private void Update()
    {
        if (owner.IsDead) return;
        lastVelocity = agent.velocity;
    }

    private void ActorDeath()
    {
        GameObject ragdollModel = Instantiate(ragdollPrefab, actualModel.transform.position, actualModel.transform.rotation);

        foreach (var item in ragdollModel.GetComponentsInChildren<Rigidbody>())
        {
            item.AddForce(lastVelocity, ForceMode.VelocityChange);
        }

        GameObject head = Instantiate(headPrefab, headBone.transform.position + Vector3.up * 0.25f, headBone.transform.rotation);
        head.GetComponent<Rigidbody>().AddForce(lastVelocity, ForceMode.VelocityChange);

        Destroy(actualModel);
    }

}