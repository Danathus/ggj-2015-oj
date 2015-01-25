using UnityEngine;
using System.Collections;

public class EndState: State {

	public EndState() {
		mName = "End";
	}

	public override void Enter() {
		if (ScenarioManager.Instance.isInitialized()) {
			Application.LoadLevel("end_game");
		}
	}

	public override void Leave() {
	}
	
	public override void Update () {
	}
}
