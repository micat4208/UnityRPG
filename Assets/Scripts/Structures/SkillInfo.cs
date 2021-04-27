using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SkillInfo
{
	// 스킬 코드
	public string skillCode;

	// 스킬 이름
	public string skillName;

	// 스킬 설명
	public string skillDescription;

	// 사용하는 아이콘 이미지 경로
	public string skillIconImagePath;

	// 쿨타임 사용 여부
	public bool useCoolTime;

	// 쿨타임
	public float coolTime;

	// 같은 스킬을 연계할 경우 콤보에 따라 재생시킬 애니메이션 이름을 나타냅니다.
	public string[] LinkableSkillAnimationName;
}
