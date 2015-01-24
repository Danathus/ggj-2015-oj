using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class ControlSignal
{
	public ControlSignal() {}
	public abstract float PollSignal();
}

[System.Serializable]
public class ControlData {

}

public class ControlScheme
{
	List<KeyValuePair<ControlSignal, Behavior> > mBehaviors = new List<KeyValuePair<ControlSignal, Behavior> >();

	public void AddControl(ControlSignal controlSignal, Behavior behavior)
	{
		mBehaviors.Add(new KeyValuePair<ControlSignal, Behavior>(controlSignal, behavior));
	}

	public void Update()
	{
		// Skip user input if we are replaying.
		if (ReplayManager.Instance.mIsReplaying) {
			return;
		}

		foreach (KeyValuePair<ControlSignal, Behavior> key_behavior in mBehaviors)
		{
			ControlSignal controlSignal = key_behavior.Key;
			float signal = controlSignal.PollSignal();

			Behavior behavior = key_behavior.Value;
			bool shouldRecord = behavior.Operate(signal);

			if (shouldRecord)
			{
				BehaviorEvent behavior_event = new BehaviorEvent(behavior.GenerateRecordedBehavior(), signal);
				ReplayManager.Instance.AddEvent(behavior_event);
			}
		}
	}
}
