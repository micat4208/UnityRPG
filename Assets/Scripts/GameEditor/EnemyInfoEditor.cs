using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyInfoEditor : MonoBehaviour
{
	[SerializeField] private List<EnemyInfo> _EnemyInfos;

	private void OnDestroy()
	{
		foreach (var enemyInfo in _EnemyInfos)
			ResourceManager.Instance.SaveJson(enemyInfo, "EnemyInfos", $"{enemyInfo.enemyCode}.json");
	}



}
