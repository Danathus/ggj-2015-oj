using UnityEngine;
using System.Collections;

public class ReviewState: State {

	protected Scenario mScenario = null;

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
