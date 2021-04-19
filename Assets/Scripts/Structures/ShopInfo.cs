using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 상점 정보를 나타내는 구조체
[System.Serializable]
public struct ShopInfo
{
	// 상점 코드를 나타냅니다.
	public string shopCode;

	// 창 제목으로 표시될 상점 이름을 나타냅니다.
	public string shopName;

	// 판매 아이템을 나타냅니다.
	public List<ShopItemInfo> shopItems;
}
