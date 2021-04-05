using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NpcInfo
{
	// Npc 코드
	public string npcCode;

	// Npc 이름
	public string npcName;

	// 기본 대화 정보
	public NpcDialogInfo defaultDialogInfo;

	// 상점 코드
	public string shopCode;

	// 퀘스트 코드
	public List<string> questCodes;



	// Npc 정보가 비어있음을 나타냅니다.
	public bool IsEmpty() =>
		string.IsNullOrEmpty(npcName);

	// 이 Npc 를 통해 상점을 이용할 수 있음을 나타냅니다.
	public bool UseShop() =>
		string.IsNullOrEmpty(shopCode);

	// 이 Npc 를 통해 퀘스트를 진행할 수 있음을 나타냅니다.
	public bool UseQuest()
	{
		if (questCodes == null) return false;
		return (questCodes.Count != 0);
	}



}
