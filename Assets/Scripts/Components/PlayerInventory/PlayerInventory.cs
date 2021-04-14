using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInventory : MonoBehaviour
{
	// PlayerInventoryWnd 프리팹을 나타냅니다.
	private PlayerInventoryWnd _Wnd_PlayerInventoryPrefab;

	// PlayerInventory 창 객체를 나타냅니다.
	public PlayerInventoryWnd playerInventoryWnd { get; private set; }

	private void Awake()
	{
		_Wnd_PlayerInventoryPrefab = ResourceManager.Instance.LoadResource<GameObject>(
			"Wnd_PlayerInventoryPrefab",
			"Prefabs/UI/ClosableWnd/PlayerInventoryWnd/Wnd_PlayerInventory").GetComponent<PlayerInventoryWnd>();
	}

	// 인벤토리 창을 토글합니다.
	public void ToggleInventoryWnd()
	{
		if (playerInventoryWnd == null) OpenInventoryWnd();
		else CloseInventoryWnd();
	}

	// 인벤토리 창을 엽니다.
	public void OpenInventoryWnd(bool usePrevPosition = true)
	{
		if (playerInventoryWnd != null) return;

		// 인벤토리 창을 생성합니다.
		playerInventoryWnd = PlayerManager.Instance.playerController.
			screenInstance.CreateWnd(_Wnd_PlayerInventoryPrefab, usePrevPosition) as PlayerInventoryWnd;

		// 인벤토리 창이 닫힐 때 playerInventoryWnd 를 null 로 설정합니다.
		playerInventoryWnd.onWndClosedEvent += () => playerInventoryWnd = null;
	}

	// 인벤토리 창을 닫습니다.
	public void CloseInventoryWnd()
	{
		if (playerInventoryWnd == null) return;

		PlayerManager.Instance.playerController.
			screenInstance.CloseWnd(playerInventoryWnd);
	}

}
