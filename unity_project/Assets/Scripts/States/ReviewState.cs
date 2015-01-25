using UnityEngine;
using System.Collections;

public class ReviewState: State {

	private Scenario[] mScenarioList = null;

	public ReviewState() {
		mName = "Review";
	}

	public override void Enter() {
		ReplayManager.Instance.Play();

		GameObject replayCamera = GameObject.FindWithTag("ReplayCamera");
		if (replayCamera != null) {
			Camera.main.enabled = false;
			replayCamera.GetComponent<Camera>().enabled = true;
		}
	}

	public override void Leave() {
		ReplayManager.Instance.Stop();
	}
	
	public override void Update () {
		if (!ReplayManager.Instance.mIsReplaying) {
			if (mScenarioList == null) {
				mScenarioList = Object.FindObjectsOfType<Scenario>();
			}

			if (Input.GetKey(KeyCode.Space)) {
				bool updatedSomeScenario = false;
				foreach (var scenario in mScenarioList) {
					scenario.Reset();
					updatedSomeScenario = true;
				}
				if (updatedSomeScenario) {
					ReplayManager.Instance.Play();
				}
			}
		}

		if (Input.GetKey(KeyCode.Return)) {
			ScenarioManager.Instance.ActivateState("Play");
		}
	}
}
