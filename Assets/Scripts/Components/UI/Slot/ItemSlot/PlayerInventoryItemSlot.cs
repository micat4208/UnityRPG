using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInventoryItemSlot : ItemSlot
{
	// 인벤토리 아이템 슬롯 인덱스
	private int _ItemSlotIndex;

	public int itemSlotIndex => _ItemSlotIndex;

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

		// 드래그 드랍 사용
		m_UseDragDrop = true;

		// 드래그 시작 시 실행할 내용을 정의
		onSlotBeginDragEvent += (dragDropOperation, dragVisual) =>
		{
			// 드래그 비쥬얼 이미지를 슬롯 아이템 이미지로 설정
			dragVisual.SetDragImageFromSprite(slotImage.sprite);


			// 드래그 취소 시 실행할 내용 정의
			dragDropOperation.onDragCancelled += () =>
				Debug.Log($"[드래그 취소] 드래그 시작 슬롯 인덱스 : {_ItemSlotIndex}");

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
						Debug.Log($"[드래그 성공] 드래그 시작 / 드랍 인덱스 : " +
							$"{_ItemSlotIndex} / {inventoryItemSlot._ItemSlotIndex}");
					}
				}
			};
		};


	}

	// 아이템 개수 텍스트를 갱신합니다.
	private void UpdateItemCountText()
	{
		var playerController = PlayerManager.Instance.playerController as GamePlayerController;
		ItemSlotInfo itemSlotInfo = playerController.playerCharacterInfo.inventoryItemInfos[_ItemSlotIndex];

		SetSlotItemCount(itemSlotInfo.itemCount);
	}

}
