using UnityEngine;
using System.Collections;

public class PlayState: State {

	private Scenario mScenario = null;

	public PlayState() {
		mName = "Play";
	}

	public override void Enter() {
		ReplayManager.Instance.Clear();
		ScenarioManager.Instance.NextScenario();
	}

	public override void Leave() {
		if (mScenario != null) {
			mScenario.Reset();
			mScenario = null;
		}
	}
	
	public override void Update () {
		if (mScenario == null) {
			mScenario = Object.FindObjectOfType(typeof(Scenario)) as Scenario;
		}

		if (mScenario != null) {
			mScenario.mControls.Update();
		}

		if (Input.GetKey(KeyCode.Return)) {
			ScenarioManager.Instance.ActivateState("Play");
		}
	}
}
