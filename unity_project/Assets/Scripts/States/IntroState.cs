using UnityEngine;
using System.Collections;

public class IntroState: State {

	public IntroState() {
		mName = "Intro";
	}

	public override void Enter() {
		ScenarioManager.Instance.Shuffle();
	}

	public override void Leave() {

	}
	
	public override void Update () {
		ScenarioManager.Instance.ActivateState("Play");
	}
}
