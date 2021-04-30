using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬을 관리하는 컴포넌트입니다.
public sealed class PlayerSkillController : MonoBehaviour
{
	private PlayerableCharacter _PlayerableCharacter;

	// 현재 실행중인 스킬 정보를 나타냅니다.
	private SkillInfo? _CurrentSkillInfo;

	// 바로 이전에 실행시킨 스킬 정보를 나타냅니다.
	private SkillInfo? _PrevSkillInfo;

	// 실행시킬 스킬 정보를 담아둘 큐
	private Queue<SkillInfo> _SkillQueue = new Queue<SkillInfo>();

	// 스킬 큐 요소 최대 개수
	private int _MaxQueueCount = 1;

	// 실행했었던 스킬 상태 정보들을 나타냅니다.
	private Dictionary<string, SkillProgressInfo> _UsedSkillInfo = new Dictionary<string, SkillProgressInfo>();

#if UNITY_EDITOR
	private List<(Vector3 start, Vector3 end, float radius, Color color, float duration, bool drawStart, float addedTime)> _DebugSkillRanges = 
		new List<(Vector3, Vector3, float, Color, float, bool, float)>();
#endif


	// 스킬을 요청할 수 있는 상태임을 나타냅니다.
	/// - 이 값이 false 라면 스킬을 요청할 수 없도록 합니다.
	public bool isRequestable { get; set; } = true;

	// 이동 제한 상태를 나타냅니다.
	public bool blockMovement { get; set; } = false;

	




	private void Awake()
	{
		_PlayerableCharacter = GetComponent<PlayerableCharacter>();
	}

	private void Update()
	{
		// 스킬을 순서대로 실행시킵니다.
		SkillProcedure();
	}

	// 스킬을 순서대로 처리합니다.
	private void SkillProcedure()
	{
		// 현재 실행중인 스킬이 존재한다면 실행하지 않습니다.
		if (_CurrentSkillInfo != null) return;

		// 큐에 요소가 없다면 실행하지 않습니다.
		if (_SkillQueue.Count == 0) return;

		// 다음에 실행시킬 스킬 정보를 얻습니다.
		SkillInfo skillInfo = _SkillQueue.Dequeue();

		UpdateUsedSkillInfo(skillInfo);

		// 스킬을 시전합니다.
		CastSkill(skillInfo);
	}

	// 스킬 상태 정보를 갱신합니다.
	private void UpdateUsedSkillInfo(SkillInfo newSkillInfo)
	{
		// 전에 사용한 스킬이 사용될 스킬과 다른 스킬이라면
		if (_PrevSkillInfo != null)
		{
			if (_PrevSkillInfo.Value.skillCode != newSkillInfo.skillCode)
			{
				SkillProgressInfo prevSkillProgressInfo = _UsedSkillInfo[_PrevSkillInfo.Value.skillCode];

				// 전에 사용한 스킬을 연계 시작 가능한 상태로 설정합니다.
				prevSkillProgressInfo.skillCombo = -1;

				_UsedSkillInfo[_PrevSkillInfo.Value.skillCode] = prevSkillProgressInfo;
			}
		}


		// 이전에 스킬이 사용된 적이 있다면
		if (_UsedSkillInfo.ContainsKey(newSkillInfo.skillCode))
		{
			// 콤보를 사용하는 스킬이라면
			if (newSkillInfo.maxComboCount != 0)
			{
				SkillProgressInfo progressInfo = _UsedSkillInfo[newSkillInfo.skillCode];

				// 콤보 카운트를 증가시킵니다.
				progressInfo.AddCombo();

				// 콤보 카운트가 최대 콤보 카운트를 초과한다면 리셋합니다.
				if (progressInfo.skillCombo == newSkillInfo.maxComboCount)
					progressInfo.ResetCombo();

				// 변경한 내용을 적용시킵니다.
				_UsedSkillInfo[newSkillInfo.skillCode] = progressInfo;
			}
		}
		// 스킬이 처음 사용된다면
		else
		{
			// 새로운 데이터를 추가합니다.
			SkillProgressInfo newSkillProgressInfo = new SkillProgressInfo(
				newSkillInfo.skillCode, 0);

			_UsedSkillInfo.Add(newSkillInfo.skillCode, newSkillProgressInfo);
		}

	}

	// 스킬을 시전합니다.
	private void CastSkill(SkillInfo skillInfo)
	{
		// 현재 실행중인 스킬로 설정합니다.
		_CurrentSkillInfo = skillInfo;

		// 시전시킬 스킬이 이동을 제한하는지 확인합니다.
		blockMovement = !_CurrentSkillInfo.Value.moveableInCastTime;

		// 스킬실행 후 스킬 요청이 이루어질 수 없도록 합니다.
		isRequestable = false;

		// 전에 사용한 스킬이 존재한다면
		if (_PrevSkillInfo != null)
		{
			// 같은 스킬을 연계 공격으로 사용한다면
			if ((_PrevSkillInfo.Value.skillCode == _CurrentSkillInfo.Value.skillCode) &&
				_CurrentSkillInfo.Value.maxComboCount != 0)
			{
				// 애니메이션 클립 이름을 얻습니다.
				int currentComboCount = _UsedSkillInfo[_CurrentSkillInfo.Value.skillCode].skillCombo;
				string animClipName = _CurrentSkillInfo.Value.linkableSkillAnimationName[currentComboCount];

				// 애니메이션을 재생합니다.
				_PlayerableCharacter.animController.controlledAnimator?.Play(animClipName);
				return;
			}
		}

		_PlayerableCharacter.animController.controlledAnimator?.CrossFade(
			_CurrentSkillInfo.Value.linkableSkillAnimationName[0], 0.15f);
	}

