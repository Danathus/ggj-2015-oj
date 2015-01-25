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
		mStates.Add(new PlayState());
		// mStates.Add(new ReplayState());
		mStates.Add(new VictoryState());
		mStates.Add(new FailureState());

		ActivateState("Intro");
	}

	public string CurrentState() {
		if (mCurrentState != null) {
			return mCurrentState.mName;
		}
		return "";
	}

	public bool ActivateState(string name) {
		for (int i = 0; i < mStates.Count; ++i) {
			if (mStates[i].mName == name) {
				if (mCurrentState != null) {
					mCurrentState.Leave();
				}
				mCurrentState = mStates[i];
				mStates[i].Enter();
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
		mCurrentScenario++;

		if (mCurrentScenario >= m_Scenarios.Count) {
			mCurrentScenario = 0;
			m_PrimaryPlayer ^= 1;
		}

		if (mCurrentScenario < m_Scenarios.Count) {
			string scenarioName = m_Scenarios[mCurrentScenario];
			Application.LoadLevel(scenarioName);
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