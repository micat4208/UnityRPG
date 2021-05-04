using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Enums;

public sealed class GamePlayerController : PlayerController
{
	static QuickSlotPanel _HUD_QuickSlotPanelPrefab;

	private QuickSlotPanel _HUD_QuickSlotPanel;

	private PlayerInventory _PlayerInventory;
	public PlayerInventory playerInventory => _PlayerInventory;

	protected override void Awake()
	{
		base.Awake();

		if (!_HUD_QuickSlotPanelPrefab)
		{
			_HUD_QuickSlotPanelPrefab = ResourceManager.Instance.LoadResource<GameObject>(
				"HUD_QuickSlotPanel",
				"Prefabs/UI/HUD/QuickSlotPanel/HUD_QuickSlotPanel").GetComponent<QuickSlotPanel>();
		}

		_PlayerInventory = GetComponent<PlayerInventory>();
	}

	private void Start()
	{
		// Floating QuickSlotPanel ... 
		{
			_HUD_QuickSlotPanel = screenInstance.CreateChildHUD(
				_HUD_QuickSlotPanelPrefab, InputMode.GameOnly, false, 0.0f, 100.0f);

			_HUD_QuickSlotPanel.rectTransform.anchoredPosition = Vector2.up * 50.0f;
		}
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
