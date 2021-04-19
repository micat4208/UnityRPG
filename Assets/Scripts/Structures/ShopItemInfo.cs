using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상점 판매 아이템을 나타내기 위한 구조체입니다.
[System.Serializable]
public struct ShopItemInfo
{
	// 판매 아이템 코드
	public string itemCode;

	// 판매 가격
	public int price;
}
