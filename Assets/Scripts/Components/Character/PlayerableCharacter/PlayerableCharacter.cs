using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Enums;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerCharacterMovement))]
[RequireComponent(typeof(PlayerInteract))]
public sealed class PlayerableCharacter : PlayerableCharacterBase
{

	[SerializeField] private SpringArm _SpringArm;

	public CharacterController characterController { get; private set; }
	public PlayerCharacterMovement movement { get; private set; }
	public PlayerInteract playerInteract { get; private set; }
	public PlayerCharacterAnimController animController { get; private set; }
	public PlayerSkillController skillController { get; private set; }

	public SpringArm springArm => _SpringArm;


	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		movement = GetComponent<PlayerCharacterMovement>();
		playerInteract = GetComponent<PlayerInteract>();
		animController = GetComponent<PlayerCharacterAnimController>();
		skillController = GetComponent<PlayerSkillController>();

		idCollider = characterController;
	}

	protected override void Update()
	{
		void InputKey()
		{
			playerController.AddPitchAngle(-InputManager.GetAxis("Mouse Y"));
			playerController.AddYawAngle(InputManager.GetAxis("Mouse X"));
			springArm.ZoomCamera(-InputManager.GetAxis("Mouse ScrollWheel"));

			if (InputManager.GetAction("Interact", ActionEvent.Down)) 
				playerInteract.TryInteraction();

		}

		base.Update();
		InputKey();
	}
}
