using UnityEngine;
using System.Collections;

public class IntroState: State {

	public IntroState() {
		mName = "Intro";
	}

	public override void Enter() {
		ScenarioManager.Instance.Shuffle();
		if (ScenarioManager.Instance.isInitialized()) {
			Application.LoadLevel("main_menu");
			ScenarioManager.Instance.StartFirstRound();
		} else {
			ScenarioManager.Instance.ActivateState("Play");
		}
	}

	public override void Leave() {

	}
	
	public override void Update () {
		if (base.ShouldAdvanceState()) {
			ScenarioManager.Instance.NextScenario();
		}

		// optional
		//TestGamepads();
	}

	void TestGamepads()
	{
		//Debug.Log("controllers: " +
		//	GamepadInput.GamePad.GetJoystickName(GamepadInput.GamePad.Index.One)
		//	+ " and " +
		//	GamepadInput.GamePad.GetJoystickName(GamepadInput.GamePad.Index.Two));
		GamepadInput.GamepadState state = GamepadInput.GamePad.GetState(GamepadInput.GamePad.Index.One);
		if (state.A)             { Debug.Log("1 A"); }
		if (state.B)             { Debug.Log("1 B"); }
		if (state.X)             { Debug.Log("1 X"); }
		if (state.Y)             { Debug.Log("1 Y"); }
		if (state.Start)         { Debug.Log("1 Start"); }
		if (state.Back)          { Debug.Log("1 Back"); }
		if (state.Left)          { Debug.Log("1 Left"); }
		if (state.Right)         { Debug.Log("1 Right"); }
		if (state.Up)            { Debug.Log("1 Up"); }
		if (state.Down)          { Debug.Log("1 Down"); }
		if (state.LeftStick)     { Debug.Log("1 LeftStick"); }
		if (state.RightStick)    { Debug.Log("1 RightStick"); }
		if (state.RightShoulder) { Debug.Log("1 RightShoulder"); }
		if (state.LeftShoulder)  { Debug.Log("1 LeftShoulder"); }

		GamepadInput.GamepadState state2 = GamepadInput.GamePad.GetState(GamepadInput.GamePad.Index.Two);
		if (state2.A)             { Debug.Log("2 A"); }
		if (state2.B)             { Debug.Log("2 B"); }
		if (state2.X)             { Debug.Log("2 X"); }
		if (state2.Y)             { Debug.Log("2 Y"); }
		if (state2.Start)         { Debug.Log("2 Start"); }
		if (state2.Back)          { Debug.Log("2 Back"); }
		if (state2.Left)          { Debug.Log("2 Left"); }
		if (state2.Right)         { Debug.Log("2 Right"); }
		if (state2.Up)            { Debug.Log("2 Up"); }
		if (state2.Down)          { Debug.Log("2 Down"); }
		if (state2.LeftStick)     { Debug.Log("2 LeftStick"); }
		if (state2.RightStick)    { Debug.Log("2 RightStick"); }
		if (state2.RightShoulder) { Debug.Log("2 RightShoulder"); }
		if (state2.LeftShoulder)  { Debug.Log("2 LeftShoulder"); }

		Debug.Log(state.LeftStickAxis);
	}
}
