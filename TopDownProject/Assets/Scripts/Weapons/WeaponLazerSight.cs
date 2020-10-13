using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLazerSight : WeaponComponent
{

    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private GameObject lazerDotPrefab;
    [SerializeField] private Vector3 startPointOffset;
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private LayerMask layerMask;

    private LineRenderer currentLine;
    private GameObject currentDot;

    public override void OnPickedUp()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        currentDot = Instantiate(lazerDotPrefab, Vector3.zero, Quaternion.identity);
    }

    public override void OnDropped()
    {
        Destroy(currentLine.gameObject);
        Destroy(currentDot.gameObject);
    }

    private void OnDestroy()
    {
        if(currentLine) Destroy(currentLine.gameObject);
        if (currentDot) Destroy(currentDot.gameObject);
    }

    private void Update()
    {
        if (weapon.State == WeaponState.Drop) return;

        Ray ray = new Ray(weapon.Owner.eyesPosition, weapon.Owner.transform.forward);
        RaycastHit hit;

        Vector3 origin = this.transform.position + this.transform.TransformDirection(startPointOffset);
        Vector3 targetPoint = weapon.Owner.eyesPosition + weapon.Owner.transform.forward * 100f;

        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            currentLine.gameObject.SetActive(hit.distance >= minDistance);
            currentLine.SetPosition(0, origin);
            currentLine.SetPosition(1, hit.point);
            currentDot.SetActive(true);
            currentDot.transform.position = hit.point;
        }
        else
        {
            currentLine.SetPosition(0, origin);
            currentLine.SetPosition(1, targetPoint);
            currentDot.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = this.transform.position + this.transform.TransformDirection(startPointOffset);
        Gizmos.DrawSphere(origin, 0.1f);
    }

}