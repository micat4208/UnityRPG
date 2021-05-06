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

	private QuickSlotInfo _QuickSlotInfo;

	public KeyCode hotKey { set => _HotKey = value; }
	public ref QuickSlotInfo quickSlotInfo => ref _QuickSlotInfo;

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

		onSlotDragStarted += (dragDropOp, dragVisual) =>
		{
			// 드래그 이미지를 슬롯 이미지로 설정합니다.
			dragVisual.SetDragImageFromSprite(slotImage.sprite);
		};

		// 드래그 드랍 시 실행할 내용을 정의
		onSlotDragFinished += (DragDropOperation dragDropOp) =>
		{
			// 퀵슬롯을 드래그 드랍 시켰을 때
			if (dragDropOp.originatedComponent == this)
			{
				// 슬롯이 비어있다면 실행 X
				if (string.IsNullOrEmpty(_QuickSlotInfo.inCode))
					return;

				foreach(var overlappedComponent in dragDropOp.overlappedComponents)
				{
					BaseSlot otherSlot = overlappedComponent as BaseSlot;

					if (otherSlot == null) continue;
					if (otherSlot.slotType == SlotType.QuickSlot)
					{
						QuickSlot otherQuickSlot = dragDropOp.overlappedComponents[0] as QuickSlot;

						SwapQuickSlot(this, otherQuickSlot);
					}
				}

			}
			// 다른 슬롯이 드래그 드랍 되었을 때
			else
			{
				BaseSlot linkedSlot = (dragDropOp.originatedComponent as BaseSlot);
				_QuickSlotInfo.linkedSlotType = linkedSlot.slotType;

				// 슬롯의 타입을 확인합니다.
				// 인벤토리 슬롯인 경우
				if (_QuickSlotInfo.linkedSlotType == SlotType.InventorySlot)
				{
					PlayerInventoryItemSlot inventoryItemSlot = linkedSlot as PlayerInventoryItemSlot;

					_QuickSlotInfo.inCode = inventoryItemSlot.itemInfo.itemCode;
					_QuickSlotInfo.linkedInventorySlotIndex = inventoryItemSlot.itemSlotIndex;
				}

				// 퀵슬롯 내용 갱신
				UpdateQuickSlot(linkedSlot);
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

	private void SwapQuickSlot(QuickSlot first, QuickSlot second)
	{
		Sprite firstSlotSprite = first.slotImage.sprite;
		Sprite secondSlotSprite = second.slotImage.sprite;

		QuickSlotInfo temp = first.quickSlotInfo;

		first._QuickSlotInfo = second._QuickSlotInfo;
		second._QuickSlotInfo = temp;

		first.slotImage.sprite = secondSlotSprite;
		second.slotImage.sprite = firstSlotSprite;

		first.SetSlotItemCount(first._QuickSlotInfo.count);
		second.SetSlotItemCount(second._QuickSlotInfo.count);
	}

	// 퀵슬롯 내용을 갱신합니다.
	public void UpdateQuickSlot(BaseSlot linkedSlot)
	{
		GamePlayerController gamePlayerController = 
			PlayerManager.Instance.playerController as GamePlayerController;

		switch (_QuickSlotInfo.linkedSlotType)
		{
		case SlotType.InventorySlot:
			{
				bool fileNotFound;
				ItemSlotInfo slotInfo = gamePlayerController.playerCharacterInfo.inventoryItemInfos[_QuickSlotInfo.linkedInventorySlotIndex];
				ItemInfo iteminfo = ResourceManager.Instance.LoadJson<ItemInfo>(
					"ItemInfos", slotInfo.itemCode + ".json", out fileNotFound);

				_QuickSlotInfo.count = slotInfo.itemCount;


				Texture2D itemImage = ResourceManager.Instance.LoadResource<Texture2D>("", iteminfo.itemImagePath, false);
				Rect rect = new Rect(0.0f, 0.0f, itemImage.width, itemImage.height);
				Sprite itemSprite = Sprite.Create(itemImage, rect, Vector2.one * 0.5f);

				// 슬롯 이미지 갱신
				slotImage.sprite = itemSprite;

				// 아이템 개수 텍스트 갱신
				SetSlotItemCount(_QuickSlotInfo.count);
			}
		break;
		}
	}


}
