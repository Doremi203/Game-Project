﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNPC : Actor
{

    [HideInInspector] public Actor Target;
    [HideInInspector] public Quaternion desiredRotation;

    public float RotationSpeed => rotationSpeed;

    [Header("BaseNPC")]
    [SerializeField] private float rotationSpeed;

    protected virtual void Update()
    {
        if (transform.rotation != desiredRotation)
        {
            this.transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
    }

}