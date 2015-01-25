using UnityEngine;
using System.Collections;

public abstract class Scenario: MonoBehaviour {

	public float m_TimeLimit = 10.0f;
	public ControlScheme mControls = new ControlScheme();

	// Use this for initialization
	void Start() {
		ScenarioManager.Instance.ShowTimeLimit();
	}
	
	// Update is called once per frame
	public void ScenarioUpdate() {
		if (!ScenarioManager.Instance.isInitialized()) {
			mControls.Update();
		}

		if (m_TimeLimit > -1) {
			m_TimeLimit -= Time.fixedDeltaTime;

			ScenarioManager.Instance.SetTimeRemaining(m_TimeLimit);

			if (m_TimeLimit <= 0) {
				Failure();
			}
		}
	}

	public void Victory() {
		ScenarioManager.Instance.HideTimeLimit();
		m_TimeLimit = -1;
		if (ScenarioManager.Instance.CurrentState() == "Play") {
			ScenarioManager.Instance.ActivateState("Victory");
		}
	}

	public void Failure() {
		ScenarioManager.Instance.HideTimeLimit();
		m_TimeLimit = -1;
		if (ScenarioManager.Instance.CurrentState() == "Play") {
			ScenarioManager.Instance.ActivateState("Failure");
		}
	}

	// public void BeginReplay() {
	// 	GameObject replayCamera = GameObject.FindWithTag("ReplayCamera");
	// 	if (replayCamera != null) {
	// 		camera.enabled = false;
	// 		replayCamera.GetComponent<Camera>().enabled = true;
	// 	}
	// 	if (ScenarioManager.Instance.CurrentState() == "Play") {
	// 		ScenarioManager.Instance.ActivateState("Replay");
	// 	}
	// }

	public abstract void Reset();
}
