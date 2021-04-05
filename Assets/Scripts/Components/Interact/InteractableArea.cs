using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public sealed class InteractableArea : MonoBehaviour
{
	private PlayerableCharacter _PlayerableCharacter;

	// 상호작용 가능 영역을 나타냅니다.
	public SphereCollider interactableArea { get; private set; }

	// 상호작용 시도 시 호출되는 대리자입니다.
	public System.Action onInteractionStarted { get; set; }

	

	private void Awake()
	{
		interactableArea = GetComponent<SphereCollider>();
		interactableArea.isTrigger = true;
		gameObject.layer = LayerMask.NameToLayer("Interactable");
	}

	private void Start()
	{
		_PlayerableCharacter = (PlayerManager.Instance.playerController.
			playerableCharacter as PlayerableCharacter);
	}

	private void OnTriggerEnter(Collider other)
	{
		_PlayerableCharacter.playerInteract.AddInteractable(this);
	}

	private void OnTriggerExit(Collider other)
	{
		_PlayerableCharacter.playerInteract.RemoveInteractable(this);
	}
}
