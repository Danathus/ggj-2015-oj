using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct ScenarioData {
	public string scenarioName;
	public int someOtherValue;
}

// a scenario includes an ordered set of behaviors that can be done
class Scenario
{
	List<Behavior> mBehaviors = new List<Behavior>();
	Dictionary<string, Behavior> mNameToBehavior = new Dictionary<string, Behavior>();

	public void AddBehavior(Behavior behavior)
	{
		mBehaviors.Add(behavior);
		mNameToBehavior[behavior.mName] = behavior;
	}
	public Behavior GetBehavior(string name)
	{
		return mNameToBehavior[name];
	}
}

public class ScenarioManager : Singleton<ScenarioManager>
{
	protected ScenarioManager () {}

	public ControlScheme mControls = new ControlScheme();

	public List<ScenarioData> m_Scenarios = new List<ScenarioData>();
	private int mCurrentScenario = -1;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);

		Shuffle();
	}

	// called once per timestep update (critical: do game state updates here!!!)
	void FixedUpdate()
	{
		if (!ReplayManager.Instance.mIsReplaying) {
			mControls.Update();
		}

		if (Input.GetKey(KeyCode.Return)) {
			NextScenario();
		}
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
			mControls.Clear();
			ReplayManager.Instance.Clear();
		}
	}
}