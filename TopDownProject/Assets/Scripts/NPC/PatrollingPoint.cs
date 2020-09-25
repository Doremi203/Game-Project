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

   
    public List<PatrollingPoint> PatrollingPoints = new List<PatrollingPoint>();

    [Range(1f, 10f)]
    public float detectionDistance = 10f;

    private void Awake()
    {
        foreach (var item in GameObject.FindObjectsOfType<PatrollingPoint>())
        {
            if (item.gameObject == this.gameObject) continue;
            float distance = Vector3.Distance(transform.position, item.transform.position);
            if (distance > this.detectionDistance + item.detectionDistance) continue;
            PatrollingPoints.Add(item);
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(this.transform.position, this.transform.up, detectionDistance);
    }

#endif

}