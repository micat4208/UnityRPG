using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public sealed class NpcDialog : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _TMP_NpcName;
	[SerializeField] private Image _Image_NpcNameBackgournd;

	private Npc _OwnerNpc;
	private ContentSizeFitter _TMP_NpcNameContentSizeFitter;

	public RectTransform rectTransform => transform as RectTransform;

	private void Awake()
	{
		_TMP_NpcNameContentSizeFitter = _TMP_NpcName.GetComponent<ContentSizeFitter>();
	}

	// NpcDialog 를 초기화합니다.
	public void InitializeNpcDialog(Npc ownerNpc)
	{
		_OwnerNpc = ownerNpc;

		// Npc 이름 설정
		SetNpcName(_OwnerNpc.npcInfo.npcName);
	}

	// 표시되는 Npc 이름을 변경합니다.
	private void SetNpcName(string npcName)
	{
		_TMP_NpcName.text = npcName;

		// _TMP_NpcNameContentSizeFitter 의 너비를 갱신합니다.
		_TMP_NpcNameContentSizeFitter.SetLayoutHorizontal();

		// 배경 크기가 글자 사이즈와 딱 맞게 적용되지 않고, 좌우에 여백을 줍니다.
		_Image_NpcNameBackgournd.rectTransform.sizeDelta = 
			_TMP_NpcName.rectTransform.sizeDelta + (Vector2.right * 60.0f);

		// 글자를 배경 중앙으로 배치합니다.
		_TMP_NpcName.rectTransform.anchoredPosition += Vector2.right * 30.0f;
	}



}
