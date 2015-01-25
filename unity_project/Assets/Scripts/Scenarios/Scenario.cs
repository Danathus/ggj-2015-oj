using UnityEngine;
using System.Collections;

public abstract class Scenario: MonoBehaviour {

	public float mTimeLimit = 5;
	public ControlScheme mControls = new ControlScheme();

	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	public void AloneUpdate() {
		if (!ScenarioManager.Instance.isInitialized()) {
			mControls.Update();
		}

		if (mTimeLimit > -1) {
			mTimeLimit -= Time.fixedDeltaTime;

			if (mTimeLimit <= 0) {
				Failure();
			}
		}
	}

	public void Victory() {
		if (ScenarioManager.Instance.CurrentState() == "Play") {
			ScenarioManager.Instance.ActivateState("Victory");
		}
	}

	public void Failure() {
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
