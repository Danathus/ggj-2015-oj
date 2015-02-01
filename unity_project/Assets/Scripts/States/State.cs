using UnityEngine;
using System.Collections;

public abstract class State {

	public string mName;

	public State() {}
	public abstract void Enter();
	public abstract void Leave();
	public abstract void Update();

	protected bool ShouldAdvanceState()
	{
		return Input.GetKey(KeyCode.Return) ||
			Input.GetKey(KeyCode.Alpha1) ||
			Input.GetKey(KeyCode.Alpha2) ||
			GamepadInput.GamePad.GetButton(
				GamepadInput.GamePad.Button.Start,
				GamepadInput.GamePad.Index.One) ||
			GamepadInput.GamePad.GetButton(
				GamepadInput.GamePad.Button.Start,
				GamepadInput.GamePad.Index.Two);
	}
}
