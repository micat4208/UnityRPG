using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class SlotDragVisual : MonoBehaviour
{
	[SerializeField] private Image _Image_Drag;

	public RectTransform rectTransform => transform as RectTransform;

	public void SetDragImageFromSprite(Sprite sprite) =>
		_Image_Drag.sprite = sprite;

	public void SetDragImageFromTexture2D(Texture2D texture2D)
	{
		Rect rc = new Rect(0.0f, 0.0f, texture2D.width, texture2D.height);
		_Image_Drag.sprite = Sprite.Create(texture2D, rc, Vector2.one * 0.5f);
	}


}
