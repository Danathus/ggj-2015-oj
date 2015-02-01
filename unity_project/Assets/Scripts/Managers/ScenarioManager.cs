using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScenarioManager : Singleton<ScenarioManager>
{
	protected ScenarioManager () {}

	public int m_PrimaryPlayer = 0;

	public GameObject m_TimeScreen = null;
	private GameObject mShowingTimeScreen = null;

	public GameObject m_VinetteScreen = null;
	public GameObject m_VictoryScreen = null;
	public GameObject m_FailureScreen = null;
	private GameObject mShowingScreen = null;

	public List<string> m_Scenarios = new List<string>();
	private int mCurrentScenario = -1;

	private List<State> mStates = new List<State>();
	private State mCurrentState = null;

	// difficulty
	public enum DifficultyLevelType { Easy, Medium, Hard, Invalid };
	public DifficultyLevelType mDifficultyLevel = DifficultyLevelType.Invalid;
	public DifficultyLevelType NextDifficultyLevel(DifficultyLevelType level)
	{
		switch (level)
		{
		case DifficultyLevelType.Invalid: return DifficultyLevelType.Easy;
		case DifficultyLevelType.Easy:    return DifficultyLevelType.Medium;
		case DifficultyLevelType.Medium:  return DifficultyLevelType.Hard;
		case DifficultyLevelType.Hard:    return DifficultyLevelType.Easy;
		}
		return DifficultyLevelType.Invalid;
	}
	public DifficultyLevel GetDifficultyInfo()
	{
		string objName = "<invalid>";
		switch (mDifficultyLevel)
		{
		case DifficultyLevelType.Easy:   objName = "Easy";   break;
		case DifficultyLevelType.Medium: objName = "Medium"; break;
		case DifficultyLevelType.Hard:   objName = "Hard";   break;
		}
		DifficultyLevel difficultyObj = GameObject.Find(objName).GetComponent<DifficultyLevel>();
		return difficultyObj;
	}

	// losses
	int mNumLosses = 0;
	public int NumLives = 3;

	// rounds
	private int mCurrentRound = 0;
	public void StartFirstRound()
	{
		mDifficultyLevel = DifficultyLevelType.Easy;
		mCurrentRound = 0;
		mNumLosses = 0;
	}
	public int CurrentRound() { return mCurrentRound; }
	public int mNumTotalRounds = 3; // constant
	public void NextRound()
	{
		++mCurrentRound;
		m_PrimaryPlayer ^= 1;
		Shuffle();

		mDifficultyLevel = NextDifficultyLevel(mDifficultyLevel);
	}

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);

		SetupStates();
	}

	// called once per timestep update (critical: do game state updates here!!!)
	void FixedUpdate()
	{
		if (mCurrentState != null) {
			mCurrentState.Update();
		}

		if (Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}
	}

	public bool isInitialized() {
		return m_Scenarios.Count > 0;
	}

	private void SetupStates() {
		mStates.Add(new IntroState());
		mStates.Add(new GetReadyState());
		mStates.Add(new PlayState());
		// mStates.Add(new ReplayState());
		mStates.Add(new VictoryState());
		mStates.Add(new FailureState());
		mStates.Add(new EndState());
		mStates.Add(new GameLostState());

		ActivateState("Intro");
	}

	public string CurrentState() {
		if (mCurrentState != null) {
			return mCurrentState.mName;
		}
		return "";
	}
	
	public string CurrentScenario() {
		string scenarioName = mCurrentScenario >= 0 && mCurrentScenario < m_Scenarios.Count
			? m_Scenarios[mCurrentScenario]
			: "";
		return scenarioName;
	}

	public bool ActivateState(string name) {
		for (int i = 0; i < mStates.Count; ++i) {
			if (mStates[i].mName == name) {
				if (mCurrentState != null) {
					Debug.Log("Leaving state " + mCurrentState.mName);
					mCurrentState.Leave();
				}
				mCurrentState = mStates[i];
				Debug.Log("Entering state " + mCurrentState.mName);
				mCurrentState.Enter();
				return true;
			}
		}

		return false;
	}

	public void Shuffle() {
		for (int i = 0; i < m_Scenarios.Count; ++i) {
			string scenarioName = m_Scenarios[i];
			int randomIndex = Random.Range(i, m_Scenarios.Count);
			m_Scenarios[i] = m_Scenarios[randomIndex];
			m_Scenarios[randomIndex] = scenarioName;
		}
	}

	public void NextScenario() {

		if (CurrentState() == "End" || CurrentState() == "GameLost") {
			// cycle back to beginning
			ActivateState("Intro");
			return;
		}

		++mCurrentScenario;

		if (mCurrentScenario >= m_Scenarios.Count) {
			if (CurrentRound() == mNumTotalRounds-1) { // end of third round
				mCurrentScenario = -1;
				ActivateState("End");
			}
			else {
				mCurrentScenario = 0;
			}
			NextRound();
		}

		ReplayManager.Instance.Stop();
		ReplayManager.Instance.Clear();

		// if we have failed too many times, game over
		if (mNumLosses >= NumLives) {
			ScenarioManager.Instance.ActivateState("GameLost");
			return;
		}

		if (0 <= mCurrentScenario && mCurrentScenario < m_Scenarios.Count) {
			ScenarioManager.Instance.ActivateState("GetReady");
		}
	}

	public void UpdatePlayerInstructions() {
		GameObject player1Text = GameObject.FindWithTag("Player1Text");
		GameObject player2Text = GameObject.FindWithTag("Player2Text");

		if (player1Text != null) {
			Text text = player1Text.GetComponent<Text>() as Text;
			text.text = text.text.Replace("{{1}}", (m_PrimaryPlayer + 1).ToString());
		}

		if (player2Text != null) {
			Text text = player2Text.GetComponent<Text>() as Text;
			text.text = text.text.Replace("{{2}}", ((m_PrimaryPlayer ^ 1) + 1).ToString());
		}
	}

	public void ShowVinette() {
		HideVinette();
		if (m_VinetteScreen != null) {
			mShowingScreen = Instantiate(m_VinetteScreen, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		}
	}

	public void HideVinette() {
		if (mShowingScreen != null) {
			Destroy(mShowingScreen);
			mShowingScreen = null;
		}
	}

	public void ShowVictory() {
		HideVictory();
		if (m_VictoryScreen != null) {
			mShowingScreen = Instantiate(m_VictoryScreen, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		}
	}

	public void HideVictory() {
		if (mShowingScreen != null) {
			Destroy(mShowingScreen);
			mShowingScreen = null;
		}
	}

	public void ShowFailure() {
		++mNumLosses;
		HideFailure();
		if (m_FailureScreen != null) {
			mShowingScreen = Instantiate(m_FailureScreen, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		}
	}

	public void HideFailure() {
		if (mShowingScreen != null) {
			Destroy(mShowingScreen);
			mShowingScreen = null;
		}
	}

	public void ShowTimeLimit() {
		HideTimeLimit();
		if (m_TimeScreen != null) {
			mShowingTimeScreen = Instantiate(m_TimeScreen, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			Text text = mShowingTimeScreen.GetComponentInChildren<Text>() as Text;
			//text.rectTransform.position = new Vector3(50, 100, 0);
			text.rectTransform.sizeDelta = new Vector2(30, 30);
			text.color = Color.red;
		}
	}

	public void HideTimeLimit() {
		if (mShowingTimeScreen != null) {
			Destroy(mShowingTimeScreen);
			mShowingTimeScreen = null;
		}
	}

	public void SetTimeRemaining(float t) {
		if (mShowingTimeScreen == null) {
			ShowTimeLimit();
		}

		if (mShowingTimeScreen != null) {
			Text text = mShowingTimeScreen.GetComponentInChildren<Text>() as Text;
			if (text != null) {
				text.text = "Time Remaining: " + t.ToString();
				text.text = ((int)t).ToString();
				float scale = 1.0f + Mathf.Repeat(t, 1.0f);
				text.rectTransform.localScale = new Vector3(scale, scale, scale);
			}
		}
	}
}