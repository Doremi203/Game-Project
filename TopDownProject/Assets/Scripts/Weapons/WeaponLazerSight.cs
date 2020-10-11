using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLazerSight : WeaponComponent
{

    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private Vector3 startPointOffset;
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private LayerMask layerMask;

    private LineRenderer currentLine;

    public override void OnPickedUp() => SpawnLine();

    public override void OnDropped() => Destroy(currentLine.gameObject);

    private void Start()
    {
        if (weapon.State == WeaponState.Drop) return;
        SpawnLine();
    }

    private void Update()
    {
        if (!currentLine) return;

        Ray ray = new Ray(weapon.Owner.eyesPosition, weapon.Owner.transform.forward);
        RaycastHit hit;

        Vector3 origin = this.transform.position + this.transform.TransformDirection(startPointOffset);
        Vector3 targetPoint = weapon.Owner.eyesPosition + weapon.Owner.transform.forward * 100f;

        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            currentLine.gameObject.SetActive(hit.distance >= minDistance);
            currentLine.SetPosition(0, origin);
            currentLine.SetPosition(1, hit.point);
        }
        else
        {
            currentLine.SetPosition(0, origin);
            currentLine.SetPosition(1, targetPoint);
        }
    }

    private void SpawnLine() => currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = this.transform.position + this.transform.TransformDirection(startPointOffset);
        Gizmos.DrawSphere(origin, 0.1f);
    }

}