using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameDrawer : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _TMP_Name;

	public RectTransform rectTransform => transform as RectTransform;

	private INameDrawable _Owner;
	private Camera _Camera;
	private RectTransform _Panel_Parent;


	private void Awake()
	{
		PlayerableCharacter playerableCharacter = PlayerManager.Instance.playerController.
				playerableCharacter as PlayerableCharacter;

		_Camera = playerableCharacter.springArm.camera;

		_Panel_Parent = transform.Find("Panel_Parent").transform as RectTransform;
	}

	private void Update() => Draw();

	private void Draw()
	{
		Vector3 screenPos = _Camera.WorldToViewportPoint(_Owner.drawablePosition);

		screenPos.x *= (Screen.width / GameStatics.screenRatio);
		screenPos.y *= (Screen.height / GameStatics.screenRatio);


		rectTransform.anchoredPosition = screenPos;

		// 카메라 뒤에 위치해 있을 경우 그리지 않습니다.
		_Panel_Parent.gameObject.SetActive(screenPos.z > 0.0);
	}


	public virtual void Initialize(INameDrawable nameDrawable)
	{
		_Owner = nameDrawable;
	}

}
