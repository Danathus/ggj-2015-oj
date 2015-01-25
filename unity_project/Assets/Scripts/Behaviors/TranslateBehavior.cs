using UnityEngine;
using System.Collections;

public class TranslateBehavior : Behavior
{
	Vector3 mOffset;
	public TranslateBehavior(string name, GameObject operand, Vector3 offset)
		: base(name, operand)
	{
		mOffset = offset;
	}

	public override bool Operate(float signal)
	{
		if (signal > 0.0f)
		{
			mOperand.transform.position += mOffset * signal;
			return true;
		}
		return false;
	}
}
