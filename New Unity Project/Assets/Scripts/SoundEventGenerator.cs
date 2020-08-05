using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundEventGenerator
{

    public static void GenerateSoundEvent(Actor causer, Vector3 eventPosition, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(eventPosition, radius); ;

        foreach (var item in hits)
        {
            ISoundsListener target = item.GetComponent<ISoundsListener>();
            if (target != null)
            {
                target.Test(causer, eventPosition);
            }
        }
    }

}