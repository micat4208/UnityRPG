


public enum ScreenEffectType
{ 
	ScreenFadeOut
}

// 아이템 타입을 나타낼 때 사용되는 열거 형식입니다.
public enum ItemType
{
	EtCetera,
	Consumption,
	Equipment
}

public enum TradeSeller
{ 
	ShopKeeper,
	Player
}

// 메시지 박스에 사용되는 버튼을 나타내기 위한 열거 형식
public enum MessageBoxButton : byte
{
	Ok     = 0b0001, 
	Cancel = 0b0010
}

// 슬롯의 타입을 나타내기 위해 사용되는 열거 형식입니다.
public enum SlotType
{ 
	ShopItemSlot,
	InventorySlot
}

// 장비 아이템 장착 부위를 나타내기 위한 열거 형식입니다.
public enum PartsType
{ 
	Face,
	Hair,
	Head,
	Body,

	Helmet,
	Backpack,

	LeftWeapon,
	RightWeapon
}



