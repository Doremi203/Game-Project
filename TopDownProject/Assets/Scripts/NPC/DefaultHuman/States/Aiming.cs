using UnityEngine;
using UnityEngine.AI;

public class Aiming : IState, IStateEnterCallbackReciver, IStateExitCallbackReciver, IStateTickCallbackReciver
{

    private HumanAI ai;
    private Actor npc;
    private NavMeshAgent agent;
    private WeaponHolder weaponHolder;
    private float ableToAttackTime;

    public Aiming(HumanAI ai, Actor npc, NavMeshAgent agent, WeaponHolder weaponHolder)
    {
        this.ai = ai;
        this.npc = npc;
        this.agent = agent;
        this.weaponHolder = weaponHolder;
    }

    public void OnEnter()
    {
        agent.speed = 2f;
        agent.ResetPath();
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        agent.SetDestination(Player.Instance.transform.position);
        npc.GetComponent<RotationController>().LookAtIgnoringY(Player.Instance.transform.position);
    }

}