using UnityEngine;
using System.Collections.Generic;

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
		// str 을 실제 계산에 사용되는 수치로 변환시킵니다.
		float ToValue(string str) =>
			float.Parse(str.Remove(0, 1));


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

		// 연산 방식 상수
		const byte defaultType		= 0b0000_0000;
		const byte addition			= 0b0000_0001;
		const byte subtraction		= 0b0000_0010;
		const byte multiplication	= 0b0000_0100;
		const byte division			= 0b0000_1000;

		// 연산 데이터들을 나타냅니다.
		Queue<(byte, string)> calcSeq = new Queue<(byte, string)>();
		// 연산 방식을 저장하기 위한 변수
		byte calcType = defaultType;

		// 수치를 나타내기 위한 변수
		string calcValue = "";

		// 영역 개수를 나타내기 위한 변수
		/// - 이 값이 0 이라면 영역 개수를 읽는 상태로 인식시킵니다.
		int rangeCount = 1;

		// 식을 읽어 연산 데이터를 생성합니다.
		for (int i = 0; i < skillCalcFormula.Length + 1; ++i)
		{
			// 모든 문자열을 읽었을 경우
			if (i == skillCalcFormula.Length)
			{
				// 하나의 데이터를 큐에 추가합니다
				calcSeq.Enqueue((calcType, calcValue));
				break;
			}


			// 문자 하나를 읽습니다.
			char read = skillCalcFormula[i];

			// 공백을 읽었다면 하나의 데이터 끝으로 인식시킵니다.
			if (read == ' ')
			{
				if (rangeCount == 0)
				{
					// 영역 개수를 저장합니다.
					rangeCount = int.Parse(calcValue);
					if (rangeCount <= 0) rangeCount = 1;
					/// - 영역 개수가 0 이하의 값으로 설정될 수 없도록 합니다.

					// 읽은 값을 지웁니다.
					calcValue = "";

				}
				// 피연산자가 존재하는 경우
				else if (!string.IsNullOrEmpty(calcValue))
				{
					// 읽은 데이터를 큐에 추가합니다.
					calcSeq.Enqueue((calcType, calcValue));

					// 연산 방식, 데이터를 비웁니다.
					calcType = defaultType;
					calcValue = "";
				}
			}

			// 영역 개수
			else if (read == 'h' || read == 'H')
				rangeCount = 0;

			// 산술 연산자
			else if (read == '+') calcType = addition;
			else if (read == '-') calcType = subtraction;
			else if (read == '*') calcType = multiplication;
			else if (read == '/') calcType = division;

			// 나머지 데이터 추가
			else calcValue += read;
		}

		// 연산합니다.
		while (calcSeq.Count != 0)
		{
			// 연산 데이터 하나 꺼내오기
			var dequeue = calcSeq.Dequeue();

			// 연산 방식을 얻습니다.
			calcType = dequeue.Item1;

			// 피연산자를 얻습니다.
			calcValue = dequeue.Item2;

			// 실제 계산에 사용되는 수치 데이터를 저장할 변수를 선언합니다.
			float value;

			// p (퍼댐)
			if (calcValue[0] == 'p' || calcValue[0] == 'P')
				value = atk * 0.01f * ToValue(calcValue);

			// d (기본 공격력 X N)
			else if (calcValue[0] == 'd' || calcValue[0] == 'D')
				value = atk * ToValue(calcValue);

			// 수치 계산
			else value = float.Parse(calcValue);

			// 연산
			switch (calcType)
			{
			case defaultType:		result = value;		break;
			case addition:			result += value;	break;
			case subtraction:		result -= value;	break;
			case multiplication:	result *= value;	break;
			case division:			result /= value;	break;
			}
		}


		return (rangeCount, result);
	}

}



