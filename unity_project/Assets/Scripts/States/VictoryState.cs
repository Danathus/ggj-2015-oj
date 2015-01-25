using UnityEngine;
using System.Collections;

public class VictoryState: ReviewState {
	public VictoryState(): base() {
		mName = "Victory";
	}

	public override void Enter() {
		ScenarioManager.Instance.ShowVictory();
		base.Enter();
	}

	public override void Leave() {
		ScenarioManager.Instance.HideVictory();
		base.Leave();
	}
}
