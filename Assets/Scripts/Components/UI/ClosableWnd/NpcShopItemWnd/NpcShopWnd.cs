using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class NpcShopWnd : ClosableWnd
{
	[SerializeField] private RectTransform _Panel_ShopItems;

	private static ShopItem _Button_ShopItemPrefab;

	// 상점 창 정보를 나타냅니다.
	private ShopInfo _ShopInfo;

	protected override void Awake()
	{
		base.Awake();

		if (!_Button_ShopItemPrefab)
		{
			_Button_ShopItemPrefab = ResourceManager.Instance.LoadResource<GameObject>(
				"_Button_ShopItem",
				"Prefabs/UI/ClosableWnd/NpcShopWnd/Button_ShopItem").GetComponent<ShopItem>();
		}
	}

	// Npc 상점 창을 초기화합니다.
	public void InitializeNpcShop(ShopInfo shopInfo)
	{
		// 상점 정보 설정
		_ShopInfo = shopInfo;

		// 타이틀 설정
		SetTitleText(shopInfo.shopName);

		// 판매하는 아이템 추가
		foreach(var shopItemInfo in _ShopInfo.shopItems)
		{
			ShopItem newShopItem = Instantiate(_Button_ShopItemPrefab, _Panel_ShopItems);
		}
	}

}
