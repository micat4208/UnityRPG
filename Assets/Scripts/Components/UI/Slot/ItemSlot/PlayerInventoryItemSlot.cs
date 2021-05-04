using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInventoryItemSlot : ItemSlot
{
	// 인벤토리 아이템 슬롯 인덱스
	private int _ItemSlotIndex;

	public int itemSlotIndex => _ItemSlotIndex;

	protected override void Awake()
	{
		base.Awake();

		// 슬롯 타입 설정
		m_SlotType = SlotType.InventorySlot;

		// 드래그 드랍 사용
		m_UseDragDrop = true;

		// 드래그 시작 시 실행할 내용을 정의
		onSlotBeginDragEvent += (dragDropOperation, dragVisual) =>
		{
			// 슬롯에 담긴 아이템의 이미지를 어둡게 표시합니다.
			if (!m_ItemInfo.isEmpty)
			{
				Color slotImageColor = new Color(0.3f, 0.3f, 0.3f);
				slotImage.color = slotImageColor;
			}

			// 드래그 비쥬얼 이미지를 슬롯 아이템 이미지로 설정
			dragVisual.SetDragImageFromSprite(slotImage.sprite);



			// 드래그 취소 시 실행할 내용 정의
			dragDropOperation.onDragCancelled += () =>
						slotImage.color = new Color(1.0f, 1.0f, 1.0f);

			// 드래그 성공 시 실행할 내용 정의
			dragDropOperation.onDragCompleted += () =>
			{
				// 모든 겹친 UI 에 추가된 컴포넌트를 확인합니다.
				foreach (var overlappedComponent in dragDropOperation.overlappedComponents)
				{
					// 겹친 UI 컴포넌트중 PlayerInventoryItemSlot 을 얻습니다.
					PlayerInventoryItemSlot inventoryItemSlot = overlappedComponent as PlayerInventoryItemSlot;

					// PlayerInventoryItemSlot 형태의 컴포넌트를 얻었다면
					if (inventoryItemSlot != null)
					{
						// 아이템 슬롯이 비어있다면 스왑이 이루어지지 않도록 합니다.
						if (m_ItemInfo.isEmpty) continue;

						slotImage.color = new Color(1.0f, 1.0f, 1.0f);

						GamePlayerController playerController = (PlayerManager.Instance.playerController as GamePlayerController);
						ref PlayerCharacterInfo playerInfo = ref playerController.playerCharacterInfo;

						// 드래그를 시작시킨 슬롯과 드랍을 시킨 위치의 슬롯에 담긴 아이템이 동일한 아이템을 담고 있는지를 나타냅니다.
						bool isSameItem =
							playerInfo.inventoryItemInfos[inventoryItemSlot.itemSlotIndex] ==
							playerInfo.inventoryItemInfos[itemSlotIndex];

						// 동일한 아이템이라면 아이템을 서로 합칩니다.
						if (isSameItem)
							playerController.playerInventory.MergeItem(this, inventoryItemSlot);
						// 아이템 스왑
						else playerController.playerInventory.SwapItem(this, inventoryItemSlot);
					}
				}
			};
		};
	}

	// 인벤토리 아이템 슬롯을 초기화합니다.
	public void InitializeInventoryItemSlot(
		SlotType slotType, 
		string itemCode, 
		int itemSlotIndex)
	{
		// 슬롯 초기화
		base.InitializeSlot(slotType, itemCode);

		// 인덱스 설정
		_ItemSlotIndex = itemSlotIndex;

		// 아이템 개수 텍스트 갱신
		UpdateItemCountText();


	}

	// 아이템 개수 텍스트를 갱신합니다.
	private void UpdateItemCountText()
	{
		var playerController = PlayerManager.Instance.playerController as GamePlayerController;
		ItemSlotInfo itemSlotInfo = playerController.playerCharacterInfo.inventoryItemInfos[_ItemSlotIndex];

		SetSlotItemCount(itemSlotInfo.itemCount);
	}

	// 인벤토리 아이템 슬롯을 갱신합니다.
	public void UpdateInventoryItemSlot()
	{
		// 아이템 이미지 갱신
		UpdateItemImage();

		// 아이탬 개수 텍스트 갱신
		UpdateItemCountText();
	}

}
