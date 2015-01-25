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
	}

	public override void Leave() {
		if (mScenarioList != null) {
			foreach(var scenario in mScenarioList)
			{
				scenario.Reset();
			}
			mScenarioList = null;
		}
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
