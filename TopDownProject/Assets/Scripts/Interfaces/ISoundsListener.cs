using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundsListener 
{

    void ApplySoundEvent(Actor causer, Vector3 eventPosition);
   
}