using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct ScenarioData {
	public string scenarioName;
	public int someOtherValue;
}

public class ScenarioManager : Singleton<ScenarioManager>
{
	protected ScenarioManager () {}

	public List<ScenarioData> m_Scenarios = new List<ScenarioData>();
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
			ScenarioData data = m_Scenarios[i];
			int randomIndex = Random.Range(i, m_Scenarios.Count);
			m_Scenarios[i] = m_Scenarios[randomIndex];
			m_Scenarios[randomIndex] = data;
		}
	}

	public void NextScenario() {
		mCurrentScenario++;

		if (mCurrentScenario >= m_Scenarios.Count) {
			mCurrentScenario = 0;
		}

		if (mCurrentScenario < m_Scenarios.Count) {
			ScenarioData data = m_Scenarios[mCurrentScenario];
			Application.LoadLevel(data.scenarioName);
		}
	}
}