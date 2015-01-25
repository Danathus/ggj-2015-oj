using UnityEngine;
using System.Collections;

public class BehaviorEvent : ReplayEvent
{
	Behavior mBehavior;
	float mSignal;

	private BehaviorEvent() {}
	public BehaviorEvent(Behavior behavior, float signal)
		: base()
	{
		mBehavior = behavior;
		mSignal = signal;
	}

	public override void Activate()
	{
		mBehavior.Operate(mSignal);
	}
}
