using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosableWnd : ClosableWndBase
{
	private ClosableWndTitlebar _ClosableWndTitlebar;

	public ClosableWndTitlebar closableWndTitlebar => _ClosableWndTitlebar;

	protected virtual void Awake()
	{
		_ClosableWndTitlebar = transform.GetComponentInChildren<ClosableWndTitlebar>();

		// 닫기 버튼이 눌린 경우 이 창을 닫도록 합니다.
		_ClosableWndTitlebar.closeButton.onClick.AddListener(CloseThisWnd);
	}

	public void SetTitleText(string titleText) => _ClosableWndTitlebar.SetTitleText(titleText);


}
