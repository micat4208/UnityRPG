

using UnityEngine;

[System.Serializable]
public struct SkillRangeInfo
{
	// SphereCast 사용 여부를 나타냅니다.
	public bool useSphereCast;

	// 캐릭터가 기준이 되는 범위 생성 시작 범위 오프셋
	public Vector3 castStartOffset;

	// SphereCast 방향
	public Vector3 castDirection;

	// SphereCast 거리
	public float castDistance;

	// SphereCast 구 반지름
	public float radius;

	// 스킬 계산식
	public string skillCalcFormula;
	/// - ex) h3 100 + d2 * p150
	///       100 + (damage * 2) * (damage 의 150%) 의 대미지를 가하는 영역을 3개 생성합니다.
	///       연산 우선 순위 사용 X

	// 여러 영역 생성 시 영역 생성 딜레이
	public float createDelay;

	// 스킬 계산식 결과 영역 개수와, 대미지를 얻습니다.
	/// - return : (영역 개수, 대미지)
	public (int rangeCount, float value) GetSkillCalcFormulaResult()
	{

		// 계산 결과를 저장할 변수
		float result = 0.0f;

		// 계산식 : "h3 200 + p300 + d3 - 10 * 3"
		// result = 200;
		// result += (atk * 300%)
		// result += (atk * 3)
		// result -= 10
		// result *= 3
		// result      =>      3270
		// rangeCount  =>      3
		// 현재 플레이어 공격력

		// 이 계산식을 푸는 코드를 Queue 를 이용하여 작성해보세요.

		float atk = 150.0f;



		return (0, 0.0f);
	}

}



