using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PlayerController))]
public class TEST : MonoBehaviour
{

    public PlayerController PlayerController => controller;
    public Vector3 Destination => destianation;

    [SerializeField] private float recalculateRate = 0.4f;
    [SerializeField] private bool showDebug;

    private PlayerController controller;
    private NavMeshAgent agent;
    private Vector3 destianation;
    private float nextRecalculate;
    private NavMeshPath currentPath;
    private int targetCorner;

    public Vector3 TargetCorner()
    {
        Vector3 vector = default;
        if(currentPath != null)
        {
            if(currentPath.corners.Length >= targetCorner)
            {
                vector = currentPath.corners[targetCorner];
            }
        }
        return vector;
    }

    public void SetDestination(Vector3 destianation)
    {
        this.destianation = destianation;
        //nextRecalculate = 0;
    }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        currentPath = new NavMeshPath();
    }

    private void Update()
    {
        if (Time.time >= nextRecalculate) CalculatePath();
        if(currentPath.corners.Length > targetCorner)
        {         
            if(Utils.GetDistance2D(transform.position, currentPath.corners[targetCorner]) > 0.4f)
            {
                controller.SetVelocity((currentPath.corners[targetCorner] - transform.position).normalized);
                Debug.Log((currentPath.corners[targetCorner] - transform.position).normalized);
            }
            else
            {
                if (currentPath.corners.Length > targetCorner + 1)
                {
                    Debug.Log(currentPath.corners.Length + " | " + targetCorner + 1);
                    targetCorner++;
                }
            }
        }
    }

    private void CalculatePath()
    {
        nextRecalculate = Time.time + recalculateRate;
        targetCorner = 1;
        currentPath.ClearCorners();
        agent.CalculatePath(destianation, currentPath);
    }

    private void OnDrawGizmos()
    {
        if (showDebug == false) return;
        if (currentPath == null) return;
        Gizmos.color = Color.red;

        for (int i = 0; i < currentPath.corners.Length - 1; i++)
        {
            Gizmos.DrawLine(currentPath.corners[i], currentPath.corners[i + 1]);
        }

    }

}