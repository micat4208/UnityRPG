

public class PlayerController : PlayerControllerBase
{
	[UnityEngine.SerializeField]
	private PlayerCharacterInfo _PlayerCharacterInfo;
	public ref PlayerCharacterInfo playerCharacterInfo => ref _PlayerCharacterInfo;

	protected override void Awake()
	{
		base.Awake();

		_PlayerCharacterInfo.Initialize();
	}
}