using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Enums;

public sealed class PlayerCharacterMovement : MonoBehaviour
{

	[Header("최대 이동 속력")]
	[SerializeField] private float _MaxSpeed = 6.0f;

	[Header("가속률")] [Range(10.0f, 10000.0f)]
	[Tooltip("1 초에 걸쳐 최대 속력의 몇 퍼센트만큼 가속되도록 할 것인지를 결정합니다.")]
	[SerializeField] private float _OneSecAcceleration = 600.0f;

	[Header("Orient Rotation To Movement")]
	[Tooltip("이동하는 방향으로 회전시킵니다.")]
	[SerializeField] private bool _UseOrientRotationToMovement = true;

	[Header("Yaw 회전 속력")]
	[Tooltip("이동하는 방향으로 회전시킵니다. (UseOrientRotationToMovement 속성을 사용할 때 적용됩니다.)")]
	[SerializeField] private float _RotationYawSpeed = 720.0f;

	[Header("최대 점프 카운트")] [Space(30.0f)]
	[SerializeField] private int _MaxJumpCount = 2;

	[Header("점프 힘")]
	[SerializeField] private float _JumpVelocityY = 10.0f;

	[Header("적용되는 중력 스케일")] [Space(30.0f)]
	[SerializeField] private float _GravityScale = 1.0f;

	[Header("최대 Y 속력")]
	[SerializeField] private float _MaxYVelocity = 10.0f;

	[Header("지형 검사 시 무시할 레이어")]
	[SerializeField] private LayerMask _IgnoreGroundLayers;

	// 캐릭터 컴포넌트를 나타냅니다.
	private PlayerableCharacter _PlayerableCharacter;

	#region Move
	// 이동 입력 값을 나타냅니다.
	private Vector3 _InputVector;

	// 목표 속도를 나타냅니다.
	private Vector3 _TargetVelocity;

	// 속도를 나타냅니다.
	private Vector3 _Velocity;
	#endregion

	#region Jump
	// 점프키 입력 끝 상태를 나타냅니다.
	private bool _IsJumpInputFinished = true;

	// 남은 점프 카운트
	/// - 0 이라면 점프할 수 없습니다.
	private int _RemainJumpCount;

	// 이전 땅 착지 상태를 나타냅니다.
	private bool _PrevGroundedState;

	#endregion

	// 땅에 닿음 상태를 나타냅니다.
	public bool isGrounded { get; private set; }

	// 이동 가능 상태를 나타냅니다.
	public bool isMovable { get; private set; } = true;

	// 점프 가능 상태를 나타냅니다.
	public bool isJumpable =>
		// 이동 가능 상태이며
		isMovable

		// 땅에 닿아있거나, 남은 점프 카운트가 0 보다 크고
		&& (isGrounded || _RemainJumpCount > 0)

		// 점프 입력이 끝났을 경우 점프할 수 있도록 합니다.
		&& _IsJumpInputFinished;


	// CharacterController 컴포넌트를 나타냅니다.
	public CharacterController characterController { get; private set; }

	// 땅으로 착지 시 호출되는 대리자입니다.
	/// - Param : 남은 점프 카운트
	public System.Action<int> onLanded;

	// 점프 시작 시 호출되는 대리자입니다.
	/// - Param : 남은 점프 카운트
	public System.Action<int> onJumpStarted;

	private void Awake()
	{
		_PlayerableCharacter = GetComponent<PlayerableCharacter>();
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		// 캐릭터를 회전시킵니다.
		OrientRotationToMovement();
	}

	private void FixedUpdate()
	{
		// 이동 입력 값을 초기화합니다.
		_InputVector = Vector3.zero;

		_InputVector.x = InputManager.GetAxis("Horizontal");
		_InputVector.z = InputManager.GetAxis("Vertical");
		_InputVector.Normalize();

		// 이동 입력 값을 카메라 방향으로 변환합니다.
		_InputVector = _PlayerableCharacter.springArm.InputToCameraDirection(_InputVector);

		// 점프 입력 처리를 합니다.
		if (InputManager.GetAction("Jump", ActionEvent.Stay))
			JumpInput();
		else FinishJumpInput();


		// 속도를 계산합니다.
		CalculateVelocity();

		// 중력을 계산합니다.
		CalculateGravity();

		// 캐릭터를 이동시킵니다.
		MoveCharacter();

		// 땅 착지 상태를 갱신합니다.
		UpdateGroundedState();
	}

	// 속도를 계산합니다.
	private void CalculateVelocity()
	{
		// 입력 값을 연산합니다.
		void CalculateInputVector()
		{
			_TargetVelocity.x = _InputVector.x * _MaxSpeed * Time.deltaTime;
			_TargetVelocity.z = _InputVector.z * _MaxSpeed * Time.deltaTime;
		}

		// 가속률을 연산합니다
		void CalculateOneSecAcceleration()
		{
			// 현재 Y 속력을 저장합니다.
			float currentVelocityY = _TargetVelocity.y;

			// 목표 속도를 저장합니다.
			Vector3 currentVelocity = isMovable ? _TargetVelocity : Vector3.zero;

			// 현재 속도를 저장합니다.
			_Velocity = characterController.velocity * Time.deltaTime;

			// 목표 속도와, 현재 속도에서 Y 축 값을 제외합니다.
			currentVelocity.y = _Velocity.y = 0.0f;

			// 가속률을 연산시킵니다.
			_Velocity = Vector3.MoveTowards(
				_Velocity, currentVelocity, _MaxSpeed * 
				(_OneSecAcceleration * 0.01f * Time.deltaTime) * Time.deltaTime);

			// 연산된 속도에 Y 속력을 적용시킵니다.
			_Velocity.y = currentVelocityY;
		}

		// 입력 값을 연산합니다.
		CalculateInputVector();

		// 가속률을 연산합니다.
		CalculateOneSecAcceleration();
	}

