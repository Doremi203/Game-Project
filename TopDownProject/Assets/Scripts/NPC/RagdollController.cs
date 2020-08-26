using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{

    private void Awake()
    {
        EnableRagdoll(false);
    }

    public void StartRagdolling()
    {
        EnableRagdoll(true);
    }

    private void EnableRagdoll(bool enable)
    {
        foreach (Rigidbody item in GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = !enable;
            Collider collider = item.GetComponent<Collider>();
            if (collider) collider.enabled = enable;
        }
    }

}