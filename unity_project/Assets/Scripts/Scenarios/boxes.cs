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
		Behavior p1Up    = new TranslateBehavior("player1 move up",    mPlayer1, new Vector3( 0,  1, 0) * speed);
		Behavior p1Down  = new TranslateBehavior("player1 move down",  mPlayer1, new Vector3( 0, -1, 0) * speed);
		Behavior p1Left  = new TranslateBehavior("player1 move left",  mPlayer1, new Vector3(-1,  0, 0) * speed);
		Behavior p1Right = new TranslateBehavior("player1 move right", mPlayer1, new Vector3( 1,  0, 0) * speed);
		Behavior p2Up    = new TranslateBehavior("player2 move up",    mPlayer2, new Vector3( 0,  1, 0) * speed);
		Behavior p2Down  = new TranslateBehavior("player2 move down",  mPlayer2, new Vector3( 0, -1, 0) * speed);
		Behavior p2Left  = new TranslateBehavior("player2 move left",  mPlayer2, new Vector3(-1,  0, 0) * speed);
		Behavior p2Right = new TranslateBehavior("player2 move right", mPlayer2, new Vector3( 1,  0, 0) * speed);		

		// create the control scheme that maps inputs to these behaviors
		SetControlScheme(0, p1Up, p1Down, p1Left, p1Right);
		SetControlScheme(1, p2Up, p2Down, p2Left, p2Right);
	}
	
	// called once per timestep update (critical: do game state updates here!!!)
	void FixedUpdate()
	{
		ScenarioUpdate();

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
