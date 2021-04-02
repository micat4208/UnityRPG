using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 캐릭터에 사용되는 Animator 컴포넌트를 제어하기 위한 컴포넌트입니다.
public sealed class PlayerCharacterAnimController : AnimController
{
	private PlayerableCharacter _PlayerCharacter;

	private void Awake()
	{
		_PlayerCharacter = GetComponent<PlayerableCharacter>();
	}

	private void Update()
	{
		if (!controlledAnimator) return;

		SetParam("_VelocityLength", _PlayerCharacter.movement.moveXZVelocity.magnitude);
		SetParam("_IsInAir", !_PlayerCharacter.movement.isGrounded);
	}

}
