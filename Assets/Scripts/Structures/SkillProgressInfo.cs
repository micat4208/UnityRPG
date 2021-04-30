

public struct SkillProgressInfo
{
	// 진행중인 스킬 코드
	public string progressSkillCode;

	// 스킬 콤보
	public int skillCombo;

	// 현재 스킬 범위 인덱스
	public int currentSkillRandeIndex;

	public SkillProgressInfo(
		string progressSkillCode, 
		int skillCombo = 0, 
		int currentSkillRandeIndex = 0)
	{
		this.progressSkillCode = progressSkillCode;
		this.skillCombo = skillCombo;
		this.currentSkillRandeIndex = currentSkillRandeIndex;
	}

	// 콤보 카운트를 증가시킵니다.
	public void AddCombo() => ++skillCombo;

	// 콤보 카운트를 리셋합니다.
	public void ResetCombo() => skillCombo = 0;
}