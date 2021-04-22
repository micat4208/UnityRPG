using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class PlayerInventoryWnd : ClosableWnd
{
	[SerializeField] private RectTransform _Panel_ItemSlots;
	[SerializeField] private TextMeshProUGUI _TMP_Silver;

	private PlayerInventoryItemSlot _Panel_PlayerInventoryItemSlotPrefab;

	// 추가된 인벤토리 아이템 슬롯들을 저장할 리스트
	private List<PlayerInventoryItemSlot> _ItemSlots = new List<PlayerInventoryItemSlot>();

	public List<PlayerInventoryItemSlot> itemSlots => _ItemSlots;





	protected override void Awake()
	{
		base.Awake();

		_Panel_PlayerInventoryItemSlotPrefab = ResourceManager.Instance.LoadResource<GameObject>(
			"_Panel_PlayerInventoryItemSlot",
			"Prefabs/UI/Slot/Panel_PlayerInventoryItemSlot").GetComponent<PlayerInventoryItemSlot>();

		// 인벤토리 창 초기화
		InitializeInventoryWnd();

		// 은화 갱신
		UpdateSilver();
	}

	// 인벤토리 창을 초기화합니다.
	private void InitializeInventoryWnd()
	{
		PlayerController playerController = (PlayerManager.Instance.playerController as PlayerController);
		ref PlayerCharacterInfo playerCharacterInfo = ref playerController.playerCharacterInfo;

		for (int i = 0; i < playerCharacterInfo.inventorySlotCount; ++i)
		{
			var newItemSlot = CreateItemSlot();

			_ItemSlots.Add(newItemSlot);

			newItemSlot.InitializeInventoryItemSlot(
				SlotType.InventorySlot,
				playerCharacterInfo.inventoryItemInfos[i].itemCode, 
				i);
		}

		// 창이 닫힐 때 드래그 드랍 작업을 끝냅니다.
		onWndClosedEvent += () => m_ScreenInstance.FinishDragDropOperation();
	}

	private PlayerInventoryItemSlot CreateItemSlot() =>
		Instantiate(_Panel_PlayerInventoryItemSlotPrefab, _Panel_ItemSlots);

	// 인벤토리 아이템 슬롯들을 갱신합니다.
	public void UpdateInventoryItemSlots()
	{
		GamePlayerController playerController = PlayerManager.Instance.playerController as GamePlayerController;
		ref PlayerCharacterInfo playerInfo = ref playerController.playerCharacterInfo;

		for (int i = 0; i < playerInfo.inventorySlotCount; ++i)
		{
			_ItemSlots[i].SetItemInfo(playerInfo.inventoryItemInfos[i].itemCode);

			_ItemSlots[i].UpdateInventoryItemSlot();

			_ItemSlots[i].InitializeInventoryItemSlot(
				SlotType.InventorySlot,
				playerInfo.inventoryItemInfos[i].itemCode, i);
		}

	}

	// 소지금을 갱신합니다.
	public void UpdateSilver()
	{
		GamePlayerController playerController = PlayerManager.Instance.playerController as GamePlayerController;
		_TMP_Silver.text = playerController.playerCharacterInfo.silver.ToString();
	}


}
