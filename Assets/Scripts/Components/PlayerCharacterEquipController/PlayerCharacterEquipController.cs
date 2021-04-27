using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 파츠 장착을 제어하는 컴포넌트입니다.
/// - 모든 파츠는 특정한 부모 오브젝트 하위로 추가되어야 합니다.
/// 
/// + Head : 얼굴, 헤어, 머리, 헬멧
/// + Back : 가방
/// + L Hand : 보조 무기
/// + R Hand : 주무기
/// 
public sealed class PlayerCharacterEquipController : MonoBehaviour
{

	// 파츠가 장착될 부모 트랜스폼을 나타냅니다.
	/// - key : 장착될 파츠 타입을 나타냅니다.
	/// - value : 파츠의 부모 트랜스폼 나타냅니다.
	private Dictionary<PartsType, Transform> _PartsSockets = new Dictionary<PartsType, Transform>();

	// 장착된 파츠 오브젝트들을 저장합니다.
	private Dictionary<PartsType, GameObject> _PartsObject = new Dictionary<PartsType, GameObject>();
	/// - 오브젝트 추가와 제거를 편하게 하기 위해 사용됩니다.

	// 플레이어 캐릭터 객체
	private PlayerableCharacter _PlayerCharacter;


	private void Start()
	{
		_PlayerCharacter = GetComponent<PlayerableCharacter>();

		InitializePartsObject();
		InitializeSockets();


		// Mesh 갱신
		foreach(var equipItemInfo in (PlayerManager.Instance.playerController as PlayerController).playerCharacterInfo.partsInfo)
			EquipMesh(equipItemInfo);
	}

	// _PartsObject 의 요소를 미리 생성합니다.
	private void InitializePartsObject()
	{
		// 생성된 모든 파츠를 제거합니다.
		if (_PartsObject.ContainsKey(PartsType.Body))
			Destroy(_PartsObject[PartsType.Body]);
		_PartsObject.Clear();

		foreach(int value in System.Enum.GetValues(typeof(PartsType)))
		{
			PartsType partsType = (PartsType)value;
			_PartsObject.Add(partsType, null);
		}
	}

	// 장비가 장착될 소켓들을 찾습니다.
	private void InitializeSockets()
	{
		// Body 오브젝트가 장착된다면 모든 파츠의 부모 오브젝트가 변경되며
		// 이때 Socket 들을 새로 갱신해야 하기 때문에 모든 요소를 비웁니다.
		if (_PartsSockets.Count != 0)
			_PartsSockets.Clear();

		// 각 파츠의 부모 오브젝트로 사용될 오브젝트의 이름을 저장합니다.
		List<(PartsType partsType, string name)> partsDataToFind = new List<(PartsType, string)>();
		partsDataToFind.Add((PartsType.Face, "+ Head"));
		partsDataToFind.Add((PartsType.Hair, "+ Head"));
		partsDataToFind.Add((PartsType.Head, "+ Head"));
		partsDataToFind.Add((PartsType.Helmet, "+ Head"));

		partsDataToFind.Add((PartsType.Backpack, "+ Back"));

		partsDataToFind.Add((PartsType.LeftWeapon, "+ L Hand"));
		partsDataToFind.Add((PartsType.RightWeapon, "+ R Hand"));

		// 모든 파츠의 부모 오브젝트를 찾습니다.
		foreach (var partsData in partsDataToFind)
			FindParentTransform(transform, partsData.partsType, partsData.name);
	}

