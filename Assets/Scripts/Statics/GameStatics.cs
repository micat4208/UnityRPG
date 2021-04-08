
using UnityEngine;

// 게임 제작에 필요한 정적 필드, 정적 메서드를 제공할 클래스
public static class GameStatics
{
	// 화면 크기에 대한 읽기 전용 프로퍼티
	public static (float width, float height) screenSize = (1600.0f, 900.0f);

	// 화면 비율에 대한 읽기 전용 프로퍼티
	public static float screenRatio => Screen.width / screenSize.width;

}