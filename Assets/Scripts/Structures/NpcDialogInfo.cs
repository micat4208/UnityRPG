using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NPC 대화 정보를 나타내기 위한 구조체
[System.Serializable]
public struct NpcDialogInfo
{
	// 대화 문자열들을 나타냅니다.
	public List<string> dialogText;
}
