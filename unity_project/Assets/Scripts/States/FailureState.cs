using UnityEngine;
using System.Collections;

public class FailureState: ReviewState {
	public FailureState(): base() {
		mName = "Failure";
	}

	public override void Enter() {
		ScenarioManager.Instance.ShowFailure();
		base.Enter();
	}

	public override void Leave() {
		ScenarioManager.Instance.HideFailure();
		base.Leave();
	}
}
