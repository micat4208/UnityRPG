

public struct QuickSlotInfo
{
	// 퀵슬롯에 연결된 슬롯의 타입을 나타냅니다.
	public SlotType linkedSlotType;

	// 인벤토리 슬롯인 경우 슬롯 인덱스를 나타냅니다.
	public int linkedInventorySlotIndex;

	// 슬롯 내부 코드를 나타냅니다.
	/// - 아이템 코드 / 스킬 코드
	public string inCode;

	// 개수를 나타냅니다.
	public int count;

	public QuickSlotInfo(SlotType linkedSlotType, string inCode, int count, int linkedInventorySlotIndex = -1)
	{
		this.linkedSlotType = linkedSlotType;
		this.inCode = inCode;
		this.count = count;
		this.linkedInventorySlotIndex = linkedInventorySlotIndex;
	}
}
