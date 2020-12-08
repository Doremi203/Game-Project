using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{

    [SerializeField] private GameObject go;
    [SerializeField] private KeyCodeParameter restartBinding;

    private bool playerDied;

    private void Start()
    {
        Player.Instance.Actor.HealthComponent.Died += PlayerDied;
        go.SetActive(false);
    }

    private void OnDisable()
    {
        Player.Instance.Actor.HealthComponent.Died -= PlayerDied;
    }

    private void Update()
    {
        if (!playerDied) return;
        if (Input.GetKeyDown(restartBinding.GetValue())) RestartLevel();
    }

    private void RestartLevel() => LevelManager.RestartLevel();

    private void PlayerDied(DamageInfo info)
    {
        go.SetActive(true);
        playerDied = true;
    }

}