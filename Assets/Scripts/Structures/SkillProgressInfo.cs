

public struct SkillProgressInfo
{
	// 진행중인 스킬 코드
	public string progressSkillCode;

	// 스킬 콤보
	public int skillCombo;

	public SkillProgressInfo(string progressSkillCode, int skillCombo = 0)
	{
		this.progressSkillCode = progressSkillCode;
		this.skillCombo = skillCombo;
	}

	// 콤보 카운트를 증가시킵니다.
	public void AddCombo() => ++skillCombo;

	// 콤보 카운트를 리셋합니다.
	public void ResetCombo() => skillCombo = 0;
}