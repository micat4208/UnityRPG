using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIBHMoveTo : AIBehaviorBase
{
	// 이동할 목표 위치를 나타냅니다.
	private Vector3 _TargetPosition;

	// 목표 위치로 이동을 끝낼 때까지 대기합니다.
	private WaitUntil _WaitMoveFin;

	public NavMeshAgent navMeshAgent { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		navMeshAgent = GetComponent<NavMeshAgent>();

		m_BehaivorBeginDelay = 0.5f;
		m_BehaivorFinalDelay = 0.5f;
	}

	public override void Run()
	{
		IEnumerator BehaviorRun()
		{
			_WaitMoveFin = new WaitUntil(
				() => Vector3.Distance(transform.position, _TargetPosition) <= navMeshAgent.stoppingDistance);

			navMeshAgent.SetDestination(_TargetPosition);

			yield return _WaitMoveFin;
		}

		StartCoroutine(BehaviorRun());
	}

	public void SetDestination(Vector3 newPosition)
	{
		_TargetPosition = newPosition;
	}
}