	// 중력을 계산합니다.
	private void CalculateGravity()
	{
		// Y 축 속력에 중력을 적용시킵니다.
		_TargetVelocity.y += Physics.gravity.y * _GravityScale * 0.05f * Time.deltaTime;

		// Y 축 속력이 최대 Y 축 이동 속력을 초과하지 않도록 합니다.
		_TargetVelocity.y = Mathf.Clamp(_TargetVelocity.y, -_MaxYVelocity, _MaxYVelocity);

	}

	// 캐릭터를 실제로 이동시킵니다.
	private void MoveCharacter()
	{
		// 캐릭터를 이동시킵니다.
		characterController.Move(_Velocity);
	}

	// 이동하는 방향으로 캐릭터를 회전시킵니다.
	private void OrientRotationToMovement()
	{
		// _UseOrientRotationToMovement 속성을 사용하지 않는다면 실행하지 않습니다.
		if (!_UseOrientRotationToMovement) return;

		// 이동 불가능 상태라면 실행하지 않습니다.
		if (!isMovable) return;

		// 이동이 있을 경우에만 회전이 이루어져야 하므로 이동 입력이 없을 경우 실행하지 않습니다.
		if (_InputVector.magnitude <= characterController.minMoveDistance) return;



		// 현재 회전값을 얻습니다.
		float currentYawRotation = transform.eulerAngles.y;

		// 목표 회전값을 얻습니다.
		float tawrgetYawRotation = _TargetVelocity.ToAngle(castDirVector : true);

		// 다음 회전값을 얻습니다.
		float nextYawRotation = Mathf.MoveTowardsAngle(
			currentYawRotation, tawrgetYawRotation, _RotationYawSpeed * Time.deltaTime);

		// 적용시킬 오일러각을 저장합니다.
		Vector3 newEulerAngle = Vector3.up * nextYawRotation;

		transform.eulerAngles = newEulerAngle;
	}
	
	// 점프 입력을 처리합니다.
	private void JumpInput()
	{
		// 점프 가능 상태일 경우
		if (isJumpable)
		{
			// 남은 점프 카운트가 0 보다 클 경우, 1 감소시킵니다.
			if (_RemainJumpCount > 0) --_RemainJumpCount;

			// Y 이동 속력을 변경합니다.
			_TargetVelocity.y = _JumpVelocityY * Time.deltaTime;

			onJumpStarted?.Invoke(_RemainJumpCount);
		}

		// 점프키가 입력되었으므로, 점프 입력 상태로 설정합니다.
		_IsJumpInputFinished = false;
	}
	// 점프 입력을 끝냅니다.
	private void FinishJumpInput() =>
		_IsJumpInputFinished = true;

	// 땅에 닿음 상태를 갱신합니다.
	private void UpdateGroundedState()
	{
		RaycastHit hit;
		Ray ray = new Ray(
			transform.position + characterController.center,
			Vector3.down);

		// 땅에 닿아 있는지 확인합니다.
		isGrounded =
		// 캐릭터의 레이어가 아닌, 다른 오브젝트가 하단에 존재한다면
		Physics.SphereCast(ray,

			// 발사시키는 구의 반지름을 설정합니다.
			characterController.radius,

			out hit,

			(characterController.center.y) + (characterController.skinWidth * 2.0f) - (characterController.radius),

			~_IgnoreGroundLayers) &&

			// 캐릭터가 상승중이 아닐 경우
			_Velocity.y <= 0.0f;

#if UNITY_EDITOR
		Debug.DrawRay(
			ray.origin,
			ray.direction * (characterController.center.y + (characterController.skinWidth)),
			Color.red);
#endif

		if (isGrounded)
		{
			// 땅에 닿아있다면 Y 축 이동 속력을 0 으로 설정합니다.
			_TargetVelocity.y = 0.0f;
		}

		// 땅에 닿음 상태가 이전 상태와 다를 경우
		if (_PrevGroundedState != isGrounded)
		{
			// 이전 상태가 땅에 닿아있음 상태일 경우
			if (_PrevGroundedState)
			{
				// 남은 점프 카운트가 _MaxJumpCount 와 일치할 경우
				// 지형에서 떨어지는 상태이므로 점프 카운트를 감소시키며, 점프 상태로 설정합니다.
				if ((_RemainJumpCount == _MaxJumpCount) && isMovable)
				{
					// 점프 카운트 1 감소
					--_RemainJumpCount;

					// 하단으로 살짝 이동시킵니다.
					_TargetVelocity.y += Time.deltaTime;

					// 점프 시작 이벤트 발생
					onJumpStarted?.Invoke(_RemainJumpCount);
				}
			}
			// 이전 상태가 땅에 닿지 않음 상태일 경우
			else
			{
				// 착지 이벤트 발생
				onLanded?.Invoke(_RemainJumpCount);

				// 남은 점프 카운트를 초기화합니다.
				_RemainJumpCount = _MaxJumpCount;
			}

			// 상태를 갱신합니다.
			_PrevGroundedState = isGrounded;
		}


	}





}
