using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 퀵슬롯 패널을 나타내기 위한 컴포넌트
public sealed class QuickSlotPanel : MonoBehaviour
{
	[SerializeField] private int _QuickSlotCount = 5;

	static QuickSlot _Panel_QuickSlotPrefab;

	public RectTransform rectTransform => transform as RectTransform;

	private void Awake()
	{
		if (!_Panel_QuickSlotPrefab)
		{
			_Panel_QuickSlotPrefab = ResourceManager.Instance.LoadResource<GameObject>(
				"Panel_QuickSlot",
				"Prefabs/UI/Slot/Panel_QuickSlot").GetComponent<QuickSlot>();
		}

		// 퀵슬롯 생성
		CreateQuickSlots();
	}

	// 퀵슬롯들을 생성합니다.
	private void CreateQuickSlots()
	{
		for(int i = 0; i < _QuickSlotCount; ++i)
		{
			QuickSlot newQuickSlot = Instantiate(_Panel_QuickSlotPrefab, transform);
			newQuickSlot.InitializeQuickSlot(
				(KeyCode)((int)KeyCode.Alpha1 + i), 
				(i + 1).ToString());
		}
	}

}
