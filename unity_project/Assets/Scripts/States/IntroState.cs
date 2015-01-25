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
		//*
		GamepadInput.GamepadState state = GamepadInput.GamePad.GetState(GamepadInput.GamePad.Index.One);
		if (state.A)             { Debug.Log("A"); }
		if (state.B)             { Debug.Log("B"); }
		if (state.X)             { Debug.Log("X"); }
		if (state.Y)             { Debug.Log("Y"); }
		if (state.Start)         { Debug.Log("Start"); }
		if (state.Back)          { Debug.Log("Back"); }
		if (state.Left)          { Debug.Log("Left"); }
		if (state.Right)         { Debug.Log("Right"); }
		if (state.Up)            { Debug.Log("Up"); }
		if (state.Down)          { Debug.Log("Down"); }
		if (state.LeftStick)     { Debug.Log("LeftStick"); }
		if (state.RightStick)    { Debug.Log("RightStick"); }
		if (state.RightShoulder) { Debug.Log("RightShoulder"); }
		if (state.LeftShoulder)  { Debug.Log("LeftShoulder"); }
        //public Vector2 LeftStickAxis = Vector2.zero;
        //public Vector2 rightStickAxis = Vector2.zero;
        //public Vector2 dPadAxis = Vector2.zero;

        //public float LeftTrigger = 0;
        //public float RightTrigger = 0;
		//*/
	}
}
