using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffect : AnimController
{
	public System.Action onAnimationFinished { get; set; }

	// 애니메이션 재생이 끝났음을 나타냅니다.
	public bool animationFinished { get; private set; }

	protected virtual void Awake()
	{
		controlledAnimator = GetComponent<Animator>();
		onAnimationFinished += () => animationFinished = true;

		StartCoroutine(WaitAnimationFin());
	}

	private void AnimEvent_AnimationFinished() =>
		onAnimationFinished?.Invoke();

	// 애니메이션 재생이 끝날 때까지 대기합니다.
	private IEnumerator WaitAnimationFin()
	{
		// 애니메이션 재생이 끝날 때까지 대기합니다.
		yield return new WaitUntil(() => animationFinished);

		// 애니메이션 재생이 끝났다면 이 오브젝트를 제거합니다.
		Destroy(gameObject);
	}
}
