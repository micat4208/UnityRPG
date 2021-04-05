using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 상호작용 기능을 담당하는 컴포넌트입니다.
public sealed class PlayerInteract : MonoBehaviour
{
	// 상호작용 가능한 객체들을 나타냅니다.
	[SerializeField] private List<InteractableArea> _InteractableAreas = new List<InteractableArea>();

	// 상호작용 가능한 객체를 추가합니다.
	public void AddInteractable(InteractableArea newInteractable)
	{
		if (!_InteractableAreas.Contains(newInteractable))
			_InteractableAreas.Add(newInteractable);
	}

	// 상호작용 가능한 객체에서 제외시킵니다.
	public void RemoveInteractable(InteractableArea removeInteractable)
	{
		if (_InteractableAreas.Contains(removeInteractable))
			_InteractableAreas.Remove(removeInteractable);
	}

	// 상호작용을 시도합니다.
	/// - 만약 상호작용 가능한 객체가 존재하지 않을 경우, 상호작용이 이루어지지 않습니다.
	public void TryInteraction()
	{
		if (_InteractableAreas.Count == 0) return;
		_InteractableAreas[0].onInteractionStarted?.Invoke();
	}



}
