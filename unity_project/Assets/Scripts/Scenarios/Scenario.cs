using UnityEngine;
using System.Collections;

public abstract class Scenario: MonoBehaviour {

	public float m_TimeLimit = 10.0f;
	public ControlScheme mControls = new ControlScheme();

	// Use this for initialization
	void Start() {
		ScenarioManager.Instance.ShowTimeLimit();
	}
	
	// Update is called once per frame
	public void ScenarioUpdate() {
		// initialize ScenarioManager
		ScenarioManager manager = ScenarioManager.Instance;

		if (m_TimeLimit > -1) {
			m_TimeLimit -= Time.fixedDeltaTime;

			ScenarioManager.Instance.SetTimeRemaining(m_TimeLimit);

			if (m_TimeLimit <= 0) {
				Failure();
			}
		}
	}

	public void SetControlScheme(int player, Behavior up, Behavior down, Behavior left, Behavior right) {
		if (ScenarioManager.Instance.m_PrimaryPlayer == player) {
			mControls.AddControl(new KeyCodeControlSignal(KeyCode.W), up   );
			mControls.AddControl(new KeyCodeControlSignal(KeyCode.S), down );
			mControls.AddControl(new KeyCodeControlSignal(KeyCode.A), left );
			mControls.AddControl(new KeyCodeControlSignal(KeyCode.D), right);
			//   for gamepads
			mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y,  1.0f), up    );
			mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y, -1.0f), down  );
			mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X, -1.0f), left  );
			mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X,  1.0f), right );
		} else {
			mControls.AddControl(new KeyCodeControlSignal(KeyCode.UpArrow),    up   );
			mControls.AddControl(new KeyCodeControlSignal(KeyCode.DownArrow),  down );
			mControls.AddControl(new KeyCodeControlSignal(KeyCode.LeftArrow),  left );
			mControls.AddControl(new KeyCodeControlSignal(KeyCode.RightArrow), right);
			//   for gamepads
			mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y,  1.0f), up    );
			mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y, -1.0f), down  );
			mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X, -1.0f), left  );
			mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X,  1.0f), right );
		}
	}

	public void Victory() {
		ScenarioManager.Instance.HideTimeLimit();
		m_TimeLimit = -1;
		if (ScenarioManager.Instance.CurrentState() == "Play") {
			ScenarioManager.Instance.ActivateState("Victory");
		}
	}

	public void Failure() {
		ScenarioManager.Instance.HideTimeLimit();
		m_TimeLimit = -1;
		if (ScenarioManager.Instance.CurrentState() == "Play") {
			ScenarioManager.Instance.ActivateState("Failure");
		}
	}

	// public void BeginReplay() {
	// 	GameObject replayCamera = GameObject.FindWithTag("ReplayCamera");
	// 	if (replayCamera != null) {
	// 		camera.enabled = false;
	// 		replayCamera.GetComponent<Camera>().enabled = true;
	// 	}
	// 	if (ScenarioManager.Instance.CurrentState() == "Play") {
	// 		ScenarioManager.Instance.ActivateState("Replay");
	// 	}
	// }

	public abstract void Reset();
}
