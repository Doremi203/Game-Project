using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{

    [SerializeField] private float rotationSpeed = 10f;

    private Quaternion desiredRotation;

    public void SetDesiredRotation(Quaternion rotation)
    {
        desiredRotation = rotation;
    }

    public void LookAtIgnoringY(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;
        if (direction == Vector3.zero) return;
        LookInDirectionIgnoringY(direction);
    }

    public void LookInDirectionIgnoringY(Vector3 direction)
    {
        desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    private void Update()
    {
        if (transform.rotation == desiredRotation) return;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }

}