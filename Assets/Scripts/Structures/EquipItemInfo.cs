using UnityEngine;

// 하나의 장비 아이템 정보를 나타냅니다.
[System.Serializable]
public struct EquipItemInfo
{
	// 아이템 코드
	public string itemCode;

	// 장착 최소 레벨
	public int minimumEquipLevel;

	// 장착 가능 부위
	public PartsType partsType;

	// 로컬 위치
	public Vector3 localPosition;

	// 로컬 회전
	public Vector3 localEulerAngle;

	// 장착 시 머리 숨김 여부
	public bool hideHairWhenEquipped;

	// 장착 시 하프 헤어 사용 여부
	public bool useHalfHair;

	// Mesh Prefab 경로
	public string meshPrefabPath;

	// Meterial 경로
	public string meterialPath;

	// 해당 부위가 헤어일 경우 하프 헤어 MeshPrefab 경로
	public string halfHairPrefabPath;



	// 추가 물리 저항력
	public float additionalArmor;

	// 추가 마법 저항력
	public float additionalMagicResistance;

	// 추가 물리 공격력
	public float additionalPhysicalDamage;

	// 추가 마법 공격력
	public float additionalMagicalDamage;

	// 추가 이동 속도
	public float additionalMoveSpeed;

	// 추가 점프 높이
	public float additionalJumpHeight;

	// 추가 HP
	public float additionalHp;

	// 추가 MP
	public float additionalMp;

	// 추가 치명타 확률
	public float additionalCriticalPercentage;

	public bool IsEmpty() => string.IsNullOrEmpty(itemCode);
}
