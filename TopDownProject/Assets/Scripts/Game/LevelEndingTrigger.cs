using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LevelEndingTrigger : MonoBehaviour
{

    private void Awake() => this.GetComponent<BoxCollider>().isTrigger = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>()) LevelManager.FinishScene();
    }

    private void OnDrawGizmos()
    {
        BoxCollider _boxCollider = this.GetComponent<BoxCollider>();
        Vector3 _size = Vector3.Scale(_boxCollider.size, transform.localScale);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, _size);
        Gizmos.color = new Color(0, 1, 0, 0.25f);
        Gizmos.DrawCube(transform.position, _size);
    }

}