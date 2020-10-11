using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SkyboxCamera : MonoBehaviour
{

    [SerializeField] private float skyboxScale;
    [SerializeField] private Vector3 skyboxOffset;

    private Camera mainCamera;
    private Camera skyboxCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        skyboxCamera = this.GetComponent<Camera>();
    }

    private void Update()
    {
        Vector3 position = new Vector3(mainCamera.transform.position.x * skyboxScale, mainCamera.transform.position.y * skyboxScale, mainCamera.transform.position.z * skyboxScale);
        position += skyboxOffset;
        skyboxCamera.transform.position = position;
        skyboxCamera.transform.rotation = mainCamera.transform.rotation;
    }

}