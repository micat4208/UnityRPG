using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyInfo
{
	// 적 코드
	public string enemyCode;

	// 적 이름
	public string enemyName;

	// 적 Capsule 크기
	public float capsuleHeight;

	// 적 Capsule 반지름
	public float capsuleRadius;

	// 적 Mesh 경로
	public string enemyMeshPath;

	// 적 최대 이동 속력
	public float maxMoveSpeed;

	// 사용하는 행동 객체 이름
	public string[] useBehaviorNames;
}
