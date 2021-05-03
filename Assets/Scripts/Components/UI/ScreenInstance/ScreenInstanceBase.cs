using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Enums;

public class ScreenInstanceBase : ScreenInstance
{
	private static RectTransform _MsgBoxBackgroundPrefab;
	private static MessageBoxWnd _Wnd_MessageBoxPrefab;

	[SerializeField] private ScreenDrawer _ScreenDrawer;

	public ScreenDrawer screenDrawer => _ScreenDrawer;



	protected override void Awake()
	{
		base.Awake();

		if (!_MsgBoxBackgroundPrefab)
		{
			_MsgBoxBackgroundPrefab = ResourceManager.Instance.LoadResource<GameObject>(
				"Panel_MessageBoxBackground",
				"Prefabs/UI/ClosableWnd/MessageBoxWnd/Panel_MsgBoxBackground").transform as RectTransform;
		}
		if (!_Wnd_MessageBoxPrefab)
		{
			_Wnd_MessageBoxPrefab = ResourceManager.Instance.LoadResource<GameObject>(
				"Wnd_MessageBox",
				"Prefabs/UI/ClosableWnd/MessageBoxWnd/Wnd_MesageBox").GetComponent<MessageBoxWnd>();
		}
	}

	// 메시지 박스를 생성합니다.
	/// - titleText : 메시지 박스의 타이틀 문자열을 전달합니다.
	/// - message : 메시지 박스의 내용을 전달합니다.
	/// - useBackground : 배경 사용 여부를 전달합니다.
	/// - useButton : 사용하는 버튼들을 전달합니다.
	public MessageBoxWnd CreateMessageBox(
		string titleText,
		string message,
		bool useBackground,
		params MessageBoxButton[] useButton)
	{
		// 메시지 박스 배경 UI 를 나타낼 변수
		RectTransform msgBoxBackground = null;

		// 메시지 박스 배경을 사용한다면
		if (useBackground)
		{
			// 메시지 박스 배경을 생성합니다.
			msgBoxBackground = Instantiate(_MsgBoxBackgroundPrefab, m_Panel_WndParent);
		}

		// 메시지 박스 생성
		MessageBoxWnd newMessageBox = CreateWnd(_Wnd_MessageBoxPrefab, false, InputMode.UIOnly, true) as MessageBoxWnd;
		
		// 메시지 박스 배경 객체 설정
		newMessageBox.m_MsgBoxBackground = msgBoxBackground;

		// 메시지 박스 초기화
		newMessageBox.InitializeMessageBox(titleText, message, useButton);

		// 생성한 메시지 박스 창 반환
		return newMessageBox;
	}

}
