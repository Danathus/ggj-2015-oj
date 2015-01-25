using UnityEngine;
using System.Collections;

public class KeyCodeControlSignal : ControlSignal
{
	KeyCode mKeyCode;

	public KeyCodeControlSignal(KeyCode keyCode)
		: base()
	{
		mKeyCode = keyCode;
	}

	public override float PollSignal()
	{
		bool keyDown = Input.GetKey(mKeyCode);
		return keyDown ? 1.0f : 0.0f;
	}
}
