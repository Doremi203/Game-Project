using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashController : MonoBehaviour
{

    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private float liveTime = 0.05f;

    private float deathTime;

    public void Show()
    {
        deathTime = Time.time + liveTime;
        muzzleFlash.SetActive(true);
    }

    private void Start()
    {
        muzzleFlash.SetActive(false);
    }

    private void Update()
    {
        if(Time.time >= deathTime) muzzleFlash.SetActive(false);
    }

}