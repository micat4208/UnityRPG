using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Enums;
using TMPro;

// 퀵슬롯을 나타내기 위한 컴포넌트
public sealed class QuickSlot : BaseSlot
{
	[SerializeField] private TextMeshProUGUI _TMP_HotKey;

	[SerializeField] private KeyCode _HotKey;

	// 퀵슬롯에 연결된 슬롯의 타입을 나타냅니다.
	private SlotType _LinkedSlotType;

	// 인벤토리 슬롯인 경우 슬롯 인덱스를 나타냅니다.
	private int _LinkedInventorySlotIndex = -1;


	public KeyCode hotKey { set => _HotKey = value; }

	public void InitializeQuickSlot(KeyCode newHotKey, string hotKeyText)
	{
		_HotKey = newHotKey;
		_TMP_HotKey.SetText(hotKeyText);
	}

	protected override void Awake()
	{
		base.Awake();

		// 슬롯 타입 설정
		m_SlotType = SlotType.QuickSlot;

		// 드래그 사용
		m_UseDragDrop = true;

		// 드래그 드랍 시 실행할 내용을 정의
		onSlotDragFinished += (DragDropOperation dragDropOp) =>
		{
			// 퀵슬롯을 드래그 드랍 시켰을 때
			if (dragDropOp.originatedComponent == this)
			{

			}
			// 다른 슬롯이 드래그 드랍 되었을 때
			else
			{
				BaseSlot linkedSlot = (dragDropOp.originatedComponent as BaseSlot);
				_LinkedSlotType = linkedSlot.slotType;

				// 슬롯의 타입을 확인합니다.
				// 인벤토리 슬롯인 경우
				if (_LinkedSlotType == SlotType.InventorySlot)
				{
					_LinkedInventorySlotIndex = (linkedSlot as PlayerInventoryItemSlot).itemSlotIndex;
				}

				// 퀵슬롯 내용 갱신
				UpdateQuickSlot();
			}
		};
	}

	private void Update()
	{
		InputHotKey();
	}

	private void InputHotKey()
	{
		if (!InputManager.Instance.IsInputMode(InputMode.GameOnly)) return;
		if (Input.GetKeyDown(_HotKey))
		{
			Debug.Log($"{_HotKey.ToString()} input!");
		}

	}

	// 퀵슬롯 내용을 갱신합니다.
	public void UpdateQuickSlot()
	{
		GamePlayerController gamePlayerController = 
			PlayerManager.Instance.playerController as GamePlayerController;

		switch (_LinkedSlotType)
		{
		case SlotType.InventorySlot:
			{
				bool fileNotFound;
				ItemInfo iteminfo = ResourceManager.Instance.LoadJson<ItemInfo>(
					"Resources/Image/ItemImage",
					gamePlayerController.playerCharacterInfo.inventoryItemInfos[_LinkedInventorySlotIndex].itemCode + ".json", 
					out fileNotFound);

				//iteminfo.itemImagePath

			}
		break;
		case SlotType.QuickSlot:
		break;
		}

	}


}
