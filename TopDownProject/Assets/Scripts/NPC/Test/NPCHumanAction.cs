using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPCAction", menuName = "NPCAction")]
public class NPCHumanAction : ScriptableObject
{

    public int idleAnimationID;
    public float exitTime;

    public AnimatorOverrideController animations;

}