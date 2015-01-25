using UnityEngine;
using System.Collections;

// a behavior is a thing a player can do in the scenario
//   does not define how this behavior is controlled
public abstract class Behavior
{
	public string mName;
	protected GameObject mOperand;

	public Behavior() {}
	public Behavior(string name, GameObject operand)
	{
		mName = name;
		mOperand = operand;
	}

	// input: signal -- degree [0.0, 1.0] to which signal should be operated (0.0 means do nothing, generally)
	// output: true iff should record
	public abstract bool Operate(float signal);
	public virtual Behavior GenerateRecordedBehavior()
	{
		return this;
	}
}
