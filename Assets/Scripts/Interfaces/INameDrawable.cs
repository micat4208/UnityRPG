using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INameDrawable
{
	// 표시될 문자열을 나타냅니다.
	string drawName { get; }

	// 피벗을 나타냅니다.
	Vector3 drawablePosition { get; }
}
