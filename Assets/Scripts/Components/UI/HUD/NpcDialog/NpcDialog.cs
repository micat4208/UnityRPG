using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public sealed class NpcDialog : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _TMP_NpcName;
	[SerializeField] private Image _Image_NpcNameBackgournd;

	[Header("Menu Buttons")]
	[SerializeField] private Button _Button_NextDialog;
	[SerializeField] private Button _Button_Close;

	[Header("Dialog")]
	[SerializeField] private TextMeshProUGUI _TMP_DialogText;

	private Npc _OwnerNpc;
	private ContentSizeFitter _TMP_NpcNameContentSizeFitter;

	// 표시되는 대화 정보를 나타냅니다.
	private NpcDialogInfo _DialogInfos;

	// 현재 몇 번째 대화를 표시하는 지 나타냅니다.
	private int _CurrentDialogIndex;

	// 마지막 대화임을 나타냅니다.
	private bool _IsLastDialog;

	public RectTransform rectTransform => transform as RectTransform;

	// NPC 대화창이 닫힐 때 호출되는 대리자
	public System.Action onDlgClosed;

	private void Awake()
	{
		_TMP_NpcNameContentSizeFitter = _TMP_NpcName.GetComponent<ContentSizeFitter>();

		BindButtonEvents();
	}

	// 버튼 클릭 이벤트를 바인딩합니다.
	private void BindButtonEvents()
	{
		_Button_Close.onClick.AddListener(
			() => 
			{
				onDlgClosed?.Invoke();
				PlayerManager.Instance.playerController.screenInstance.CloseChildHUD(rectTransform);
			});

		_Button_NextDialog.onClick.AddListener(NextDialog);
	}

	// NpcDialog 를 초기화합니다.
	public void InitializeNpcDialog(Npc ownerNpc)
	{
		_OwnerNpc = ownerNpc;

		// Npc 이름 설정
		SetNpcName(_OwnerNpc.npcInfo.npcName);

		// Dialog 초기화
		InitializeDialog();
	}

	// NpcDialog 를 초기 상태로 되돌립니다.
	private void InitializeDialog()
	{
		_IsLastDialog = false;

		// 기본 대화 내용을 표시합니다.
		_DialogInfos = _OwnerNpc.npcInfo.defaultDialogInfo;

		// 대화 순서를 처음으로 되돌립니다.
		_CurrentDialogIndex = 0;

		// 대화 내용 표시
		ShowDialog(_CurrentDialogIndex);
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

	// 지정한 순서의 대화를 표시합니다.
	private void ShowDialog(int newDialogIndex)
	{
		// 대화 텍스트를 설정합니다.
		void SetDialogText(string dlgText)
		{
			_TMP_DialogText.text = dlgText;
			ContentSizeFitter dlgTextSizeFitter = _TMP_DialogText.GetComponent<ContentSizeFitter>();
			dlgTextSizeFitter.SetLayoutHorizontal();
			dlgTextSizeFitter.SetLayoutVertical();

			(_TMP_DialogText.transform.parent as RectTransform).sizeDelta = _TMP_DialogText.rectTransform.sizeDelta;
		}

		// 사용할 수 있는 대화가 존재하지 않는다면
		if (_DialogInfos.dialogText.Count == 0)
		{
#if UNITY_EDITOR
			Debug.LogError("Usable Dialog Count is Zero!");
#endif
			return;
		}

		if (_DialogInfos.dialogText.Count <= newDialogIndex)
		{
			Debug.LogError($"Out Of Range! newDialogIndex is Changed. ({newDialogIndex} -> {_DialogInfos.dialogText.Count - 1})");
			newDialogIndex = _DialogInfos.dialogText.Count - 1;
		}

		// 대화 텍스트 설정
		SetDialogText(_DialogInfos.dialogText[newDialogIndex]);

		// 마지막 대화 설정
		_IsLastDialog = (_DialogInfos.dialogText.Count - 1) == newDialogIndex;

		// 마지막 대화라면 다음 대화 버튼 숨깁니다.
		SetDialogButtonVisibility(!_IsLastDialog);
	}

	// 다음 대화를 표시합니다.
	private void NextDialog()
	{
		if ((_DialogInfos.dialogText.Count - 1) <= _CurrentDialogIndex)
			return;

		ShowDialog(++_CurrentDialogIndex);
	}

	// 다음 대화 버튼의 가시성을 설정합니다.
	private void SetDialogButtonVisibility(bool bVisible) =>
		_Button_NextDialog.gameObject.SetActive(bVisible);

}
