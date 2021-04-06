using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public sealed class PathFixer : EditorWindow
// - EditorWindow : 에디터에서 사용할 수 있는 창 객체를 만들기 위한 형식
{
	public string path { get; set; } = "Input Path...";
	string ConvertedPath = "Result...";

	[MenuItem("Window/CustomUtil/PathFixer")]
	private static void Init()
	{
		PathFixer window = EditorWindow.GetWindow(typeof(PathFixer)) as PathFixer;
		window.minSize = new Vector2(20, 100);
	}

	private void OnGUI()
	{
		// 라벨
		GUILayout.Label("Base Path", EditorStyles.boldLabel);

		path = EditorGUILayout.TextField("Input Path", path);

		// Convert 버튼이 눌렸다면
		if (GUILayout.Button("Convert"))
		{
			// 경로를 컨버팅합니다.
			ConvertedPath = ConvertPath(path);
		}

		EditorGUILayout.TextField(ConvertedPath);
	}

	private string ConvertPath(string originalPath)
	{
		int pathStartIndex = originalPath.IndexOf("Resources/") + ("Resources/".Length);
		int dotIndex = originalPath.IndexOf(".");

		return $"\"{originalPath.Substring(pathStartIndex, dotIndex - pathStartIndex)}\"";
		// Assets/Resources/Prefabs/Characters/PlayerableCharacter.prefab
		// -> "Prefabs/Characters/PlayerableCharacter"
	}
}
