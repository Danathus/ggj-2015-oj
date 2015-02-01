using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetReadyState: State {

	float mCountdown;
	bool mInitialized = false;
	public GetReadyState() {
		mName = "GetReady";
	}

	public override void Enter() {
		if (ScenarioManager.Instance.isInitialized()) {
			Application.LoadLevel("get_ready");
		}
		mCountdown = 3.0f;
		mInitialized = false;
	}

	public override void Leave() {
	}
	
	public override void Update () {
		if (!mInitialized)
		{
			mInitialized = true;
			Text extraText = GameObject.Find("ExtraText").GetComponent<Text>();
			int num_lives_remaining = ScenarioManager.Instance.LivesRemaining();
			extraText.text = num_lives_remaining + (num_lives_remaining == 1 ? " life" : " lives") + " remaining!";
		}
		mCountdown -= Time.deltaTime;
		if (base.ShouldAdvanceState() || mCountdown <= 0.0f) {
			ScenarioManager.Instance.ActivateState("Play");
		}
	}
}
