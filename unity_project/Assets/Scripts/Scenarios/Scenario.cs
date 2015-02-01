using UnityEngine;
using System.Collections;

public abstract class Scenario: MonoBehaviour {

	private float m_TimeLimit = 10.0f;
	public ControlScheme mControls = new ControlScheme();

	private bool m_Initialized = false;

	// Update is called once per frame
	public void ScenarioUpdate() {
		if (!m_Initialized) {
			m_Initialized = true;

			DifficultyLevel difficultyInfo = ScenarioManager.Instance.GetDifficultyInfo();
			m_TimeLimit = difficultyInfo.TimeLimitInSeconds;
			Debug.Log("*** difficulty level: " + ScenarioManager.Instance.mDifficultyLevel + ", time limit: " + m_TimeLimit);

			ScenarioManager.Instance.ShowTimeLimit();
			ScenarioManager.Instance.UpdatePlayerInstructions();
		}
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
			if (up != null) mControls.AddControl(new KeyCodeControlSignal(KeyCode.W), up   );
			if (down != null) mControls.AddControl(new KeyCodeControlSignal(KeyCode.S), down );
			if (left != null) mControls.AddControl(new KeyCodeControlSignal(KeyCode.A), left );
			if (right != null) mControls.AddControl(new KeyCodeControlSignal(KeyCode.D), right);
			//   for gamepads
			if (up != null) mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y,  1.0f), up    );
			if (down != null) mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y, -1.0f), down  );
			if (left != null) mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X, -1.0f), left  );
			if (right != null) mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X,  1.0f), right );
		} else {
			if (up != null) mControls.AddControl(new KeyCodeControlSignal(KeyCode.UpArrow),    up   );
			if (down != null) mControls.AddControl(new KeyCodeControlSignal(KeyCode.DownArrow),  down );
			if (left != null) mControls.AddControl(new KeyCodeControlSignal(KeyCode.LeftArrow),  left );
			if (right != null) mControls.AddControl(new KeyCodeControlSignal(KeyCode.RightArrow), right);
			//   for gamepads
			if (up != null) mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y,  1.0f), up    );
			if (down != null) mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y, -1.0f), down  );
			if (left != null) mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X, -1.0f), left  );
			if (right != null) mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X,  1.0f), right );
		}
	}

	public void Victory() {
		ScenarioManager.Instance.HideTimeLimit();
		m_TimeLimit = -1;
		if (ScenarioManager.Instance.CurrentState() == "Play") {
			ScenarioManager.Instance.AddVictoryStat();
			ScenarioManager.Instance.ActivateState("Victory");
		}
	}

	public void Failure() {
		ScenarioManager.Instance.HideTimeLimit();
		m_TimeLimit = -1;
		if (ScenarioManager.Instance.CurrentState() == "Play") {
			ScenarioManager.Instance.AddFailStat();
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
