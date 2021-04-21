using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public sealed class TradeWnd:ClosableWnd
{
	[SerializeField] private TextMeshProUGUI _TMP_ItemCost;
	[SerializeField] private TMP_InputField _InputField_TradeCount;
	[SerializeField] private TextMeshProUGUI _TMP_Result;

	[SerializeField] private Button _Button_Trade;
	[SerializeField] private Button _Button_TradeCancel;

	[SerializeField] private TextMeshProUGUI _TMP_Trade;

	// 교환 버튼이 클릭되었을 경우 발생하는 이벤트입니다.
	public event System.Action<TradeWnd> onTradeButtonClicked;

	// 판매자를 나타냅니다.
	private TradeSeller _TradeSeller;

	// 판매 아이템 정보를 나타냅니다.
	private ShopItemInfo? _ShopItemInfo;

	// 함께 사용된 슬롯 객체를 나타냅니다.
	private ItemSlot _ConnectedItemSlot;

	// 입력된 텍스트가 비어있는지 확인합니다.
	public bool isInputTextEmpty => string.IsNullOrEmpty(_InputField_TradeCount.text);

	// 입력된 텍스트의 내용을 숫자로 얻습니다.
	public int inputTradeCount
	{
		get 
		{
			if (string.IsNullOrEmpty(_InputField_TradeCount.text)) return -1;

			try
			{
				return int.Parse(_InputField_TradeCount.text);
			}
			catch (System.StackOverflowException)
			{ return -1; }
			catch (System.FormatException)
			{ return -1; }
		}
	}


	// 교환 창을 초기화합니다.
	/// - tradeSeller : 창을 띄운 판매자 타입을 전달합니다.
	/// - connectedItemSlot : 함께 사용되는 아이템 슬롯을 전달합니다.
	/// - shopItemInfo : tradeSeller 가 ShopKeeper 일 경우 판매하는 아이템 정보를 전달합니다.
	public void InitializeTradeWnd(TradeSeller tradeSeller,
		ItemSlot connectedItemSlot,
		ShopItemInfo? shopItemInfo = null)
	{
		// 슬롯 연결
		_ConnectedItemSlot = connectedItemSlot;

		// 판매 아이템 정보 설정
		_ShopItemInfo = shopItemInfo;

		// 판매자 설정
		_TradeSeller = tradeSeller;

		// 타이틀 바 내용을 아이템 이름으로 설정
		SetTitleText(_ConnectedItemSlot.itemInfo.itemName);

		// 버튼 텍스트 설정
		_TMP_Trade.text = (_TradeSeller == TradeSeller.ShopKeeper ? "구매" : "판매");

		// 아이템 가격 설정
		switch (_TradeSeller)
		{
		case TradeSeller.ShopKeeper:
		_TMP_ItemCost.text = _ShopItemInfo.Value.price.ToString();
		break;

		case TradeSeller.Player:
		_TMP_ItemCost.text = _ConnectedItemSlot.itemInfo.price.ToString();
		break;
		}


		// _InputField_TradeCount 의 내용이 변경되었을 경우 호출할 메서드 바인딩
		_InputField_TradeCount.onValueChanged.AddListener(OnTradeCountChanged);

		// 버튼 이벤트 설정
		_Button_Trade.onClick.AddListener(() => onTradeButtonClicked?.Invoke(this));
		_Button_TradeCancel.onClick.AddListener(CloseThisWnd);

	}

	// _InputField_TradeCount 의 내용이 변경되었을 경우 호출되는 메서드
	private void OnTradeCountChanged(string text)
	{
		// 입력된 개수
		int tradeCount = inputTradeCount;

		// 입력된 값이 음수라면
		if (tradeCount < 0)
		{
			// 입력된 내용을 비웁니다.
			_InputField_TradeCount.text = "";

			// 총 가격을 0 으로 변경합니다.
			_TMP_Result.text = "0";

			return;
		}

		// GamePlayerController
		GamePlayerController gamePlayerController = (PlayerManager.Instance.playerController as GamePlayerController);

		// 소지금
		int price = 0;

		// 교환 가능한 최대 아이템 개수
		int maxTradeCount = 0;

		switch (_TradeSeller)
		{
		case TradeSeller.ShopKeeper:
			{
				// 소지금을 얻습니다.
				int silver = gamePlayerController.playerCharacterInfo.silver;

				// 아이템 가격 얻습니다.
				price = _ShopItemInfo.Value.price;

				// 최대 구매 가능한 아이템 개수를 최대로 구매 가능한 아이템 개수로 설정합니다.
				maxTradeCount = (silver / price);

				// 장비 아이템이라면 교환 가능 최대 개수를 1 로 설정합니다.
				if (_ConnectedItemSlot.itemInfo.itemType == ItemType.Equipment && maxTradeCount != 0)
					maxTradeCount = 1;

				break;
			}

		case TradeSeller.Player:
			{
				// 판매하려는 아이템 슬롯 인덱스
				int slotIndex = (_ConnectedItemSlot as PlayerInventoryItemSlot).itemSlotIndex;

				// 판매하려는 아이템 슬롯 정보
				ItemSlotInfo slotInfo = gamePlayerController.playerCharacterInfo.inventoryItemInfos[slotIndex];

				// 아이템 가격을 얻습니다.
				price = _ConnectedItemSlot.itemInfo.price;

				// 최대 판매 가능한 아이템 개수를 슬롯의 아이템 개수로 설정합니다.
				maxTradeCount = slotInfo.itemCount;

				break;
			}
		}

		// 최대 교환 개수를 초과했다면 입력된 숫자를 최대 교환 가능 개수로 설정합니다.
		if (tradeCount > maxTradeCount)
			tradeCount = maxTradeCount;

		// 교환 개수를 설정합니다.
		_InputField_TradeCount.text = tradeCount.ToString();

		// 가격을 설정합니다.
		_TMP_Result.text = (tradeCount * price).ToString();
	}
}
