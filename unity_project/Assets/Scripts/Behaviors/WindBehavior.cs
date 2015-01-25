using UnityEngine;
using System.Collections;

public delegate void WindCallback(GameObject operand, Vector3 wind);

public class WindBehavior : Behavior
{
	public Vector3 mWindVelocity;
	protected WindCallback mCallback;

	public WindBehavior(string name, GameObject operand, WindCallback callback, Vector3 velocity)
		: base(name, operand)
	{
		mCallback = callback;
		mWindVelocity = velocity;
	}
	
	public override bool Operate(float signal)
	{
		if (signal > 0.0f)
		{
			mCallback(mOperand, mWindVelocity * signal);
			return true;
		}
		return false;
	}
}

public class DynamicWindBehavior : WindBehavior
{
	private float  mWindTimer = 0.0f;

	public DynamicWindBehavior(string name, GameObject operand, WindCallback callback)
		: base(name, operand, callback, new Vector3(0,0,0))
	{
	}
	
	public override bool Operate(float signal)
	{
		if (base.Operate(signal))
		{
			mWindTimer -= Time.fixedDeltaTime;
			if (mWindTimer <= 0.0f) {
				mWindTimer = (Random.value * 3.0f) + 2.0f;

				mWindVelocity = new Vector3(Random.value * 1.0f, 0.0f, Random.value * 1.0f);
			}
			return true;
		}
		return false;
	}

	public override Behavior GenerateRecordedBehavior()
	{
		Behavior generated_behavior = new WindBehavior("auto generate wind behavior", mOperand, mCallback, mWindVelocity);
		
		return generated_behavior;
	}
}