	// 스킬 실행을 요청합니다.
	public void RequestSkill(string skillCode)
	{
		// 스킬을 요청할 수 없는 상태라면 실행하지 않습니다.
		if (!isRequestable) return;

		// 스킬이 _MaxQueueCount 개 이상 등록되었다면 추가시키지 않습니다.
		if (_SkillQueue.Count >= _MaxQueueCount) return;

		// 요청한 스킬 정보를 얻습니다.
		bool fileNotFound;
		SkillInfo requestSkillInfo = ResourceManager.Instance.LoadJson<SkillInfo>(
			"SkillInfos", $"{skillCode}.json", out fileNotFound);

		// 요청한 스킬 정보를 찾지 못했다면 요청 취소
		if (fileNotFound)
		{
			Debug.LogError($"skillCode({skillCode}) is not available.");
			return;
		}

		if (!requestSkillInfo.castableInAir && !_PlayerableCharacter.movement.isGrounded) return;

		// 요청한 스킬을 큐에 추가합니다.
		_SkillQueue.Enqueue(requestSkillInfo);
	}

	// 스킬이 끝났음을 알립니다.
	public void SkillFinished()
	{
		_PrevSkillInfo = _CurrentSkillInfo;
		_CurrentSkillInfo = null;

		// 스킬 요청 가능 상태로 설정합니다.
		isRequestable = true;

		// 이동 제한을 해제합니다.
		blockMovement = false;

		_PlayerableCharacter.animController.controlledAnimator?.CrossFade("BT_MoveGround", 0.2f);

		// 사용할 스킬이 존재하지 않는다면
		if (_SkillQueue.Count == 0)
		{
			// 사용했던 스킬 콤보를 연계 시작 가능한 상태로 설정합니다.
			SkillProgressInfo skillProgressInfo = _UsedSkillInfo[_PrevSkillInfo.Value.skillCode];
			skillProgressInfo.skillCombo = -1;
			_UsedSkillInfo[_PrevSkillInfo.Value.skillCode] = skillProgressInfo;
		}
	}

	// 스킬 범위 인덱스를 다음 인덱스로 변경합니다.
	public void NextSkillRangeIndex()
	{
		// 현재 실행중인 스킬이 존재하지 않는다면 실행하지 않습니다.
		if (_CurrentSkillInfo == null) return;

		// 스킬 범위 인덱스 변경
		SkillProgressInfo skillProgressInfo = _UsedSkillInfo[_CurrentSkillInfo.Value.skillCode];
		++skillProgressInfo.currentSkillRandeIndex;

		// 만약 인덱스라 배열 범위를 초과하는 경우 마지막 요소 인덱스로 설정
		if (skillProgressInfo.currentSkillRandeIndex == _CurrentSkillInfo.Value.skillRangeInfos.Length)
			--skillProgressInfo.currentSkillRandeIndex;

		_UsedSkillInfo[_CurrentSkillInfo.Value.skillCode] = skillProgressInfo;
	}

	// 스킬 범위를 생성합니다.
	public void MakeSkillRange()
	{
		if (_CurrentSkillInfo == null) return;

		// 스킬 공격 범위 인덱스를 얻습니다.
		int rangeIndex = _UsedSkillInfo[_CurrentSkillInfo.Value.skillCode].currentSkillRandeIndex;

		// 스킬 범위 정보를 얻습니다.
		SkillRangeInfo skillRangeInfo = _CurrentSkillInfo.Value.skillRangeInfos[rangeIndex];

		// 계산식 결과를 얻습니다.
		var calcFormulaResult = skillRangeInfo.GetSkillCalcFormulaResult();

		Ray ray = new Ray(transform.position, transform.forward);
		float distance = 5.0f;
		float radius = 2.0f;
#if UNITY_EDITOR
		float debugDrawDuration = 5.0f;
#endif
		RaycastHit[] hits = Physics.SphereCastAll(
			ray,
			radius,
			distance,
			1 << LayerMask.NameToLayer("EnemyCharacter"));

#if UNITY_EDITOR
		void AddDebugRange(
			Vector3 start, 
			Vector3 end, 
			float drawRadius, 
			Color32 color, 
			float duration = 5.0f, 
			bool drawStart = true)
		{
			var debugRangeData = (start, end, drawRadius, color, duration, drawStart, Time.time);
			_DebugSkillRanges.Add(debugRangeData);
		}

		// 기본 범위 그리기
		AddDebugRange(ray.origin, ray.origin + (ray.direction * distance), radius, Color.red, debugDrawDuration);

		// 감지 그리기
		foreach(RaycastHit hit in hits)
		{
			Debug.Log(hit.point);

			// 감지된 위치까지의 거리
			AddDebugRange(ray.origin, hit.point, radius, Color.green, debugDrawDuration, false);
		}
#endif

	}

#if UNITY_EDITOR

	// 씬 화면에 어떤 것을 그릴 때 사용되는 메서드
	private void OnDrawGizmos()
	{
		foreach(var debug in _DebugSkillRanges)
		{
			// 그릴 색상을 설정합니다.
			Gizmos.color = debug.color;

			// 시작 위치를 그린다면
			if (debug.drawStart)
				// 시작 위치를 그립니다.
				Gizmos.DrawWireSphere(debug.start, debug.radius);

			// 시작 위치부터 끝 위치까지 선을 그립니다.
			Gizmos.DrawLine(debug.start, debug.end);

			// 끝 위치를 그립니다.
			Gizmos.DrawWireSphere(debug.end, debug.radius);
		}

		// 그리는 시간이 초과한 요소들을 제거합니다.
		_DebugSkillRanges.RemoveAll((debugData) =>
			Time.time >= debugData.addedTime + debugData.duration);
	}
#endif
}
