using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameScreenInstance : ScreenInstance
{
	[SerializeField] private EffectController _EffectController;
	public EffectController effectController => _EffectController;
}
