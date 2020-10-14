using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HealthComponent))]
public class DoorTest : MonoBehaviour, IDamageable
{

    [SerializeField] private Transform doorObject;
    [SerializeField] private NavMeshObstacle doorObstacle;
    [SerializeField] private GameObject doorGameObject;
    [SerializeField] private GameObject doorBroken;

    private bool isOpened;
    private float targetRotation;
    private HealthComponent healthComponent;
    private bool isDead;

    public bool ApplyDamage(DamageInfo info)
    {
        if (isDead) return false;
        if (info.DamageType == DamageType.Melee) return false;
        healthComponent.ApplyDamage(info.DamageAmount);
        if (healthComponent.Health <= 0)
            Death();
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        if (isOpened) return;
        Actor actor = other.GetComponent<Actor>();
        if (!actor) return;

        Vector3 _playerPosition = new Vector3(actor.transform.position.x, 0, actor.transform.position.z);
        Vector3 _thisPosition = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        Vector3 _thisForward = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);

        Vector3 _targetDirection = _playerPosition - _thisPosition;
        float angle = Vector3.Angle(_targetDirection, _thisForward);

        if (angle < 90f)
            targetRotation = -100f;
        else
            targetRotation = 100f;

        doorObstacle.carving = true;
        isOpened = true;
    }

    private void Awake() => healthComponent = GetComponent<HealthComponent>();

    private void Start()
    {
        doorGameObject.SetActive(true);
        doorBroken.SetActive(false);
    }

    private void Update()
    {
        if (isDead) return;
        if (!isOpened) return;
        doorObject.localRotation = Quaternion.Lerp(doorObject.localRotation, Quaternion.Euler(new Vector3(0, targetRotation, 0)), 10f * Time.deltaTime);
    }

    private void Death()
    {
        isDead = true;
        doorGameObject.SetActive(false);
        doorBroken.SetActive(true);
    }

}