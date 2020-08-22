using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Dash))]
[RequireComponent(typeof(WeaponHolder))]
[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{

	[Header("Movement")]
	[SerializeField] private InputBinding moveForwardInput;
	[SerializeField] private InputBinding moveBackwardInput;
	[SerializeField] private InputBinding moveRightInput;
	[SerializeField] private InputBinding moveLeftInput;
	[Header("Weapons")]
	[SerializeField] private InputBinding shoot;
	[SerializeField] private InputBinding pickupWeapon;
	[SerializeField] private InputBinding equipKatana;
	[SerializeField] private InputBinding equipWeapon;
	[Header("Player")]
	[SerializeField] private InputBinding dash;

	private PlayerController playerController;
	private Dash dashAbility;
	private WeaponHolder weaponHolder;
	private Player player;

    private void Awake()
    {
		playerController = GetComponent<PlayerController>();
		dashAbility = GetComponent<Dash>();
		weaponHolder = GetComponent<WeaponHolder>();
		player = GetComponent<Player>();
	}

	private void Update()
	{

		if (Input.GetKeyDown(KeyCode.R)) UnityEngine.Application.LoadLevel(0);

		if (player.IsDead) return;

		Vector3 inputVector = new Vector3();

		if (Input.GetKey(moveForwardInput.GetKeyCode())) inputVector.z += 1;
		if (Input.GetKey(moveBackwardInput.GetKeyCode())) inputVector.z -= 1;
		if (Input.GetKey(moveRightInput.GetKeyCode())) inputVector.x += 1;
		if (Input.GetKey(moveLeftInput.GetKeyCode())) inputVector.x -= 1;

		inputVector = inputVector.normalized;

		playerController.SetVelocity(inputVector);

		Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

		if (groundPlane.Raycast(cameraRay, out var rayLenght))
		{
			Vector3 pointToLook = cameraRay.GetPoint(rayLenght);
			Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
			//player.transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
			Vector3 direction = pointToLook - player.transform.position;
			player.desiredRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		}

		//if (Input.GetKeyDown(dash.GetKeyCode())) dashAbility.Cast();

		if (player.weaponHolder.currentWeapon)
		{
			if(Input.GetKeyDown(shoot.GetKeyCode())) player.weaponHolder.currentWeapon.Use(true);
			if (Input.GetKeyUp(shoot.GetKeyCode())) player.weaponHolder.currentWeapon.Use(false);
		}
		//if (player.CurrentWeaponSecond) player.CurrentWeaponSecond.Use(Input.GetKey(KeyCode.Mouse1));

		if (Input.GetKeyDown(pickupWeapon.GetKeyCode())) player.TakeWeapon();
		if (Input.GetKeyDown(KeyCode.Q)) player.TakeWeaponLeftArm();

	}

}