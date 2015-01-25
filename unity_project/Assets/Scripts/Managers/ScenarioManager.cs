using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScenarioManager : Singleton<ScenarioManager>
{
	protected ScenarioManager () {}

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
	}

	private void SetupStates() {
		mStates.Add(new IntroState());
		mStates.Add(new PlayState());
		mStates.Add(new ReplayState());

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
		}

		if (mCurrentScenario < m_Scenarios.Count) {
			string scenarioName = m_Scenarios[mCurrentScenario];
			Application.LoadLevel(scenarioName);
		}
	}
}