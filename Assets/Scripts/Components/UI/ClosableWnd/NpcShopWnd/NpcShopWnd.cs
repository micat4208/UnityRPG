using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class NpcShopWnd : ClosableWnd
{
	[SerializeField] private RectTransform _Panel_ShopItems;
	[SerializeField] private Button _Button_OpenInventory;

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

		// 버튼 이벤트 설정
		_Button_OpenInventory.onClick.AddListener(FloatingInventory);
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

		// 인벤토리 창을 띄웁니다.
		FloatingInventory();
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

	// 인벤토리 창을 상점 창 우측에 띄웁니다.
	public void FloatingInventory()
	{
		GamePlayerController gamePlayerController = (PlayerManager.Instance.playerController as GamePlayerController);

		// 인벤토리 창을 띄웁니다.
		gamePlayerController.playerInventory.OpenInventoryWnd(usePrevPosition : false);

		// 띄운 인벤토리 창을 얻습니다.
		PlayerInventoryWnd inventoryWnd = gamePlayerController.playerInventory.playerInventoryWnd;

		// 상점 창의 절반 사이즈를 얻습니다.
		Vector2 shopWndHalfSize = rectTransform.sizeDelta * 0.5f;

		// 띄운 인벤토리 창의 절반 크기를 얻습니다.
		Vector2 inventoryWndHalfSize = inventoryWnd.rectTransform.sizeDelta * 0.5f;

		// 인벤토리 창의 위치를 계산합니다.
		Vector2 newInventoryWndPosition =
			rectTransform.anchoredPosition + ((shopWndHalfSize + inventoryWndHalfSize) * Vector2.right);

		// 이벤토리 창의 Y 위치를 설정합니다.
		newInventoryWndPosition.y += inventoryWndHalfSize.y - shopWndHalfSize.y;

		// 인벤토리 창의 위치를 설정합니다.
		inventoryWnd.rectTransform.anchoredPosition = newInventoryWndPosition;

		// 인벤토리 아이템 슬롯 우클릭 시 아이템 판매가 이루어질 수 있도록 합니다.
		foreach (var playerInventoryItemSlot in inventoryWnd.itemSlots)
			playerInventoryItemSlot.onSlotRightClicked += () =>
				SaleItem(inventoryWnd, playerInventoryItemSlot);
	}

	// 아이템을 판매합니다.
	public void SaleItem(PlayerInventoryWnd playerInventoryWnd, ItemSlot itemSlot)
	{
		// 교환 창을 생성합니다.
		var tradeWnd = CreateTradeWnd(TradeSeller.Player, itemSlot);
		if (!tradeWnd) return;

		tradeWnd.onTradeButtonClicked += (tradeWindow) =>
		{
			PlayerInventoryItemSlot inventoryItemSlot = itemSlot as PlayerInventoryItemSlot;
			int inputCount = tradeWnd.inputTradeCount;
			int itemPrice = tradeWnd.connectedItemSlot.itemInfo.price * inputCount;

			MessageBoxWnd msgBox = null;

			// 입력 값이 잘못 되었을 경우
			if (tradeWnd.isInputTextEmpty || tradeWnd.inputTradeCount == 0)
			{
				msgBox = (m_ScreenInstance as ScreenInstanceBase).CreateMessageBox(
					titleText:		"입력 확인",
					message:		"입력된 내용이 잘못 되었습니다.",
					useBackground:	true,
					useButton:		MessageBoxButton.Ok);

				msgBox.onOkButtonClicked += (screenInst, mb) => msgBox.CloseThisWnd();

				return;
			}

			// 아이템 판매
			string itemName = tradeWnd.connectedItemSlot.itemInfo.itemName;
			msgBox = (m_ScreenInstance as ScreenInstanceBase).CreateMessageBox(
					titleText:		"아이템 판매 확인",
					message:		$"{itemName} 을(를) {inputCount} 개 판매합니다.",
					useBackground:	true,
					/*useButton:*/	MessageBoxButton.Ok, MessageBoxButton.Cancel);

			msgBox.onOkButtonClicked += (screenInst, mb) =>
			{
				GamePlayerController gamePlayerController = (PlayerManager.Instance.playerController as GamePlayerController);
				ref PlayerCharacterInfo playerInfo = ref gamePlayerController.playerCharacterInfo;

				// 아이템 제거
				gamePlayerController.playerInventory.RemoveItem(inventoryItemSlot.itemSlotIndex, inputCount);

				// 은화 획득
				playerInfo.silver += itemPrice;
				playerInventoryWnd.UpdateSilver();


				// 교환 창 닫기
				tradeWnd.CloseThisWnd();

				// 메시지 박스 닫기
				msgBox.CloseThisWnd();
			};

			msgBox.onCancelButtonClicked += (screenInst, mb) => msgBox.CloseThisWnd();
		};


	}
}
