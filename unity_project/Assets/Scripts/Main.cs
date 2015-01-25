using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// BEHAVIOR ENTRY POINT ////////////////////////////////////////////////////////

public class Main : MonoBehaviour
{
	private GameObject mPlayer1, mPlayer2, mCamera, mMan, mLeftHand, mRightHand, mRightShoulder;
	private ControlScheme mControls;

	// Use this for initialization
	void Start ()
	{
		// create mock scene with two player objects
		mPlayer1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		mPlayer2 = GameObject.Find("CubeHand");
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
		//Animator manimator = mMan.GetComponent<Animator>();
		//manimator.runtimeAnimatorController = overrideController;
		//
		mLeftHand = GameObject.FindWithTag("LeftHand");
		mRightHand = GameObject.FindWithTag("RightHand");
		mRightShoulder = GameObject.FindWithTag("RightShoulder");
		// hook up the IK
		if (mRightShoulder != null)
		{
			//IKControl ik = mRightShoulder.GetComponent<IKControl>();
			//ik.target = mPlayer2.transform;
			//ik.ikActive = true;

			//IKCtrl ik = mRightShoulder.GetComponent<IKCtrl>();
			//ik.rightHandObj = mPlayer2.transform;
			//ik.ikActive = true; 

			//ikLimb2 ik = mRightShoulder.GetComponent<ikLimb2>();
			//IKLimb ik = mRightShoulder.GetComponent<IKLimb>();
			IKScriptNew ik = mRightShoulder.GetComponent<IKScriptNew>(); 
			//ik.target = mPlayer2.transform;
			ik.IsEnabled = true; 
		}
		// restart the scene
		Restart();

		// create the behaviors in this scene
		float speed = 0.025f;
		Behavior p1Up    = new TranslateBehavior("player1 move up",    mPlayer1, new Vector3( 0,  1, 0) * speed);
		Behavior p1Down  = new TranslateBehavior("player1 move down",  mPlayer1, new Vector3( 0, -1, 0) * speed);
		Behavior p1Left  = new TranslateBehavior("player1 move left",  mPlayer1, new Vector3(-1,  0, 0) * speed);
		Behavior p1Right = new TranslateBehavior("player1 move right", mPlayer1, new Vector3( 1,  0, 0) * speed);
		Behavior p2Up    = new TranslateBehavior("player2 move up",    mPlayer2, new Vector3( 0,  1, 0) * speed);
		Behavior p2Down  = new TranslateBehavior("player2 move down",  mPlayer2, new Vector3( 0, -1, 0) * speed);
		Behavior p2Left  = new TranslateBehavior("player2 move left",  mPlayer2, new Vector3(-1,  0, 0) * speed);
		Behavior p2Right = new TranslateBehavior("player2 move right", mPlayer2, new Vector3( 1,  0, 0) * speed);

		// create the control scheme that maps inputs to these behaviors
		mControls = new ControlScheme();
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.W),          p1Up   );
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.S),          p1Down );
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.A),          p1Left );
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.D),          p1Right);
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.UpArrow),    p2Up   );
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.DownArrow),  p2Down );
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.LeftArrow),  p2Left );
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.RightArrow), p2Right);
		//
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y,  1.0f), p1Up    );
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y, -1.0f), p1Down  );
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X, -1.0f), p1Left  );
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X,  1.0f), p1Right );
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y,  1.0f), p2Up    );
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y, -1.0f), p2Down  );
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X, -1.0f), p2Left  );
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X,  1.0f), p2Right );
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
		if (mLeftHand != null)
		{
			mLeftHand.transform.position  = mPlayer1.transform.position;
		}
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
