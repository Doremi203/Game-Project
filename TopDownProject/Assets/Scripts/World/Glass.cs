using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(NavMeshObstacle))]
public class Glass : MonoBehaviour, IDamageable
{

    [SerializeField] private Material brokenMaterial;
    [SerializeField] private ParticleSystem particleSystemPrefab;

    private bool isDead;

    public void ApplyDamage(Actor damageCauser, float damage, DamageType damageType, Vector3 damageDirection)
    {
        if (isDead) return;
        isDead = true;

        this.GetComponent<MeshRenderer>().sharedMaterial = brokenMaterial;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<NavMeshObstacle>().enabled = false;

        Vector3 _playerPosition = new Vector3(damageCauser.transform.position.x, 0, damageCauser.transform.position.z);
        Vector3 _thisPosition = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        Vector3 _thisForward = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);

        Vector3 _targetDirection = _playerPosition - _thisPosition;
        float angle = Vector3.Angle(_targetDirection, _thisForward);

        Vector3 _rotation = this.transform.eulerAngles;

        if (angle < 90)
            _rotation = _rotation + new Vector3(0, 180, 0);

        Instantiate(particleSystemPrefab, transform.position, Quaternion.Euler(_rotation));
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Player _player = Player.Instance;

        if (_player == null) return;

        Vector3 _playerPosition = new Vector3(_player.transform.position.x, 0, _player.transform.position.z);
        Vector3 _thisPosition = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        Vector3 _thisForward = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);

        Vector3 _targetDirection = _playerPosition - _thisPosition;
        float f = Vector3.Angle(_targetDirection, _thisForward);
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        style.fontSize = 25;
        Handles.Label(transform.position, f.ToString(), style);
    }

#endif

}