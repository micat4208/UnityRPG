using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerCharacterMovement))]
public sealed class PlayerableCharacter : PlayerableCharacterBase
{
	public CharacterController characterController { get; private set; }
	public PlayerCharacterMovement movement { get; private set; }

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		movement = GetComponent<PlayerCharacterMovement>();

		idCollider = characterController;
	}
}
