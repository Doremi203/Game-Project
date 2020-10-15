using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicalProp : MonoBehaviour
{

    [SerializeField] private BoolParameter physicalObjectsSetting;

    private new Rigidbody rigidbody;

    private void Awake() => rigidbody = GetComponent<Rigidbody>();

    private void Start()
    {
        rigidbody.isKinematic = !physicalObjectsSetting.GetValue();
        rigidbody.Sleep();
    }

}