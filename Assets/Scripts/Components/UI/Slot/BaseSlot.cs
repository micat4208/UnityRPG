using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseSlot : 
	MonoBehaviour, 
	IBeginDragHandler, 
	IDragHandler, 
	IEndDragHandler, 
	IPointerEnterHandler, 
	IPointerExitHandler
{
	[SerializeField] private TextMeshProUGUI _TMP_Count;
	[SerializeField] private Image _Image_Slot;

	protected ScreenInstance m_ScreenInstance;

	protected Button m_Button_Slot;

	// 슬롯 타입을 나타냅니다.
	protected SlotType m_SlotType;

	// 아이템 슬롯과 함께 사용되는 코드를 나타냅니다.
	protected string m_InCode;

	public Image slotImage => _Image_Slot;

	// 드래깅을 사용할 것인지를 나타냅니다.
	protected bool m_UseDragDrop = false;

	// 드래깅이 시작되었을 경우 발생하는 이벤트
	public event System.Action<DragDropOperation, SlotDragVisual> onSlotBeginDragEvent;
	/// - DragDropOperation : 드래그 드랍 작업 객체가 전달됩니다.
	/// - SlotDragVisual : 드래그 비쥬얼 객체가 전달됩니다.


	// 슬롯 드래그 시작 시 발생하는 이벤트
	public event System.Action onSlotDragStarted;
	// 슬롯 드래그 드랍 시 발생하는 이벤트
	public event System.Action onSlotDragFinished;
	// 슬롯 드래그 취소 시 발생하는 이벤트
	public event System.Action onSlotDragCancelled;
	/// 이벤트
	/// - 선언된 클래스 내부에서만 호출할 수 있는 Multicast Delegate 입니다.
	/// - 이벤트의 접근자를 구성할 때에는, property 를 구성할 때와 조금 다르게
	///   get; set; 대신 add, remove 를 사용합니다.
	/// 
	/// 대리자와 다른 점
	/// - 대리자는 public 으로 선언되어 있다면 클래스 외부에서도 호출할 수 있지만
	/// - 이벤트는 public 으로 선언되어 있어도 클래스 외부에서는 호출할 수 없습니다.
	/// - 이벤트는 인터페이스 에서도 멤버로 선언될 수 있지만, 대리자는 인터페이스의 멤버로 선언될 수 없습니다.
	/// - 이벤트와 대리자는 콜백 용도로 사용된다는 것은 동일하나
	///   이벤트는 객체 자신의 상태 변화나, 특정한 사건의 발생을 다른 객체에게 알리는 용도로 사용합니다.

	// 투명한 이미지를 나타냅니다.
	protected static Texture2D m_T_NULL;

	// 드래깅 비쥬얼 프리팹을 나타냅니다.
	private static SlotDragVisual _SlotDragVisualPrefab;

	protected virtual void Awake()
	{
		if (!m_T_NULL)
		{
			m_T_NULL = ResourceManager.Instance.LoadResource<Texture2D>(
				"m_T_NULL", "Image/Slot/T_NULL");
		}

		if (!_SlotDragVisualPrefab)
		{
			_SlotDragVisualPrefab = ResourceManager.Instance.LoadResource<GameObject>(
				"SlotDragVisual",
				"Prefabs/UI/Slot/SlotDragVisual").GetComponent<SlotDragVisual>();
		}

		m_ScreenInstance = PlayerManager.Instance.playerController.screenInstance;

		m_Button_Slot = GetComponent<Button>();

		SetSlotItemCount(0);
	}

	// 슬롯을 초기화합니다.
	public virtual void InitializeSlot(SlotType slotType, string inCode)
	{
		m_SlotType = slotType;
		m_InCode = inCode;
	}


	// 슬롯에 표시되는 숫자를 설정합니다.
	/// - itemCount : 표시시킬 아이템 개수를 전달합니다.
	/// - visibleLessThan2 : 2 개 미만일 경우에도 숫자를 표시할 것인지를 전달합니다.
	public void SetSlotItemCount(int itemCount, bool visibleLessThan2 = false) =>
		_TMP_Count.text = (itemCount >= 2 || visibleLessThan2) ? itemCount.ToString() : "";

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		// 드래그 드랍을 사용하지 않는다면 실행하지 않습니다.
		if (!m_UseDragDrop) return;

		// 드래깅 작업을 끝냅니다.
		m_ScreenInstance.FinishDragDropOperation();
	}

	// OnEndDrag, OnBeginDrag 사용을 위해 추가됨.
	void IDragHandler.OnDrag(PointerEventData eventData) { }
	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
	{
		// 드래그 드랍을 사용하는 경우에만 실행합니다.
		if (m_UseDragDrop)
		{
			// 드래그 비쥬얼을 생성합니다.
			SlotDragVisual dragVisual = Instantiate(_SlotDragVisualPrefab, m_ScreenInstance.rectTransform);

			// 드래그 드랍 작업을 시작합니다.
			m_ScreenInstance.StartDragDropOperation(new DragDropOperation(this, dragVisual.rectTransform));

			// 드래그 시작을 알립니다.
			onSlotBeginDragEvent?.Invoke(m_ScreenInstance.dragDropOperation, dragVisual);
		}
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		// 드래그 드랍을 사용하지 않는다면 실행하지 않도록 합니다.
		if (!m_UseDragDrop) return;

		//Debug.Log("OnPointerEnter!");

		// UI 영역과 겹침
		m_ScreenInstance.dragDropOperation?.OnPointerEnter(this);
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		// 드래그 드랍을 사용하지 않는다면 실행하지 않도록 합니다.
		if (!m_UseDragDrop) return;

		//Debug.Log("OnPointerExit!");
		// UI 영역 겹침 끝
		m_ScreenInstance.dragDropOperation?.OnPointerExit(this);
	}
}