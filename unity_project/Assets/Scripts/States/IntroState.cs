using UnityEngine;
using System.Collections;

public class IntroState: State {

	public IntroState() {
		mName = "Intro";
	}

	public override void Enter() {
		ScenarioManager.Instance.Shuffle();
		Application.LoadLevel("main_menu");
	}

	public override void Leave() {

	}
	
	public override void Update () {
		if (Input.GetKey(KeyCode.Return)) {
			ScenarioManager.Instance.ActivateState("Play");
		}
	}
}
