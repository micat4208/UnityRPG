using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 슬롯 정보를 나타내기 위한 구조체
[System.Serializable]
public struct ItemSlotInfo
{
	// 아이템 코드
	public string itemCode;

	// 아이템 개수
	public int itemCount;

	// 슬롯에 저장될 수 있는 최대 개수
	public int maxSlotCount;

	public ItemSlotInfo(string itemCode, int itemCount = 0, int maxSlotCount = 0)
	{
		this.itemCode = itemCode;
		this.itemCount = itemCount;
		this.maxSlotCount = maxSlotCount;
	}

	// 같은 아이템 정보인지 확인합니다.
	public static bool operator==(ItemSlotInfo thisItemSlotInfo, ItemSlotInfo itemSlotInfo) =>
		thisItemSlotInfo.itemCode == itemSlotInfo.itemCode;

	// 다른 아이템 정보인지 확인합니다.
	public static bool operator!=(ItemSlotInfo thisItemSlotInfo, ItemSlotInfo itemSlotInfo) =>
		thisItemSlotInfo.itemCode != itemSlotInfo.itemCode;

	// 해당 데이터가 비어있음을 나타냅니다.
	public bool IsEmpty() => string.IsNullOrEmpty(itemCode);

	// 해당 데이터를 비웁니다.
	public void Clear()
	{
		itemCode = null;
		itemCount = maxSlotCount = 0;
	}





}
