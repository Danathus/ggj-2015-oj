using UnityEngine;
using System.Collections;

public abstract class State {

	public string mName;

	public State() {}
	public abstract void Enter();
	public abstract void Leave();
	public abstract void Update();
}
