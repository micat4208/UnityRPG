using System.Collections.Generic;

[System.Serializable]
public struct PlayerCharacterInfo
{
	// 현재 장착중인 아이템 정보
	public List<EquipItemInfo> partsInfo;

	// 인벤토리 슬롯 개수
	public int inventorySlotCount;

	// 소지중인 아이템 정보
	public List<ItemSlotInfo> inventoryItemInfos;

	// 소지금
	public int silver;


	public void Initialize()
	{
		inventorySlotCount = 50;
		silver = 10000;

		inventoryItemInfos = new List<ItemSlotInfo>();
		for (int i = 0; i < inventorySlotCount; ++i)
			inventoryItemInfos.Add(new ItemSlotInfo());

		inventoryItemInfos[3] = new ItemSlotInfo("90002", 3, 10);
		inventoryItemInfos[6] = new ItemSlotInfo("90004", 4, 10);
		inventoryItemInfos[9] = new ItemSlotInfo("90000", 5, 10);
		inventoryItemInfos[12] = new ItemSlotInfo("90005", 6, 10);



		partsInfo = new List<EquipItemInfo>();

		bool fileNotFound;
		partsInfo.Add(ResourceManager.Instance.LoadJson<EquipItemInfo>("EquipItemInfos", $"000200.json", out fileNotFound)); // 몸통
		partsInfo.Add(ResourceManager.Instance.LoadJson<EquipItemInfo>("EquipItemInfos", $"000001.json", out fileNotFound)); // 머리

		partsInfo.Add(ResourceManager.Instance.LoadJson<EquipItemInfo>("EquipItemInfos", $"000020.json", out fileNotFound)); // 얼굴
		partsInfo.Add(ResourceManager.Instance.LoadJson<EquipItemInfo>("EquipItemInfos", $"000040.json", out fileNotFound)); // 머리카락

		partsInfo.Add(ResourceManager.Instance.LoadJson<EquipItemInfo>("EquipItemInfos", $"001200.json", out fileNotFound)); // 모자
		partsInfo.Add(ResourceManager.Instance.LoadJson<EquipItemInfo>("EquipItemInfos", $"003200.json", out fileNotFound)); // 주무기
		partsInfo.Add(ResourceManager.Instance.LoadJson<EquipItemInfo>("EquipItemInfos", $"004000.json", out fileNotFound)); // 방패

		partsInfo.Add(ResourceManager.Instance.LoadJson<EquipItemInfo>("EquipItemInfos", $"002000.json", out fileNotFound)); // 가방

	}



}