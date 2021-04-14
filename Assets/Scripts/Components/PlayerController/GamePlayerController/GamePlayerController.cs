using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GamePlayerController : PlayerController
{
	private PlayerInventory _PlayerInventory;
	public PlayerInventory playerInventory => _PlayerInventory;

	protected override void Awake()
	{
		base.Awake();

		_PlayerInventory = GetComponent<PlayerInventory>();
	}

	private void Update()
	{
		InputKey();
	}

	private void InputKey()
	{
		if (InputManager.GetAction("OpenInventory", UnityStartUpFramework.Enums.ActionEvent.Down))
			_PlayerInventory.ToggleInventoryWnd();
	}

	




}
