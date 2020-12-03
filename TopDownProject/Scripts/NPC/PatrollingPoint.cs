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

    public static PatrollingPoint GetClosestPoint(Vector3 from, int layerId)
    {
        PatrollingPoint _closestPoint = default;
        foreach (var item in GameObject.FindObjectsOfType<PatrollingPoint>())
        {
            if (item.layerId != layerId) continue;
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
    public List<PatrollingPoint> PatrollingPoints = new List<PatrollingPoint>();

    [Range(1f, 10f)]
    public float detectionDistance = 10f;
    public int layerId;

    private void Awake()
    {
        foreach (var item in GameObject.FindObjectsOfType<PatrollingPoint>())
        {
            if (item.layerId != layerId) continue;
            if (item.gameObject == this.gameObject) continue;
            float distance = Vector3.Distance(transform.position, item.transform.position);
            if (distance > this.detectionDistance + item.detectionDistance) continue;
            PatrollingPoints.Add(item);
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = layerId % 2 == 0 ? Color.yellow : Color.magenta;
        Handles.color = layerId % 2 == 0 ? Color.yellow : Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.5f);
        Handles.DrawWireDisc(this.transform.position, this.transform.up, detectionDistance);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var item in GameObject.FindObjectsOfType<PatrollingPoint>())
        {
            if (item.layerId != layerId) continue;
            if (item.gameObject == this.gameObject) continue;
            float distance = Vector3.Distance(transform.position, item.transform.position);
            if (distance > this.detectionDistance + item.detectionDistance) continue;
            Gizmos.DrawLine(this.transform.position, item.transform.position);
        }
    }

#endif

}