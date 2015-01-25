using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// BEHAVIOR ENTRY POINT ////////////////////////////////////////////////////////

public class boxes : Scenario
{
	public GameObject mPlayer1, mPlayer2;

	// Use this for initialization
	void Start () {
		float speed = 0.1f;

		// create the control scheme that maps inputs to these behaviors
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.W),          new TranslateBehavior("player1 move up",    mPlayer1, new Vector3( 0,  1, 0) * speed));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.S),          new TranslateBehavior("player1 move down",  mPlayer1, new Vector3( 0, -1, 0) * speed));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.A),          new TranslateBehavior("player1 move left",  mPlayer1, new Vector3(-1,  0, 0) * speed));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.D),          new TranslateBehavior("player1 move right", mPlayer1, new Vector3( 1,  0, 0) * speed));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.UpArrow),    new TranslateBehavior("player2 move up",    mPlayer2, new Vector3( 0,  1, 0) * speed));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.DownArrow),  new TranslateBehavior("player2 move down",  mPlayer2, new Vector3( 0, -1, 0) * speed));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.LeftArrow),  new TranslateBehavior("player2 move left",  mPlayer2, new Vector3(-1,  0, 0) * speed));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.RightArrow), new TranslateBehavior("player2 move right", mPlayer2, new Vector3( 1,  0, 0) * speed));
	}
	
	// called once per timestep update (critical: do game state updates here!!!)
	void FixedUpdate()
	{
		AloneUpdate();

		if (Input.GetKey(KeyCode.Space))
		{
			Victory();
		}
	}

	public override void Reset() {
		mPlayer1.transform.position = new Vector3(-1, 0, 0);
		mPlayer2.transform.position = new Vector3( 1, 0, 0);
	}
}

// END OF FILE /////////////////////////////////////////////////////////////////
