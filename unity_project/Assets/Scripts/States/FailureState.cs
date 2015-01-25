using UnityEngine;
using System.Collections;

public class FailureState: State {

	private Scenario mScenario = null;

	public FailureState() {
		mName = "Failure";
	}

	public override void Enter() {
		ScenarioManager.Instance.ShowFailure();
		ReplayManager.Instance.Play();

		GameObject replayCamera = GameObject.FindWithTag("ReplayCamera");
		if (replayCamera != null) {
			Camera.main.enabled = false;
			replayCamera.GetComponent<Camera>().enabled = true;
		}
	}

	public override void Leave() {
		ScenarioManager.Instance.HideFailure();
		ReplayManager.Instance.Stop();
	}
	
	public override void Update () {
		if (!ReplayManager.Instance.mIsReplaying) {
			if (mScenario == null) {
				mScenario = Object.FindObjectOfType(typeof(Scenario)) as Scenario;
			}

			if (Input.GetKey(KeyCode.Space)) {
				if (mScenario != null) {
					mScenario.Reset();
					ReplayManager.Instance.Play();
				}
			}
		}

		if (Input.GetKey(KeyCode.Return)) {
			ScenarioManager.Instance.ActivateState("Play");
		}
	}
}
