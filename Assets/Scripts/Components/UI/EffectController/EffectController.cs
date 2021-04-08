using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EffectController : MonoBehaviour
{
	// 재생시킬 때 사용되는 애니메이션 오브젝트 프리팹들을 나타냅니다.
	private Dictionary<ScreenEffectType, ScreenEffect> _ScreenEffectPrefabs = new Dictionary<ScreenEffectType, ScreenEffect>();

	private void Awake()
	{
		// Prefab Load
		_ScreenEffectPrefabs.Add(ScreenEffectType.ScreenFadeOut, ResourceManager.Instance.LoadResource<GameObject>(
			null, "Prefabs/UI/ScreenEffect/Image_Fade", false).GetComponent<ScreenEffect>());

		PlayAnimation(ScreenEffectType.ScreenFadeOut);
	}

	public ScreenEffect PlayAnimation(ScreenEffectType type)
	{
		// 재생시킬 애니메이션 오브젝트를 생성합니다.
		var newScreenEffect = Instantiate(_ScreenEffectPrefabs[type], transform);

		return newScreenEffect;
	}

}
