using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// BEHAVIOR ENTRY POINT ////////////////////////////////////////////////////////

public class Main : MonoBehaviour
{
	private GameObject mPlayer1, mPlayer2, mCamera, mMan, mLeftHand, mRightHand;
	private ControlScheme mControls;

	// Use this for initialization
	void Start () {
		// create mock scene with two player objects
		mPlayer1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		mPlayer2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
		mPlayer1.transform.localScale = scale;
		mPlayer2.transform.localScale = scale;
		// find the camera and man
		mCamera = GameObject.Find("Main Camera");
		mMan = GameObject.Find("Man");
		//RuntimeAnimatorController animCtrl = mMan.AddComponent("RuntimeAnimatorController") as RuntimeAnimatorController;
		//
		//RuntimeAnimatorController anim = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("myAnimation", typeof(RuntimeAnimatorController)));
		AnimatorOverrideController overrideController = new AnimatorOverrideController();
		Animator manimator = mMan.GetComponent<Animator>();
		manimator.runtimeAnimatorController = overrideController;
		//
		mLeftHand = GameObject.FindWithTag("LeftHand");
		mRightHand = GameObject.FindWithTag("RightHand");
		// hook up the IK
		IKControl ik = mMan.GetComponent<IKControl>();
		ik.rightHandObj = mPlayer2.transform;
		ik.ikActive = true;
		// restart the scene
		Restart();

		// // create the behaviors in this scene
		// Scenario scenario = new Scenario();
		// float speed = 0.025f;
		// scenario.AddBehavior(new TranslateBehavior("player1 move up",    mPlayer1, new Vector3( 0,  1, 0) * speed));
		// scenario.AddBehavior(new TranslateBehavior("player1 move down",  mPlayer1, new Vector3( 0, -1, 0) * speed));
		// scenario.AddBehavior(new TranslateBehavior("player1 move left",  mPlayer1, new Vector3(-1,  0, 0) * speed));
		// scenario.AddBehavior(new TranslateBehavior("player1 move right", mPlayer1, new Vector3( 1,  0, 0) * speed));
		// scenario.AddBehavior(new TranslateBehavior("player2 move up",    mPlayer2, new Vector3( 0,  1, 0) * speed));
		// scenario.AddBehavior(new TranslateBehavior("player2 move down",  mPlayer2, new Vector3( 0, -1, 0) * speed));
		// scenario.AddBehavior(new TranslateBehavior("player2 move left",  mPlayer2, new Vector3(-1,  0, 0) * speed));
		// scenario.AddBehavior(new TranslateBehavior("player2 move right", mPlayer2, new Vector3( 1,  0, 0) * speed));

		// // create the control scheme that maps inputs to these behaviors
		// mControls = new ControlScheme();
		// mControls.AddControl(new KeyCodeControlSignal(KeyCode.W),          scenario.GetBehavior("player1 move up"));
		// mControls.AddControl(new KeyCodeControlSignal(KeyCode.S),          scenario.GetBehavior("player1 move down"));
		// mControls.AddControl(new KeyCodeControlSignal(KeyCode.A),          scenario.GetBehavior("player1 move left"));
		// mControls.AddControl(new KeyCodeControlSignal(KeyCode.D),          scenario.GetBehavior("player1 move right"));
		// mControls.AddControl(new KeyCodeControlSignal(KeyCode.UpArrow),    scenario.GetBehavior("player2 move up"));
		// mControls.AddControl(new KeyCodeControlSignal(KeyCode.DownArrow),  scenario.GetBehavior("player2 move down"));
		// mControls.AddControl(new KeyCodeControlSignal(KeyCode.LeftArrow),  scenario.GetBehavior("player2 move left"));
		// mControls.AddControl(new KeyCodeControlSignal(KeyCode.RightArrow), scenario.GetBehavior("player2 move right"));
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	// called once per timestep update (critical: do game state updates here!!!)
	void FixedUpdate()
	{
		// update all the controls
		mControls.Update();

		// left hand -- hard-coded
		mLeftHand.transform.position  = mPlayer1.transform.position;
		// right hand -- via IK system
		//mRightHand.transform.position = mPlayer2.transform.position;
		//Animator manimator = mMan.GetComponent<Animator>();
		//manimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        //manimator.SetIKPosition(AvatarIKGoal.RightHand, mPlayer2.transform.position);

		Camera camera = mCamera.GetComponent<Camera>();
		int headLayerBit = 1 << 8;
		if (Input.GetKey(KeyCode.Q)) // show head
		{
			camera.cullingMask |= headLayerBit; //0xffffffff;
		}
		if (Input.GetKey(KeyCode.E)) // hide head
		{
			camera.cullingMask &= ~headLayerBit; //0xffffffff;
		}

		if (Input.GetKey(KeyCode.Space))
		{
			// start the playback
			Restart();
			ReplayManager.Instance.Play();
		}
	}

	/*
	void OnAnimatorIK(int layerIndex)
	{
		Animator manimator = mMan.GetComponent<Animator>();
        float reach = 1.0f; //animator.GetFloat("RightHandReach");
        manimator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
        manimator.SetIKPosition(AvatarIKGoal.RightHand, mPlayer2.transform.position);
    }
	//*/

	// helper functions
	void Restart()
	{
		float depth = -8.75f;
		mPlayer1.transform.position = new Vector3(-1, 1, depth);
		mPlayer2.transform.position = new Vector3( 1, 1, depth);
	}
}

// END OF FILE /////////////////////////////////////////////////////////////////
