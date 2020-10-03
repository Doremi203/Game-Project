using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.HighDefinition;

public class BloodyFootprints : MonoBehaviour
{

    [SerializeField] private DecalProjector footprintDecalPrefab;
    [SerializeField] private float cooldown;
    [SerializeField] private LayerMask layerMask;

    private NavMeshAgent agent;
    private CharacterController characterController;
    private float timeToNextFootprint;
    private int currentFootprint;
    private float bloodAmount;

    public void SetBloodAmount(float blood) => bloodAmount = blood;

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        if (agent) return;
        characterController = this.GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (bloodAmount > 0)
            bloodAmount -= Time.deltaTime;
        else
            bloodAmount = 0;

        bool isMoving = false;

        if(agent)
            isMoving = agent.velocity.magnitude >= 0.2f;
        else
            isMoving = characterController.velocity.magnitude >= 0.2f;

        if (Time.time >= timeToNextFootprint && bloodAmount > 0 && isMoving) CreateFootprint();
    }

    private void CreateFootprint()
    {
        currentFootprint++;
        timeToNextFootprint = Time.time + cooldown;
        Ray _ray = new Ray(transform.position, Vector3.up * -1);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, 2, layerMask))
        {
            Quaternion _rotation = Quaternion.LookRotation(transform.forward);
            _rotation = Quaternion.Euler(90, _rotation.eulerAngles.y, _rotation.eulerAngles.z);
            DecalProjector _footprint = Instantiate(footprintDecalPrefab, _hit.point, _rotation);
            if(currentFootprint % 2 == 0)
            {
                _footprint.uvScale = new Vector2(-1, 1);
                _footprint.transform.position = _footprint.transform.position + _footprint.transform.right * 0.175f;
            }
            else
            {
                _footprint.transform.position = _footprint.transform.position + _footprint.transform.right * -0.175f;
            }
        }
    }

}