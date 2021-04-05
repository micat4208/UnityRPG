using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Npc : MonoBehaviour
{
	[Header("Npc Code 를 나타냅니다.")]
	[SerializeField] private string _NpcCode;

	// Npc 정보를 나타냅니다.
	private NpcInfo _NpcInfo;

	public InteractableArea interactableArea { get; private set; }



	private void Awake()
	{
		interactableArea = GetComponentInChildren<InteractableArea>();
		InitializeNpc();

		interactableArea.onInteractionStarted += 
			() => Debug.Log("상호작용!");
	}

	// Npc 를 초기화합니다.
	private void InitializeNpc()
	{
		bool fileNotFound;
		_NpcInfo = ResourceManager.Instance.LoadJson<NpcInfo>($"NpcInfo/{_NpcCode}.json", out fileNotFound);
		
#if UNITY_EDITOR
		if (fileNotFound) Debug.LogError($"{_NpcCode}.json not Found");
#endif
	}




}
