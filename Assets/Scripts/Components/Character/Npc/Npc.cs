using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Npc : MonoBehaviour
{
	[Header("Npc Code")]
	[SerializeField] private string _NpcCode;

	[Header("상호작용 뷰 타깃")]
	[SerializeField] private Transform _ViewTarget;

	// HUD_NpcDialog 프리팹을 나타냅니다.
	private NpcDialog _HUD_NpcDialogPrefab;

	// Npc 정보를 나타냅니다.
	private NpcInfo _NpcInfo;

	public ref NpcInfo npcInfo => ref _NpcInfo;
	public InteractableArea interactableArea { get; private set; }




	private void Awake()
	{
		_HUD_NpcDialogPrefab = ResourceManager.Instance.LoadResource<GameObject>(
			"HUD_NpcDialog",
			"Prefabs/UI/HUD/NpcDialog/HUD_NpcDialog").GetComponent<NpcDialog>();

		interactableArea = GetComponentInChildren<InteractableArea>();
		InitializeNpc();

		interactableArea.onInteractionStarted +=
			() =>
			{
				var playerCharacter = PlayerManager.Instance.playerController.playerableCharacter as PlayerableCharacter;
				var gameScreenInstance = (PlayerManager.Instance.playerController.screenInstance as GameScreenInstance);

				// FadeOut 애니메이션 재생
				gameScreenInstance.effectController.PlayAnimation(ScreenEffectType.ScreenFadeOut);

				// NpcDialog 생성
				var npcDialog = gameScreenInstance.CreateChildHUD(_HUD_NpcDialogPrefab);

				// 생성한 HUD 를 화면에 맞춥니다.
				npcDialog.rectTransform.offsetMin = 
				npcDialog.rectTransform.offsetMax = Vector2.zero;

				// NpcDialog 초기화
				npcDialog.InitializeNpcDialog(this);

				// 뷰 타깃을 변경합니다.
				playerCharacter.springArm.SetViewTarget(_ViewTarget);

				// HUD 가 닫힐 때 뷰 타깃을 초기화합니다.
				npcDialog.onDlgClosed += () =>
				{
					// FadeOut 애니메이션 재생
					gameScreenInstance.effectController.PlayAnimation(ScreenEffectType.ScreenFadeOut);

					// 뷰 타깃 초기화
					playerCharacter.springArm.SetViewTarget(null);
				};
			};
	}

	// Npc 를 초기화합니다.
	private void InitializeNpc()
	{
		bool fileNotFound;
		_NpcInfo = ResourceManager.Instance.LoadJson<NpcInfo>("NpcInfo", $"{_NpcCode}.json", out fileNotFound);
		
#if UNITY_EDITOR
		if (fileNotFound) Debug.LogError($"{_NpcCode}.json not Found");
#endif
	}




}
