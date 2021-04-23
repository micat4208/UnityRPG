using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI 의 행동을 제어하는 컴포넌트
public sealed class BehaviorController : 
	MonoBehaviour
{
	// 수행할 행동 객체들을 저장할 리스트
	private List<AIBehaviorBase> _AIBehaivors = new List<AIBehaviorBase>();

	private AIBehaviorBase _NextBehavior;
	private IEnumerator _Behavior;



	// 행동을 시작시킵니다.
	public void StartBehavior()
	{
		// 실행할 행동이 없을 경우 실행 취소
		if (_AIBehaivors.Count == 0) return;

		// 모든 행동 초기화
		foreach (var behavior in _AIBehaivors)
			behavior.InitializeBehavior();

		StartCoroutine(_Behavior = BehaviorControl());
	}

	// 행동을 중단합니다.
	public void StopBehaivor()
	{
		StopCoroutine(_Behavior);

		_NextBehavior.StopBehaivor();
	}

	// 행동을 제어합니다.
	private IEnumerator BehaviorControl()
	{
		// 다음으로 실행할 행동 인덱스를 나타냅니다.
		int nextBehaivorIndex = 0;

		// 행동 시작을 허용할 때까지 대기합니다.
		WaitUntil waitAllowBehaviorStarted = new WaitUntil(
			() => _NextBehavior.allowStartBehavior);

		// 행동이 끝날 때까지 대기합니다.
		WaitUntil waitBehaviorFinished = new WaitUntil(
			() => _NextBehavior.behaviorFinished);

		while (true)
		{
			// 실행할 내용을 결정합니다.
			_NextBehavior = _AIBehaivors[nextBehaivorIndex];

			// 다음 행동 인덱스로 변경합니다.
			nextBehaivorIndex = (nextBehaivorIndex == _AIBehaivors.Count - 1) ?
				0 : ++nextBehaivorIndex;

			// 만약 행동 시작 지연 시간이 0 초가 아닐 경우
			if (!Mathf.Approximately(_NextBehavior.behaivorBeginDelay, 0.0f))
				yield return new WaitForSeconds(_NextBehavior.behaivorBeginDelay);

			// 다음 Update() 호출까지 대기
			yield return null;

			// 행동 시작 이벤트 실행
			_NextBehavior.onBehaviorStarted?.Invoke();
			
			// 행동 시작을 대기
			yield return waitAllowBehaviorStarted;

			// 행동 실행
			_NextBehavior.Run();

			// 행동 끝을 대기
			yield return waitBehaviorFinished;

			// 행동 끝 이벤트 실행
			_NextBehavior.onBehaviorFinished?.Invoke();

			// 만약 행동 끝 지연 시간이 0 이 아닐 경우
			if (!Mathf.Approximately(_NextBehavior.behaivorFinalDelay, 0.0f))
				yield return new WaitForSeconds(_NextBehavior.behaivorFinalDelay);

			// 다음 Update() 호출까지 대기
			yield return null;

			// 행동 상태 초기화
			_NextBehavior.InitializeBehavior();
		}

	}

	// 행동을 추가합니다.
	public void AddBehaivor(string behaviorName) =>
		_AIBehaivors.Add(gameObject.AddComponent(System.Type.GetType(behaviorName)) as AIBehaviorBase);



}
