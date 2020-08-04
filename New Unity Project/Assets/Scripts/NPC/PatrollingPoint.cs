using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PatrollingPoint : MonoBehaviour
{

    public static PatrollingPoint GetClosestPoint(Vector3 from)
    {
        PatrollingPoint _closestPoint = default;
        foreach (var item in GameObject.FindObjectsOfType<PatrollingPoint>())
        {
            if (_closestPoint == null)
            {
                _closestPoint = item;
                continue;
            }
            else
            {
                float oldDistance = Vector3.Distance(from, _closestPoint.transform.position);
                float newDistance = Vector3.Distance(from, item.transform.position);
                if (oldDistance > newDistance) _closestPoint = item;
            }
        }
        return _closestPoint;
    }

    [HideInInspector]
    public List<PatrollingPoint> patrollingPoints = new List<PatrollingPoint>();

    private float detectionDistance = 10f;

    private void Awake()
    {
        foreach (var item in GameObject.FindObjectsOfType<PatrollingPoint>())
        {
            if (item.gameObject == this.gameObject) continue;
            float distance = Vector3.Distance(transform.position, item.transform.position);
            if (distance > detectionDistance) continue;
            patrollingPoints.Add(item);
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(this.transform.position, this.transform.up, 1f);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(this.transform.position, this.transform.up, detectionDistance / 2);
    }

}