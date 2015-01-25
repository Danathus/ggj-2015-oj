using UnityEngine;
using System.Collections;

public class ReviewState: State {
	float mCountdownToReplay;

	public ReviewState() {
		mName = "Review";
	}

	public override void Enter() {
		mCountdownToReplay = 3.0f;
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

	// called at FixedUpdate()
	public override void Update () {
		if (!ReplayManager.Instance.mIsReplaying) {
			mCountdownToReplay -= Time.fixedDeltaTime;
			if (mCountdownToReplay <= 0.0f) {
				bool updatedSomeScenario = false;
				Scenario[] scenarioList = Object.FindObjectsOfType<Scenario>();
				foreach (var scenario in scenarioList) {
					scenario.Reset();
					updatedSomeScenario = true;
				}
				if (updatedSomeScenario) {
					Debug.Log("Replay!");
					ReplayManager.Instance.AddEvent(new BookendEvent()); // add a bookend first to mark end
					ReplayManager.Instance.Play();
				}
			}
		}

		if (Input.GetKey(KeyCode.Return)) {
			ScenarioManager.Instance.ActivateState("Play");
		}
	}
}
