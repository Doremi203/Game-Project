﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(SphereCollider))]
public class Ki4ki : MonoBehaviour
{

    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minTargetSize;
    [SerializeField] private float maxTargetSize;
    [SerializeField] private float bloodyFootprintsTime = 4f;

    private DecalProjector decalProjector;
    private float targetSize;
    private float speed;
    private SphereCollider sphereCollider;

    private void Awake()
    {
        decalProjector = this.GetComponent<DecalProjector>();
        sphereCollider = this.GetComponent<SphereCollider>();
    }

    private void Start()
    {
        targetSize = Random.Range(minTargetSize, maxTargetSize);
        speed = Random.Range(minSpeed, maxSpeed);
        sphereCollider.radius = 0f;
    }

    private void Update()
    {
        if(decalProjector.size.x <= targetSize)
        {
            decalProjector.size = new Vector3(decalProjector.size.x + speed * Time.deltaTime, decalProjector.size.y + speed * Time.deltaTime, 0.2f);
            sphereCollider.radius = decalProjector.size.x / 2f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        BloodyFootprints _bloodyFootprints = other.GetComponent<BloodyFootprints>();
        if (_bloodyFootprints) _bloodyFootprints.SetBloodAmount(bloodyFootprintsTime);
    }

}