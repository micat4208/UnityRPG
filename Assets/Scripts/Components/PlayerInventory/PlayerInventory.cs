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
			screenInstance.CloseWnd(false, playerInventoryWnd);
	}

	// 인벤토리 아이템을 교체합니다.
	public void SwapItem(PlayerInventoryItemSlot first, PlayerInventoryItemSlot second)
	{
		ref PlayerCharacterInfo playerInfo = 
			ref (PlayerManager.Instance.playerController as GamePlayerController).playerCharacterInfo;

		// 소지 아이템 정보 변경
		var tempItemInfo = playerInfo.inventoryItemInfos[first.itemSlotIndex];
		playerInfo.inventoryItemInfos[first.itemSlotIndex] = playerInfo.inventoryItemInfos[second.itemSlotIndex];
		playerInfo.inventoryItemInfos[second.itemSlotIndex] = tempItemInfo;

		// 슬롯 아이템 정보 변경
		first.SetItemInfo(playerInfo.inventoryItemInfos[first.itemSlotIndex].itemCode);
		second.SetItemInfo(playerInfo.inventoryItemInfos[second.itemSlotIndex].itemCode);

		// 슬롯 갱신
		first.UpdateInventoryItemSlot();
		second.UpdateInventoryItemSlot();
	}

	// 아이템을 인벤토리에 추가합니다.
	/// - newItemSlotInfo : 추가할 아이템 정보를 전달합니다.
	/// - return
	///   result : 추가를 완료한 경우 true 입니다.
	///   resultInfo : 아이템 추가 작업이 이루어진 후 남은 아이템을 나타냅니다.
	public (bool result, ItemSlotInfo resultInfo) AddItem(ItemSlotInfo newItemSlotInfo)
	{
		var playerController = PlayerManager.Instance.playerController as GamePlayerController;
		ref PlayerCharacterInfo playerInfo = ref playerController.playerCharacterInfo;

		if (newItemSlotInfo.itemCount <= 0)
		{
#if UNITY_EDITOR
			Debug.LogError("newItemSlotInfo.itemCount is Zero!");
#endif
			return (true, newItemSlotInfo);
		}

		// 아이템을 채웁니다.
		/// - slotIndex : 채울 슬롯 인덱스를 전달합니다.
		void FillSlot(List<ItemSlotInfo> inventoryItemSlotInfos, int slotIndex)
		{
			// 아이템을 추가할 수 있는 여유 공간이 존재하는지 확인합니다.
			int addableItemCount = inventoryItemSlotInfos[slotIndex].maxSlotCount - inventoryItemSlotInfos[slotIndex].itemCount;
			if (addableItemCount > 0)
			{
				// 추가할 수 있는 여유 공간을 매꾸며, 아이템을 최대한 채웁니다.
				for (int i = 0; 
					(i < addableItemCount) && (newItemSlotInfo.itemCount > 0); 
					++i)
				{
					// 아이템을 추가합니다.
					ItemSlotInfo itemSlotInfo = inventoryItemSlotInfos[slotIndex];
					++itemSlotInfo.itemCount;
					inventoryItemSlotInfos[slotIndex] = itemSlotInfo;

					// 추가한 아이템을 제외합니다.
					--newItemSlotInfo.itemCount;
				}
			}

		}

		for (int i = 0; i < playerInfo.inventorySlotCount; ++i)
		{
			// 만약 추가하려는 아이템과 동일한 아이템을 갖는 슬롯을 찾았다면
			if (playerInfo.inventoryItemInfos[i] == newItemSlotInfo)
			{
				// 아이템 추가
				FillSlot(playerInfo.inventoryItemInfos, i);
			}

			// 빈 아이템 슬롯을 찾았다면
			else if (playerInfo.inventoryItemInfos[i].IsEmpty())
			{
				ItemSlotInfo slotInfo = newItemSlotInfo;
				slotInfo.itemCount = 0;
				playerInfo.inventoryItemInfos[i] = slotInfo;

				// 아이템 추가
				FillSlot(playerInfo.inventoryItemInfos, i);
			}

			// 모든 아이템을 추가했다면
			if (newItemSlotInfo.itemCount == 0)
			{
				// 아이템을 모두 추가했음.
				return (true, newItemSlotInfo);
			}
		}

		// 아이템을 모두 추가하지 못했음.
		return (false, newItemSlotInfo);
	}

	// 아이템을 인벤토리에서 제거합니다.
	/// - itemSlotIndex : 제거할 아이템을 갖는 슬롯 인덱스를 전달합니다.
	/// - removeCount : 제거 개수를 전달합니다.
	public void RemoveItem(int itemSlotIndex, int removeCount)
	{
		ref var playerInfo = ref (PlayerManager.Instance.playerController as GamePlayerController).playerCharacterInfo;

		// 슬롯에서 아이템 개수를 감소시킵니다.
		ItemSlotInfo itemSlotInfo = playerInfo.inventoryItemInfos[itemSlotIndex];
		itemSlotInfo.itemCount -= removeCount;
		playerInfo.inventoryItemInfos[itemSlotIndex] = itemSlotInfo;

		// 만약 슬롯에 아이템이 존재하지 않는다면
		if (itemSlotInfo.itemCount <= 0)
		{
			// 슬롯 정보를 비웁니다.
			itemSlotInfo.Clear();

			// 슬롯 내용을 비웁니다.
			playerInfo.inventoryItemInfos[itemSlotIndex] = itemSlotInfo;

		}

		// 인벤토리 창이 열려있다면
		if (playerInventoryWnd)
		{
			// 슬롯 갱신
			PlayerInventoryItemSlot inventorySlot = playerInventoryWnd.itemSlots[itemSlotIndex];

			inventorySlot.SetItemInfo(itemSlotInfo.itemCode);

			inventorySlot.UpdateInventoryItemSlot();
		}
	}

	// 아이템을 합칩니다.
	public void MergeItem(PlayerInventoryItemSlot ori, PlayerInventoryItemSlot target)
	{
		if (ori == target) return;

		GamePlayerController playerController = PlayerManager.Instance.playerController as GamePlayerController;
		ref PlayerCharacterInfo playerInfo = ref playerController.playerCharacterInfo;

		ItemSlotInfo oriItemSlotInfo = playerInfo.inventoryItemInfos[ori.itemSlotIndex];
		ItemSlotInfo targetItemSlotInfo = playerInfo.inventoryItemInfos[target.itemSlotIndex];

		// 슬롯에 들어갈 수 있는 최대 아이템 개수
		int maxSlotCount = ori.itemInfo.maxSlotCount;

		// 둘 중 하나라도 최대 개수라면 스왑이 일어나도록 합니다.
		if (oriItemSlotInfo.itemCount == maxSlotCount ||
			targetItemSlotInfo.itemCount == maxSlotCount)
		{
			SwapItem(ori, target);
		}
		else
		{
			// 추가 가능한 아이템 개수를 계산합니다.
			int addable = maxSlotCount - targetItemSlotInfo.itemCount;

			// 옮기려는 아이템의 개수가 슬롯의 아이템 개수보다 큰 경우
			if (addable > oriItemSlotInfo.itemCount)
				// 옮길 개수를 슬롯 아이템 개수로 설정합니다.
				addable = oriItemSlotInfo.itemCount;

			// 아이템을 옮깁니다.
			oriItemSlotInfo.itemCount -= addable;
			targetItemSlotInfo.itemCount += addable;

			// 옮긴 후 슬롯이 비어있다면
			if (oriItemSlotInfo.itemCount == 0)
			{
				oriItemSlotInfo.Clear();
				ori.SetItemInfo("");
				Debug.Log(oriItemSlotInfo.itemCode);
				Debug.Log(oriItemSlotInfo.itemCount);
			}

			playerInfo.inventoryItemInfos[ori.itemSlotIndex] = oriItemSlotInfo;
			playerInfo.inventoryItemInfos[target.itemSlotIndex] = targetItemSlotInfo;

			// 슬롯 갱신
			ori.UpdateInventoryItemSlot();
			target.UpdateInventoryItemSlot();
		}
	}

}
