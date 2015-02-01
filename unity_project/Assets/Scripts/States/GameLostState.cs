using UnityEngine;
using System.Collections;

public class GameLostState: State {

	public GameLostState() {
		mName = "GameLost";
	}

	public override void Enter() {
		if (ScenarioManager.Instance.isInitialized()) {
			Application.LoadLevel("you_lose");
		}
	}

	public override void Leave() {
	}

	public override void Update () {
		if (base.ShouldAdvanceState()) {
			ScenarioManager.Instance.NextScenario();
		}
	}
}
