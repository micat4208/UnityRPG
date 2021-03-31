using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerCharacterMovement))]
public sealed class PlayerableCharacter : PlayerableCharacterBase
{
	[SerializeField] private SpringArm _SpringArm;

	public CharacterController characterController { get; private set; }
	public PlayerCharacterMovement movement { get; private set; }
	public SpringArm springArm => _SpringArm;

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		movement = GetComponent<PlayerCharacterMovement>();

		idCollider = characterController;
	}

	protected override void Update()
	{
		void InputKey()
		{
			playerController.AddPitchAngle(-InputManager.GetAxis("Mouse Y"));
			playerController.AddYawAngle(InputManager.GetAxis("Mouse X"));
			springArm.ZoomCamera(-InputManager.GetAxis("Mouse ScrollWheel"));
		}

		base.Update();
		InputKey();
	}
}
