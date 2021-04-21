#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ClosableWnd), true)]
public class ClosableWnd_UpdateSizeButton : Editor
{

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		// 컴포넌트 객체를 얻습니다.
		ClosableWnd closableWndComponent = target as ClosableWnd;

		// 버튼 추가
		if (GUILayout.Button("Update Size"))
		{

			// 타이틀 바 얻기
			ClosableWndTitlebar titleBar = closableWndComponent.GetComponentInChildren<ClosableWndTitlebar>();

			// 컨텐츠 패널 얻기
			RectTransform panel_Content = closableWndComponent.transform.
				Find("Panel_Content").transform as RectTransform;

			// 타이틀 바 크기 설정
			Vector2 titlebarSize = titleBar.rectTransform.sizeDelta;
			titlebarSize.x = closableWndComponent.rectTransform.sizeDelta.x;
			titleBar.rectTransform.sizeDelta = titlebarSize;

			// 컨텐츠 패널 크기 설정
			panel_Content.offsetMin = new Vector2(0.0f, -closableWndComponent.rectTransform.sizeDelta.y);
			panel_Content.offsetMax = new Vector2(0.0f, -40.0f);
		}
	}
}
#endif
