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
	}
}
