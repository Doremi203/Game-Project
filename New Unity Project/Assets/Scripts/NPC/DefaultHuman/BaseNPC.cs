using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNPC : Actor
{

    [HideInInspector] public Actor Target;
    [HideInInspector] public Vector3 desiredRotation;

    public float RotationSpeed => rotationSpeed;

    [Header("BaseNPC")]
    [SerializeField] private float rotationSpeed;

    protected virtual void Update()
    {
        if (transform.eulerAngles != desiredRotation)
        {
            this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, desiredRotation, rotationSpeed * Time.deltaTime);
        }
    }

}