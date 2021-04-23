using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public sealed class ShopItem : MonoBehaviour,
	IPointerClickHandler
{
	[SerializeField] private ItemSlot _ShopItemSlot;
	[SerializeField] private TextMeshProUGUI _TMP_ItemName;
	[SerializeField] private TextMeshProUGUI _TMP_Price;


	// 상점 창 객체를 나타냅니다.
	private NpcShopWnd _NpcShopWnd;

	// 판매하는 아이템 정보를 나타냅니다.
	private ShopItemInfo _ShopItemInfo;


	// 상점 아이템 UI 를 초기화합니다.
	/// - npcShopWnd : 상점 창 객체를 전달합니다.
	/// - shopItemInfo : 판매시킬 아이템 정보를 전달합니다.
	public void InitializeShopItem(NpcShopWnd npcShopWnd, ShopItemInfo shopItemInfo)
	{
		// 상점 창 객체를 저장합니다.
		_NpcShopWnd = npcShopWnd;

		// 판매하는 아이템 정보를 저장합니다.
		_ShopItemInfo = shopItemInfo;

		// 아이템 슬롯 초기화
		_ShopItemSlot.InitializeSlot(SlotType.ShopItemSlot, _ShopItemInfo.itemCode);

		// 판매하는 아이템 이름 설정
		_TMP_ItemName.text = _ShopItemSlot.itemInfo.itemName;

		// 가격 설정
		_TMP_Price.text = _ShopItemInfo.price.ToString();
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		// 우클릭이 이루어졌다면 아이템 구매창을 엽니다.
		if (eventData.button == PointerEventData.InputButton.Right)
			BuyItem();
	}

	// 아이템을 구매합니다.
	private void BuyItem()
	{
		// 교환 창을 생성합니다.
		var tradeWnd = _NpcShopWnd.CreateTradeWnd(TradeSeller.ShopKeeper, _ShopItemSlot, _ShopItemInfo);

		// 만약 교환 창이 열려있거나, 교환하려는 아이템이 비어있다면 실행하지 않습니다.
		if (!tradeWnd) return;

		tradeWnd.onTradeButtonClicked += (wnd) =>
		{
			MessageBoxWnd msgBox = null;

			// 입력 값이 잘못 되었을 경우
			if (wnd.isInputTextEmpty || wnd.inputTradeCount <= 0)
			{
				// 메시지 박스 생성
				msgBox = (wnd.m_ScreenInstance as ScreenInstanceBase).CreateMessageBox(
					titleText: "입력 확인",
					message: "입력된 내용이 잘못 되었습니다.",
					useBackground: true,
					useButton: MessageBoxButton.Ok);

				// Ok 버튼 클릭 이벤트 설정
				msgBox.onOkButtonClicked += (screenInstance, msgBoxWnd) =>
					msgBoxWnd.CloseThisWnd();
				return;
			}

			string itemName = tradeWnd.connectedItemSlot.itemInfo.itemName;

			msgBox = (wnd.m_ScreenInstance as ScreenInstanceBase).CreateMessageBox(
				titleText:		"아이템 구매 확인",
				message:		$"{itemName} 을(를) {tradeWnd.inputTradeCount} 개 구매합니다.",
				useBackground:	true,
				/*useButton:*/	MessageBoxButton.Ok, MessageBoxButton.Cancel);

			msgBox.onOkButtonClicked += (screenInstance, messageBoxWnd) =>
			{
				GamePlayerController gamePlayerController = (PlayerManager.Instance.playerController as GamePlayerController);
				ref PlayerCharacterInfo playerInfo = ref gamePlayerController.playerCharacterInfo;

				bool fileNotFound;
				ItemInfo itemInfo = ResourceManager.Instance.LoadJson<ItemInfo>(
					"ItemInfos", $"{_ShopItemInfo.itemCode}.json", out fileNotFound);

				// 추가할 아이템 슬롯 정보를 생성합니다.
				ItemSlotInfo newItemSlotInfo = new ItemSlotInfo(
					itemCode :		_ShopItemInfo.itemCode,
					itemCount :		tradeWnd.inputTradeCount,
					maxSlotCount :	(itemInfo.itemType == ItemType.Equipment) ? 1 : itemInfo.maxSlotCount);

				var retVal = gamePlayerController.playerInventory.AddItem(newItemSlotInfo);

				// 구매된 아이템 개수
				int addedCount = tradeWnd.inputTradeCount - retVal.resultInfo.itemCount;

				// 인벤토리에 추가할 수 있는 슬롯이 존재하지 않는 경우
				if (!retVal.result && retVal.resultInfo.itemCount == tradeWnd.inputTradeCount)
				{
					MessageBoxWnd failMsgBox = (wnd.m_ScreenInstance as ScreenInstanceBase).CreateMessageBox(
						titleText: "구매 실패",
						message: "인벤토리에 빈 공간이 없습니다.",
						true,
						MessageBoxButton.Ok);

					failMsgBox.onOkButtonClicked += (screenInst, msg) => failMsgBox.CloseThisWnd();

					return;
				}

				// 소지금 감소
				int price = addedCount * tradeWnd.shopitemInfo.Value.price;
				playerInfo.silver -= price;

				// 인벤토리 창 갱신
				gamePlayerController.playerInventory.playerInventoryWnd?.UpdateInventoryItemSlots();
				gamePlayerController.playerInventory.playerInventoryWnd?.UpdateSilver();

				// 교환 창 닫기
				tradeWnd.CloseThisWnd();

				// 메시지 박스 닫기
				msgBox.CloseThisWnd();
			};

			msgBox.onCancelButtonClicked += (screenInstance, messageBoxWnd) => msgBox.CloseThisWnd();
		};
	}
}
