using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ablity : MonoBehaviour
{ 
    public abstract IEnumerator Cast();
    public abstract void CoolDownUpdater();
}