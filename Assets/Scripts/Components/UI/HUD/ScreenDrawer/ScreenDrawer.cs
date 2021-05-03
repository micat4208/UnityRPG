using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ScreenDrawer : MonoBehaviour
{
	static NameDrawer HUD_NameDrawerPrefab;

	private List<(INameDrawable drawable, bool hpDrawable)> _NameDrawables = new List<(INameDrawable, bool)>();

	private void Awake()
	{
		if (!HUD_NameDrawerPrefab)
		{
			HUD_NameDrawerPrefab = ResourceManager.Instance.LoadResource<GameObject>(
				"HUD_NameDrawer",
				"Prefabs/UI/HUD/NameDrawer/HUD_NameDrawer").GetComponent<NameDrawer>();
		}
	}

	public void AddNameDrawable(INameDrawable nameDrawable, bool hpDrawable = false)
	{
		NameDrawer CreateNameDrawer()
		{
			NameDrawer newNameDrawer = Instantiate(HUD_NameDrawerPrefab, transform);
			newNameDrawer.Initialize(nameDrawable);

			return newNameDrawer;
		}

		_NameDrawables.Add((nameDrawable, hpDrawable));
		CreateNameDrawer();
	}

	public void RemoveNameDrawable(INameDrawable nameDrawable)
	{
		_NameDrawables.RemoveAll(
			((INameDrawable drawable, bool hpDrawable) elem) => elem.drawable == nameDrawable);
	}





}
