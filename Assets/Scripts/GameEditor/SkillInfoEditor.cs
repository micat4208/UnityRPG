using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SkillInfoEditor : MonoBehaviour
{
	[SerializeField] private List<SkillInfo> _SkillInfos = new List<SkillInfo>();

	private void OnDestroy()
	{
		foreach (var skillInfo in _SkillInfos)
			ResourceManager.Instance.SaveJson(skillInfo, "SkillInfos", $"{skillInfo.skillCode}.json");
	}
}
