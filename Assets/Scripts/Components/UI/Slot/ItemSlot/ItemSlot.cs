using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : BaseSlot
{
	// 아이템 정보를 나타냅니다.
	protected ItemInfo m_ItemInfo;

	public ItemInfo itemInfo => m_ItemInfo;

	public override void InitializeSlot(SlotType slotType, string inCode)
	{
		base.InitializeSlot(slotType, inCode);

		// 아이템 정보 설정
		SetItemInfo(inCode);

		// 아이템 이미지 갱신
		UpdateItemImage();
	}

	// 사용되는 아이템 정보를 설정합니다.
	public void SetItemInfo(string itemCode)
	{
		// 아이템 코드가 비어있다면
		if (string.IsNullOrEmpty(itemCode))
		{
			m_ItemInfo = new ItemInfo();
			return;
		}
		// 아이템 코드가 비어있지 않다면
		else
		{
			// 아이템 정보를 읽어옵니다.
			bool fileNotFound;
			ItemInfo itemInfo = ResourceManager.Instance.LoadJson<ItemInfo>(
				"ItemInfos", $"{itemCode}.json", out fileNotFound);



			// 파일을 읽어오지 못했다면
			if (fileNotFound) m_ItemInfo = new ItemInfo();

			// 파일을 성공적으로 읽어왔다면
			else m_ItemInfo = itemInfo;
		}
	}

	// 아이템 이미지를 갱신합니다.
	protected void UpdateItemImage()
	{
		Texture2D itemImage;

		// 아이템 정보가 비어있다면 투명한 이미지를 사용합니다.
		if (m_ItemInfo.isEmpty) itemImage = m_T_NULL;

		// 아이템 정보가 비어있지 않는 경우
		else
		{
			// 아이템 이미지 경로가 비어있다면 투명한 이미지 사용
			if (string.IsNullOrEmpty(m_ItemInfo.itemImagePath))
				itemImage = m_T_NULL;

			// 아이템 이미지 경로가 비어있지 않을 경우 이미지를 로드합니다.
			else itemImage = ResourceManager.Instance.LoadResource<Texture2D>(
				"", m_ItemInfo.itemImagePath, false);


			// 이미지 로드에 실패한 경우 투명한 이미지를 사용하도록 합니다.
			itemImage = itemImage ?? m_T_NULL;
		}

		// 아이템 이미지 적용
		Rect rect = new Rect(0.0f, 0.0f, itemImage.width, itemImage.height);
		slotImage.sprite = Sprite.Create(itemImage, rect, Vector2.one * 0.5f);
	}
}
