using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬을 관리하는 컴포넌트입니다.
public sealed class PlayerSkillController : MonoBehaviour
{
	// 현재 실행중인 스킬 정보를 나타냅니다.
	private SkillInfo? _CurrentSkillInfo;

	// 바로 이전에 실행시킨 스킬 정보를 나타냅니다.
	private SkillInfo? _PrevSkillInfo;

	private Queue<SkillInfo> _SkillQueue = new Queue<SkillInfo>();

	

	// 스킬을 순서대로 처리합니다.
	private void SkillProcedure()
	{
		// 현재 실행중인 스킬이 존재한다면 실행하지 않습니다.
		if (_CurrentSkillInfo == null) return;

		// 큐에 요소가 없다면 실행하지 않습니다.
		if (_SkillQueue.Count == 0) return;

		// 다음에 실행시킬 스킬 정보를 얻습니다.
		SkillInfo skillInfo = _SkillQueue.Dequeue();

		// 스킬을 시전합니다.
		CastSkill(skillInfo);
	}

	// 스킬을 시전합니다.
	private void CastSkill(SkillInfo skillInfo)
	{
		// 현재 실행중인 스킬로 설정합니다.
		_CurrentSkillInfo = skillInfo;

	}
}
