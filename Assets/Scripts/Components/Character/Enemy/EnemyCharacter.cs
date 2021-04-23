using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorController))]
public sealed class EnemyCharacter : CharacterBase
{
	[Header("적 코드")]
	[SerializeField] private string _EnemyCode;

	private EnemyInfo _EnemyInfo;

	public NavMeshAgent navMeshAgent { get; private set; }
	public BehaviorController behaviorController { get; private set; }

	public ref EnemyInfo enemyInfo => ref _EnemyInfo;

	private void Awake()
	{
		idCollider = GetComponent<CapsuleCollider>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		behaviorController = GetComponent<BehaviorController>();

		idCollider.isTrigger = true;
	}

	protected override void Start()
	{
		base.Start();

		InitializeEnemyCharacter(_EnemyCode);
	}

	private void Update()
	{
		// 목표 위치를 설정합니다.
		navMeshAgent.SetDestination(
			PlayerManager.Instance.playerController.
			playerableCharacter.transform.position);
		/// - SetDestination(position) : position 의 위치를 목표 위치로 하여, 이동을 시작합니다.
	}

	// 적 캐릭터를 초기화합니다.
	public void InitializeEnemyCharacter(string enemyCode)
	{
		bool fileNotFound;
		_EnemyInfo = ResourceManager.Instance.LoadJson<EnemyInfo>(
			"EnemyInfos", $"{enemyCode}.json", out fileNotFound);

		if (fileNotFound)
		{
#if UNITY_EDITOR
		Debug.Log($"_EnemyInfo is not valid! (enemyCode is {enemyCode}!)");
#endif
			return;
		}

		// Enemy Mesh 생성
		Instantiate(ResourceManager.Instance.LoadResource<GameObject>(
				$"{enemyCode}_Mesh", _EnemyInfo.enemyMeshPath), transform);

		// 이동 속도 설정
		navMeshAgent.speed = _EnemyInfo.maxMoveSpeed;

		// 캡슐 크기 설정
		(idCollider as CapsuleCollider).height = _EnemyInfo.capsuleHeight;
		(idCollider as CapsuleCollider).radius = _EnemyInfo.capsuleRadius;

		// 캡슐 오프셋 설정
		(idCollider as CapsuleCollider).center = Vector3.up * (_EnemyInfo.capsuleHeight * 0.5f);

		// 행동 객체 생성
		foreach (string behaviorName in _EnemyInfo.useBehaviorNames)
			behaviorController.AddBehaivor(behaviorName);

		// 행동 시작
		behaviorController.StartBehavior();
	}
}
