using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{

    [SerializeField] private GameObject go;
    [SerializeField] private KeyCodeParameter restartBinding;

    private void OnDisable()
    {
        Player.Instance.OnDeath.RemoveListener(PlayerDied);
    }

    private void Start()
    {
        Player.Instance.OnDeath.AddListener(PlayerDied);
        go.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(restartBinding.GetValue())) RestartLevel();
    }

    private void RestartLevel()
    {
        LevelManager.RestartLevel();
    }

    private void PlayerDied()
    {
        go.SetActive(true);
    }

}