	// 파츠가 장착될 때 부모 오브젝트로 사용될 Transform 을 찾습니다.
	/// - parent : 탐색의 기준이되는 부모 오브젝트를 전달합니다.
	///   ex) parent
	///			child1
	///			child2
	///	  에서 child1 을 찾을 때 parent 가 전달되어야 합니다.
	///	- partsType : 부모 오브젝트 사용될 파츠 타입을 전달합니다.
	///	- name : 찾을 부모 오브젝트를 전달합니다.
	private void FindParentTransform(Transform parent, PartsType partsType, string name)
	{
		// 탐색의 기준이 되는 부모 오브젝트의 모든 자식 오브젝트를 확인합니다.
		for (int i = 0; i < parent.childCount; ++i)
		{
			/// - transform.childCount : transform 의 자식 오브젝트 개수를 나타냅니다.
			///   이때 transform 의 자식 오브젝트가 갖는 자식 오브젝트의 개수는 포함시키지 않습니다.
			///   ex) child1
			///         child_1
			///				child_1_1
			///				child_1_2
			///         child_2
			///				child_2_1
			///				child_2_2
			///	   위와 같은 경우 child1 의 자식 오브젝트 개수 : 2
			
			Transform childTransform = parent.GetChild(i);
			/// - transform.GetChild(index) : transform 의 index 번째 자식 오브젝트를 얻습니다.
			
			// 자식 오브젝트의 이름이 찾고자 하는 부모 오브젝트와 일치하는지 확인합니다.
			if (childTransform.gameObject.name == name)
			{
				Debug.Log($"partsType = {partsType}");
				Debug.Log($"childTransform = {childTransform}");
				_PartsSockets.Add(partsType, childTransform);
				break;
			}
			// 찾고자 하는 오브젝트를 찾지 못했을 경우 자식 오브젝트 찾고자 하는 오브젝트가 존재하는지 확인합니다.
			else FindParentTransform(childTransform, partsType, name);
		}

	}

