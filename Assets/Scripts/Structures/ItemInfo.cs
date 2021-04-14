using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템의 정보를 나타내기 위한 구조체

[System.Serializable]
public struct ItemInfo
{
	// 아이템 코드
	public string itemCode;

	// 아이템 타입
	public ItemType itemType;

	// 아이템 이름
	public string itemName;

	// 아이템 설명
	public string itemDescription;

	// 아이템 이미지 경로
	public string itemImagePath;

	// 이 아이템이 슬롯에 몇 개 들어갈 수 있는지를 나타냅니다.
	public int maxSlotCount;

	// 아이템 판매 가격
	public int price;

	public bool isEmpty => string.IsNullOrEmpty(itemCode);

	public ItemInfo(string itemCode, ItemType itemType, string itemName,
		string itemDescription, string itemImagePath, int maxSlotCount, int price)
	{
		this.itemCode			= itemCode;
		this.itemType			= itemType;
		this.itemName			= itemName;
		this.itemDescription	= itemDescription;
		this.itemImagePath		= itemImagePath;
		this.maxSlotCount		= maxSlotCount;
		this.price				= price;
	}


}
