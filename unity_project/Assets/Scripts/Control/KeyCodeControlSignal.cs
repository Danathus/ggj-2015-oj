using UnityEngine;
using System.Collections;

public class KeyCodeControlSignal : ControlSignal
{
	KeyCode mKeyCode;
	KeyCode[] mComplements;

	public KeyCodeControlSignal(KeyCode keyCode, KeyCode[] complements = null)
		: base()
	{
		mKeyCode = keyCode;
		mComplements = complements;
	}

	public override float PollSignal()
	{
		bool keyDown = Input.GetKey(mKeyCode);
		float signal = keyDown ? 1.0f : 0.0f;
		if (mComplements != null)
		{
			bool weaken = false;
			foreach (var complement in mComplements)
			{
				if (Input.GetKey(complement))
				{
					weaken = !weaken;
				}
			}
			if (weaken)
			{
				signal /= Mathf.Sqrt(2.0f);
			}
		}
		return signal;
	}
}
