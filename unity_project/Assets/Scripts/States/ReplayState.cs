using UnityEngine;
using System.Collections;

public class ReplayState: State {

	private Scenario mScenario = null;

	public ReplayState() {
		mName = "Replay";
	}

	public override void Enter() {
		ReplayManager.Instance.Play();
	}

	public override void Leave() {
		ReplayManager.Instance.Stop();
	}
	
	public override void Update () {
		if (!ReplayManager.Instance.mIsReplaying) {
			if (mScenario == null) {
				mScenario = Object.FindObjectOfType(typeof(Scenario)) as Scenario;
			}
		}

		if (base.ShouldAdvanceState()) {
			ScenarioManager.Instance.ActivateState("Play");
		}
	}
}
