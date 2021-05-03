using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHPDrawable: INameDrawable
{
	// 최대 체력을 나타냅니다.
	float maxHp { get; }

	// 체력을 나타냅니다.
	float hp { get; }
}
