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

	// 스킬 시전중 이동 가능 여부
	public bool moveableInCastTime;

	// 공중에서 시전 가능 여부
	public bool castableInAir;

	// 같은 스킬을 연계할 경우 콤보에 따라 재생시킬 애니메이션 이름을 나타냅니다.
	public string[] linkableSkillAnimationName;

	// 같은 스킬을 연계할 때의 쌓을 수 있는 최대 콤보 카운트를 나타냅니다.
	public int maxComboCount => linkableSkillAnimationName.Length;

	// 스킬 범위
	public SkillRangeInfo[] skillRangeInfos;
}
