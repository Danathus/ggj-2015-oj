using UnityEngine;
using System.Collections;

public abstract class Scenario: MonoBehaviour {

	public ControlScheme mControls = new ControlScheme();

	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		mControls.Update();
	}

	public void BeginReplay() {
		if (ScenarioManager.Instance.CurrentState() == "Play") {
			ScenarioManager.Instance.ActivateState("Replay");
		}
	}

	public abstract void Reset();
}
