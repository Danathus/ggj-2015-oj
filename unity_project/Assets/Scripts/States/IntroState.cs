using UnityEngine;
using System.Collections;

public class IntroState: State {

	public IntroState() {
		mName = "Intro";
	}

	public override void Enter() {
		ScenarioManager.Instance.Shuffle();
		if (!ScenarioManager.Instance.isInitialized()) {
			Application.LoadLevel("main_menu");
		} else {
			ScenarioManager.Instance.ActivateState("Play");
		}
	}

	public override void Leave() {

	}
	
	public override void Update () {
		if (Input.GetKey(KeyCode.Return)) {
			ScenarioManager.Instance.ActivateState("Play");
		}
	}
}
