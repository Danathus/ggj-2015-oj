using UnityEngine;
using System.Collections;

public class GetReadyState: State {

	float mCountdown;
	public GetReadyState() {
		mName = "GetReady";
	}

	public override void Enter() {
		if (ScenarioManager.Instance.isInitialized()) {
			Application.LoadLevel("end_game");
		}
		mCountdown = 3.0f;
	}

	public override void Leave() {
	}
	
	public override void Update () {
		mCountdown -= Time.deltaTime;
		if (base.ShouldAdvanceState() || mCountdown <= 0.0f) {
			ScenarioManager.Instance.ActivateState("Play");
		}
	}
}
