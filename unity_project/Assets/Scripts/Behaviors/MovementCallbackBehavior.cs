using UnityEngine;
using System.Collections;

public delegate void MovementCallback(GameObject gameObject, Vector3 offset);

public class MovementCallbackBehavior : Behavior
{
	MovementCallback mCallback;
	Vector3 mOffset;

	public MovementCallbackBehavior(string name, GameObject operand, Vector3 offset, MovementCallback callback)
		: base(name, operand)
	{
		mCallback = callback;
		mOffset = offset;
	}
	
	public override bool Operate(float signal)
	{
		if (signal > 0.0f)
		{
			mCallback(mOperand, signal * mOffset);
			return true;
		}
		return false;
	}
}
