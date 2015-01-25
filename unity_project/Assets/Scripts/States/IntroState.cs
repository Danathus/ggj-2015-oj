using UnityEngine;
using System.Collections;

public class IntroState: State {

	public IntroState() {
		mName = "Intro";
	}

	public override void Enter() {
		ScenarioManager.Instance.Shuffle();
		Application.LoadLevel("main_menu");

		string[] joysticks = Input.GetJoystickNames();
		foreach (string joystick in joysticks)
		{
			//Debug.Log(joystick);
			if (joystick == "Logitech RumblePad 2 USB")
			{
				Debug.Log(joystick + " recognized");
			}
		}
	}

	public override void Leave() {

	}
	
	public override void Update () {
		if (Input.GetKey(KeyCode.Return)) {
			ScenarioManager.Instance.ActivateState("Play");
		}
		if (Input.GetButton("joystick 1 button 0"))
		{
			Debug.Log("yeah");
		}
	}
}
