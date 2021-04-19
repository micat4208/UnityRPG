using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ShopInfoEditor : MonoBehaviour
{
	[SerializeField] private List<ShopInfo> _ShopInfos = new List<ShopInfo>();

	private void OnDestroy()
	{
		foreach (var shopInfo in _ShopInfos)
			ResourceManager.Instance.SaveJson(shopInfo, "ShopInfos", $"{shopInfo.shopCode}.json");
	}
}
