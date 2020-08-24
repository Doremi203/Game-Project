using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndingTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>()) GameManager.FinishScene();
    }

}