	// 장비 아이템을 장착시킵니다.
	/// - newEquipItemInfo : 장착시킬 장비 아이템 정보를 전달합니다.
	private void EquipMesh(EquipItemInfo newEquipItemInfo)
	{

		Debug.Log($"newEquipItemInfo.partsType = {newEquipItemInfo.partsType}");

		ref var playerInfo = ref (PlayerManager.Instance.playerController as PlayerController).playerCharacterInfo;

		// 바디 파츠가 장착된 경우
		if (newEquipItemInfo.partsType == PartsType.Body)
		{
			// 캐릭터 전체 Mesh 를 제거합니다.
			InitializePartsObject();

			// 바디 오브젝트를 생성합니다.
			var partsObj = Instantiate(
				ResourceManager.Instance.LoadResource<GameObject>(
					"", newEquipItemInfo.meshPrefabPath, false), transform);

			// 파츠 정보를 갱신시킵니다.
			_PartsObject[newEquipItemInfo.partsType] = partsObj;

			// 캐릭터가 제어하는 애니메이터 설정
			_PlayerCharacter.animController.controlledAnimator =
				_PartsObject[newEquipItemInfo.partsType].GetComponent<Animator>();

			// 소켓들을 찾습니다.
			InitializeSockets();

			// 모든 파츠 재생성
			foreach(EquipItemInfo info in playerInfo.partsInfo)
			{
				// 정보가 비어있다면 생성 취소
				if (info.IsEmpty()) continue;

				// Body 파츠는 이미 생성되었기 때문에 제외합니다.
				if (info.partsType != PartsType.Body)
					EquipMesh(info);
			}
		}
		// 장착되는 파츠가 Body 가 아닌 경우
		else
		{
			Debug.Log("_PartsObject.count = " + _PartsObject.Count);

			// 이미 장착된 파츠가 있다면 제거합니다.
			if (_PartsObject[newEquipItemInfo.partsType] != null)
			{
				Destroy(_PartsObject[newEquipItemInfo.partsType].gameObject);
				_PartsObject[newEquipItemInfo.partsType] = null;
			}

			// 파츠 오브젝트를 생성합니다.
			var partsObj = (newEquipItemInfo.partsType == PartsType.Hair) ? null :
				Instantiate(ResourceManager.Instance.LoadResource<GameObject>(
					"", newEquipItemInfo.meshPrefabPath, false), _PartsSockets[newEquipItemInfo.partsType]);

			// 만약 Hair 파츠인 경우
			if (newEquipItemInfo.partsType == PartsType.Hair)
			{
				// Hair 파츠를 숨길 것인지를 결정합니다.
				bool hideHairParts = false;

				// 사용중인 Helmet 파츠가 존재한다면
				if (_PartsObject[PartsType.Helmet] != null)
				{
					// 현재 장착중인 장비 정보에서 Helmet 파츠 인덱스를 얻습니다.
					int helmetPartsIndex = playerInfo.partsInfo.FindIndex(
						(equippedItemInfo) => equippedItemInfo.partsType == PartsType.Helmet);

					// 사용중인 Helmet 파츠가 Hair 파츠를 숨기는지 확인합니다.
					hideHairParts = playerInfo.partsInfo[helmetPartsIndex].hideHairWhenEquipped;

					// 사용중인 Helmet 파츠가 Half Hair 를 사용한다면
					if (playerInfo.partsInfo[helmetPartsIndex].useHalfHair)
					{
						// HalfHair 파츠 오브젝트를 로드 후 생성합니다.
						partsObj = Instantiate(ResourceManager.Instance.LoadResource<GameObject>(
							"", newEquipItemInfo.halfHairPrefabPath, false), _PartsSockets[newEquipItemInfo.partsType]);
					}
				}

				// 사용중인 Helmet 파츠가 Half Hair 를 사용하지 않는다면
				if (partsObj == null)
				{
					// Hair 파츠 오브젝트를 로드 후 생성합니다.
					partsObj = Instantiate(ResourceManager.Instance.LoadResource<GameObject>(
						"", newEquipItemInfo.meshPrefabPath, false), _PartsSockets[newEquipItemInfo.partsType]);
				}

				// 메터리얼 로드 후 생성합니다.
				var material = Instantiate(ResourceManager.Instance.LoadResource<Material>(
					"", newEquipItemInfo.meterialPath, false));

				// 생성한 메터리얼을 적용시킵니다.
				partsObj.GetComponent<MeshRenderer>().material = material;

				// 사용중인 Helmet 파츠가 Hair 파츠를 숨긴다면
				if (hideHairParts)
					// Hair 파츠 오브젝트 비활성화
					partsObj.SetActive(false);
			}

			// 만약 Helmet 파츠인 경우 Half Hair 사용 여부와 Hair 파츠 숨김 여부를 검사합니다.
			else if (newEquipItemInfo.partsType == PartsType.Helmet)
			{
				// 만약 Half Hair 를 사용한다면
				if (newEquipItemInfo.useHalfHair)
				{
					// 현재 장착중인 장비 정보에서 Hair 파츠 인덱스를 얻습니다.
					int halfHairIndex = playerInfo.partsInfo.FindIndex(
						(equippedItemInfo) => equippedItemInfo.partsType == PartsType.Hair);

					// 사용중인 Hair 파츠 오브젝트가 존재한다면 제거합니다.
					if (_PartsObject[PartsType.Hair] != null)
					{
						Destroy(_PartsObject[PartsType.Hair]);
						_PartsObject[PartsType.Hair] = null;
					}

					// Half Hair 애셋을 로드 후 생성합니다.
					_PartsObject[PartsType.Hair] = Instantiate(
						ResourceManager.Instance.LoadResource<GameObject>(
							playerInfo.partsInfo[halfHairIndex].halfHairPrefabPath));
				}
				// Hair 파츠를 숨겨야 한다면
				else if (newEquipItemInfo.hideHairWhenEquipped)
				{
					// 사용중인 Hair 파츠 오브젝트가 존재한다면
					if (_PartsObject[PartsType.Hair] != null)
					{
						// 오브젝트를 비활성화 시킵니다.
						_PartsObject[PartsType.Hair].SetActive(false);
					}
				}

			}

			// 파츠 정보를 갱신합니다.
			_PartsObject[newEquipItemInfo.partsType] = partsObj;
		}

		// 생성된 파츠의 상대 위치, 회전 설정
		_PartsObject[newEquipItemInfo.partsType].transform.localPosition = newEquipItemInfo.localPosition;
		_PartsObject[newEquipItemInfo.partsType].transform.localEulerAngles = newEquipItemInfo.localEulerAngle;


	}

}
