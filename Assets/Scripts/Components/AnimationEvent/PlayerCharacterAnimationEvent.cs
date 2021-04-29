using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 애니메이션 이벤트에 바인딩되는 메서드를 갖는 클래스입니다.
public sealed class PlayerCharacterAnimationEvent : MonoBehaviour
{
	private PlayerableCharacter _PlayerableCharacter;

	private void Awake()
	{
		_PlayerableCharacter = PlayerManager.Instance.playerController.
			playerableCharacter as PlayerableCharacter;
	}

	private void AnimEvent_SkillFinished()
	{
		_PlayerableCharacter.skillController.SkillFinished();
	}

	private void AnimEvent_SetSkillRequestable() =>
		_PlayerableCharacter.skillController.isRequestable = true;

	private void AnimEvent_BlockSkillRequestable() =>
		_PlayerableCharacter.skillController.isRequestable = false;

	private void AnimEvent_AddImpulseForward(float power) =>
		_PlayerableCharacter.movement.AddImpulse(
			_PlayerableCharacter.transform.forward * power);

}
