using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	#region Move
	// 이동 입력 값을 나타냅니다.
	private Vector3 _InputVector;

	// 목표 속도를 나타냅니다.
	private Vector3 _TargetVelocity;

	// 속도를 나타냅니다.
	private Vector3 _Velocity;
	#endregion

	// 이동 가능 상태를 나타냅니다.
	public bool isMovable { get; private set; } = true;

	// CharacterController 컴포넌트를 나타냅니다.
	public CharacterController characterController { get; private set; }

	private void Awake()
	{
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

		// 속도를 계산합니다.
		CalculateVelocity();

		// 캐릭터를 이동시킵니다.
		MoveCharacter();

		Debug.Log("speed = " + characterController.velocity.magnitude);
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
		}

		// 입력 값을 연산합니다.
		CalculateInputVector();

		// 가속률을 연산합니다.
		CalculateOneSecAcceleration();
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
	



}
