using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseSlot : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _TMP_Count;

	protected Button m_Button_Slot;

	protected virtual void Awake()
	{
		m_Button_Slot = GetComponent<Button>();

		SetSlotItemCount(0);
	}

	// 슬롯에 표시되는 숫자를 설정합니다.
	/// - itemCount : 표시시킬 아이템 개수를 전달합니다.
	/// - visibleLessThan2 : 2 개 미만일 경우에도 숫자를 표시할 것인지를 전달합니다.
	public void SetSlotItemCount(int itemCount, bool visibleLessThan2 = false) =>
		_TMP_Count.text = (itemCount >= 2 || visibleLessThan2) ? itemCount.ToString() : "";


}
