using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI 행동 객체를 만들기 위한 기반 타입
public abstract class AIBehaviorBase:
	MonoBehaviour
{
	[Header("행동 시작 / 끝 지연 시간")]
	[SerializeField] protected float m_BehaivorBeginDelay = 0.0f;
	[SerializeField] protected float m_BehaivorFinalDelay = 0.0f;

	// 행동 시작을 제어하기 위한 프로퍼티
	public bool allowStartBehavior { get; set; }
	/// - 이 값이 true 일 경우 행동이 시작됩니다.

	// 행동이 끝났음을 나타내는 프로퍼티
	public bool behaviorFinished { get; set; }
	/// - 이 값이 true 일 경우 행동이 끝납니다.

	// 행동이 시작될 때 실행될 내용
	public System.Action onBehaviorStarted { get; set; }

	// 행동이 끝날 때 실행될 내용
	public System.Action onBehaviorFinished { get; set; }

	// 행동을 제어하는 BehaviorController Component 를 나타냅니다.
	public BehaviorController behaviorController { get; private set; }

	public float behaivorBeginDelay => m_BehaivorBeginDelay;
	public float behaivorFinalDelay => m_BehaivorFinalDelay; 

	protected virtual void Awake()
	{
		behaviorController = GetComponent<BehaviorController>();


		// 기본적으로 행동 시작을 허용하도록 합니다.
		onBehaviorStarted = () => allowStartBehavior = true;
	}

	public virtual void InitializeBehavior()
	{
		allowStartBehavior = behaviorFinished = false;
	}

	// 행동
	public abstract void Run();

	// 행동 중지
	public virtual void StopBehaivor() { }
}
