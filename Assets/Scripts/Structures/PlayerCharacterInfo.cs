using System.Collections.Generic;

[System.Serializable]
public struct PlayerCharacterInfo
{
	// 인벤토리 슬롯 개수
	public int inventorySlotCount;

	// 소지중인 아이템 정보
	public List<ItemSlotInfo> inventoryItemInfos;


	public void Initialize()
	{
		inventorySlotCount = 50;

		inventoryItemInfos = new List<ItemSlotInfo>();
		for (int i = 0; i < inventorySlotCount; ++i)
			inventoryItemInfos.Add(new ItemSlotInfo());

		inventoryItemInfos[3] = new ItemSlotInfo("90002", 3, 10);
		inventoryItemInfos[6] = new ItemSlotInfo("90004", 4, 10);
		inventoryItemInfos[9] = new ItemSlotInfo("90000", 5, 10);
		inventoryItemInfos[12] = new ItemSlotInfo("90005", 6, 10);


	}



}