public class ScoreController
{
	private readonly float comboDecayDuration;

	private float elapsedSinceLastComboUpdate;

	public Observer<int> Score { get; private set; }
	public Observer<int> Combo { get; private set; }

	public ScoreController(float comboDecayDuration)
	{
		this.comboDecayDuration = comboDecayDuration;

		Score = new Observer<int>(0);
		Combo = new Observer<int>(0);
	}

	public void Update(float deltaTime)
	{
		if (Combo <= 0f)
			return;

		elapsedSinceLastComboUpdate += deltaTime;

		// Decay combo
		if (elapsedSinceLastComboUpdate >= comboDecayDuration)
		{
			Combo.Value -= 1;
			elapsedSinceLastComboUpdate = 0f;
		}
	}

	public void Add(int amount)
	{
		// Reset combo decay
		elapsedSinceLastComboUpdate = 0f;
		Combo.Value += 1;

		Score.Value += amount * Combo;
	}
}
