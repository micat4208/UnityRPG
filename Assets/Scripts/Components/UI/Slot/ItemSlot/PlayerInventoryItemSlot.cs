using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInventoryItemSlot : ItemSlot
{
	// 인벤토리 아이템 슬롯 인덱스
	private int _ItemSlotIndex;

	public int itemSlotIndex => _ItemSlotIndex;

	// 인벤토리 아이템 슬롯을 초기화합니다.
	public void InitializeInventoryItemSlot(int itemSlotIndex)
	{
		_ItemSlotIndex = itemSlotIndex;
	}

}
