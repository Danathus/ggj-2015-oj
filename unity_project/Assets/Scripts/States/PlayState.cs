using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayState: State {

	private Scenario[] mScenarioList = null;

	public PlayState() {
		mName = "Play";
	}

	public override void Enter() {
		ReplayManager.Instance.Clear();
		ScenarioManager.Instance.NextScenario();
		// keep track in the replay manager of when we started
		ReplayManager.Instance.AddEvent(new BookendEvent());
	}

	public override void Leave() {
		if (mScenarioList != null) {
			mScenarioList = null;
		}
		// keep track in the replay manager of when we stopped
		//ReplayManager.Instance.AddEvent(new BookendEvent()); // now done in the next state
	}
	
	public override void Update () {
		if (mScenarioList == null) {
			mScenarioList = Object.FindObjectsOfType<Scenario>();
		}

		foreach (var scenario in mScenarioList) {
			scenario.mControls.Update();
		}

		if (Input.GetKey(KeyCode.Return)) {
			ScenarioManager.Instance.ActivateState("Play");
		}
	}
}
