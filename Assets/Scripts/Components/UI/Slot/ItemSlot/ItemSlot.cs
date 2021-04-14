using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : BaseSlot
{
	// 아이템 정보를 나타냅니다.
	protected ItemInfo m_ItemInfo;

	public ItemInfo itemInfo => m_ItemInfo;
}
