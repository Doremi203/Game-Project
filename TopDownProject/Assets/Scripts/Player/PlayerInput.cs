﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(WeaponHolder))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(AbilityShurikens))]
public class PlayerInput : MonoBehaviour
{

	[Header("Movement")]
	[SerializeField] private KeyCodeParameter moveForwardInput;
	[SerializeField] private KeyCodeParameter moveBackwardInput;
	[SerializeField] private KeyCodeParameter moveRightInput;
	[SerializeField] private KeyCodeParameter moveLeftInput;
	[Header("Weapons")]
	[SerializeField] private KeyCodeParameter shoot;
	[SerializeField] private KeyCodeParameter throwShuriken;
	[SerializeField] private KeyCodeParameter pickupWeapon;
	[Header("Camera")]
	[SerializeField] private KeyCodeParameter lookAround;
	[SerializeField] private CinemachineCameraOffset cameraOffset;

	private PlayerController playerController;
	private WeaponHolder weaponHolder;
	private Player player;
	private AbilityShurikens shuriken;

    private void Awake()
    {
		playerController = GetComponent<PlayerController>();
		weaponHolder = GetComponent<WeaponHolder>();
		player = GetComponent<Player>();
		shuriken = GetComponent<AbilityShurikens>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T)) UnityEngine.Application.LoadLevel(0);

		float _targetOffsetX = 0;
		float _targetOffsetY = 0;

		if (Input.GetKey(lookAround.GetValue()))
        {
			_targetOffsetX = (Input.mousePosition.x / Screen.width - 0.5f) * 2 * 5;
			_targetOffsetY = (Input.mousePosition.y / Screen.height - 0.5f) * 2 * 5;
        }

		cameraOffset.m_Offset.x = Mathf.Lerp(cameraOffset.m_Offset.x, _targetOffsetX, 15f * Time.deltaTime);
		cameraOffset.m_Offset.y = Mathf.Lerp(cameraOffset.m_Offset.y, _targetOffsetY, 15f * Time.deltaTime);

		if (player.IsDead) return;

		Vector3 inputVector = new Vector3();

		if (Input.GetKey(moveForwardInput.GetValue())) inputVector.z += 1;
		if (Input.GetKey(moveBackwardInput.GetValue())) inputVector.z -= 1;
		if (Input.GetKey(moveRightInput.GetValue())) inputVector.x += 1;
		if (Input.GetKey(moveLeftInput.GetValue())) inputVector.x -= 1;

		inputVector = inputVector.normalized;

		playerController.SetVelocity(inputVector);

		Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

		if (groundPlane.Raycast(cameraRay, out var rayLenght))
		{
			Vector3 pointToLook = cameraRay.GetPoint(rayLenght);
			Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
			Vector3 direction = pointToLook - player.transform.position;
			player.desiredRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		}

		if (player.weaponHolder.CurrentWeapon)
		{
			if (Input.GetKeyDown(shoot.GetValue())) weaponHolder.CurrentWeapon.Use(true);
			if (Input.GetKeyUp(shoot.GetValue())) weaponHolder.CurrentWeapon.Use(false);
		}

		if (Input.GetKeyDown(pickupWeapon.GetValue())) player.TakeWeapon();

		if (Input.GetKeyDown(throwShuriken.GetValue())) shuriken.Use();

	}

}