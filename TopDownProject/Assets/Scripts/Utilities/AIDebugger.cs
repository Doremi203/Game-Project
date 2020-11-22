using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDebugger : MonoBehaviour
{

    private BaseAI target;

    private BaseAI FindTarget()
    {
        BaseAI potentialTarget = null;

        foreach (var item in FindObjectsOfType<BaseAI>())
        {
            if (!potentialTarget)
                potentialTarget = item;
            else
            {
                Vector3 playerPosition = Player.Instance.transform.position;
                if(Vector3.Distance(playerPosition, potentialTarget.transform.position) 
                    > Vector3.Distance(playerPosition, item.transform.position))
                {
                    potentialTarget = item;
                }
            }
        }

        return potentialTarget;
    }

    private void Start() => target = FindTarget();

    private void OnGUI()
    {
        if (!target) return;
        GUILayout.Label(target.transform.name);
        target.StateMachine.DisplayDebugInformation();
    }

}