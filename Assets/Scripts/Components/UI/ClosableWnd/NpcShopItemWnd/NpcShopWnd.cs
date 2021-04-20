using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class NpcShopWnd : ClosableWnd
{
	[SerializeField] private RectTransform _Panel_ShopItems;

	private static ShopItem _Button_ShopItemPrefab;
	private static TradeWnd _Wnd_TradePrefab;

	// 상점 창 정보를 나타냅니다.
	private ShopInfo _ShopInfo;

	// 교환 창 객체를 나타냅니다.
	private TradeWnd _Wnd_Trade;

	protected override void Awake()
	{
		base.Awake();

		if (!_Button_ShopItemPrefab)
		{
			_Button_ShopItemPrefab = ResourceManager.Instance.LoadResource<GameObject>(
				"_Button_ShopItem",
				"Prefabs/UI/ClosableWnd/NpcShopWnd/Button_ShopItem").GetComponent<ShopItem>();
		}
		if (!_Wnd_TradePrefab)
		{
			_Wnd_TradePrefab = ResourceManager.Instance.LoadResource<GameObject>(
				"Wnd_Trade",
				"Prefabs/UI/ClosableWnd/TradeWnd/Wnd_Trade").GetComponent<TradeWnd>();
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
			// 상점에서 판매하는 아이템 목록을 구성합니다.
			ShopItem newShopItem = Instantiate(_Button_ShopItemPrefab, _Panel_ShopItems);

			// 판매하는 아이템 UI 를 초기화합니다.
			newShopItem.InitializeShopItem(this, shopItemInfo);
		}
	}

	// 아이템 교환 창을 전달합니다.
	/// - tradeSeller : 창을 띄운 판매자 타입을 전달합니다.
	/// - connectedItemSlot : 함께 사용되는 아이템 슬롯을 전달합니다.
	/// - shopItemInfo : tradeSeller 가 ShopKeeper 일 경우 판매하는 아이템 정보를 전달합니다.
	/// 
	/// - Return : 생성된 교환 창 객체를 반환합니다.
	///   만약 이미 교환 창이 생성된 상태라면 null 을 반환합니다.
	public TradeWnd CreateTradeWnd(
		TradeSeller tradeSeller, 
		ItemSlot connectedItemSlot, 
		ShopItemInfo? shopItemInfo = null)
	{
		if (_Wnd_Trade) return null;
		if (connectedItemSlot.itemInfo.isEmpty) return null;

		// 교환 창 객체를 생성합니다.
		_Wnd_Trade = PlayerManager.Instance.playerController.screenInstance.CreateWnd(
			_Wnd_TradePrefab, true, UnityStartUpFramework.Enums.InputMode.Default) as TradeWnd;

		// 교환 창 객체를 초기화합니다.
		_Wnd_Trade.InitializeTradeWnd(tradeSeller, connectedItemSlot, shopItemInfo);

		// 교환 창이 닫힐 때 _Wnd_Trade 을 null 로 설정합니다.
		_Wnd_Trade.onWndClosedEvent += () => _Wnd_Trade = null;

		// 생성된 교환 창 객체를 반환합니다.
		return _Wnd_Trade;
	}
}
