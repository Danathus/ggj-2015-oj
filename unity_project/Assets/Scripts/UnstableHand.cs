using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Rshaking: http://forum.unity3d.com/threads/camera-shake-script-c.142764/

/// </summary>
public class TrueSignal : ControlSignal
{
	public override float PollSignal()
	{
		return 1.0f;
	}
}


public class UnstableBehavior: Behavior
{
	private Vector3 originPosition;
	//private Quaternion originRotation;
	
	private float shake_decay;
	private float shake_intensity;
	private float drunk_level = 2.0f;
	private Vector2 camera_range = new Vector2(1.0f, 2.0f);
	private float diff_time;
	private float x_delta;
	private float y_delta;
	private Vector3 position_offset;
	
	public UnstableBehavior(string name, GameObject operand)
		: base(name, operand)
	{
		//Debug.Log("UnstableBehavior initialized");
		originPosition = mOperand.transform.position;
		//originRotation = mOperand.transform.rotation;
		shake_intensity = 0.001f;
		shake_decay = 0.002f;
		diff_time = 0.0f;
		x_delta = Random.Range(-drunk_level, drunk_level);
		y_delta = Random.Range(-drunk_level, drunk_level);
		position_offset = new Vector3(0, 0, 0);
	}
	
	public override bool Operate(float signal)
	{
		if (signal > 0.0f)
		{
			//Debug.Log("UnstableBehavior Operate");
			if(shake_intensity > 0){
				Vector3 start_position = mOperand.transform.position;
				Vector3 min_moving_range = originPosition - new Vector3 (camera_range.x, camera_range.y, 0);
				Vector3 max_moving_range = originPosition + new Vector3 (camera_range.x, camera_range.y, 0);
				diff_time += Time.deltaTime;
				//Debug.Log("change delta value, diff time: " + diff_time.ToString());
				if(diff_time > Random.Range(3.0f, 5.0f)) {
					//Debug.Log("change delta value, diff time: " + diff_time.ToString());
					x_delta = Random.Range(-drunk_level, drunk_level);
					y_delta = Random.Range(-drunk_level, drunk_level);
					diff_time = 0.0f;
				}
				mOperand.transform.position = new Vector3 (
					Mathf.Min(Mathf.Max(mOperand.transform.position.x + x_delta * shake_intensity, min_moving_range.x), max_moving_range.x),
					Mathf.Min(Mathf.Max(mOperand.transform.position.y + y_delta * shake_intensity, min_moving_range.y), max_moving_range.y),
					mOperand.transform.position.z);
				if(mOperand.transform.position.x == min_moving_range.x || mOperand.transform.position.x == max_moving_range.x ||
				   mOperand.transform.position.y == min_moving_range.y || mOperand.transform.position.y == max_moving_range.y){
					x_delta = Random.Range(-drunk_level, drunk_level);
					y_delta = Random.Range(-drunk_level, drunk_level);
				}
				position_offset = mOperand.transform.position - start_position;
				//Debug.Log("delta time: " + Time.deltaTime.ToString());
				//Debug.Log("x_delt: " + x_delta.ToString());
				//Debug.Log("new position: " + transform.position.ToString());
				//transform.rotation = new Quaternion(
				//	originRotation.x + Random.Range(-shake_intensity,shake_intensity)*.2f,
				//	originRotation.y + Random.Range(-shake_intensity,shake_intensity)*.2f,
				//	originRotation.z,
				//	originRotation.w);
				///shake_intensity -= shake_decay;
				return true;
			}
		}
		return false;
	}
	
	public override Behavior GenerateRecordedBehavior()
	{
		//Debug.Log("UnstableBehavior GenerateRecordedBehavior");
		TranslateBehavior generated_behavior = new TranslateBehavior("auto generate unstable behavior",    mOperand, position_offset);
		
		return generated_behavior;
	}
}

public class PushBehavior : Behavior {
	
	private Vector3 originPosition;
	private Vector3 originObstaclePosition;
	private bool forward;
	private bool pushing;
	private GameObject mObstacle;
	public bool mColiding;
	
	public PushBehavior(string name, GameObject operand, GameObject obstacle=null)
		:base(name, operand)
	{
		mName = name;
		mOperand = operand;
		mObstacle = obstacle;
		originPosition = mOperand.transform.position;
		originObstaclePosition = obstacle.transform.position;
		forward = true;
		pushing = false;
		mColiding = false;
	}
	
	// input: signal -- degree [0.0, 1.0] to which signal should be operated (0.0 means do nothing, generally)
	// output: true iff should record
	public override bool Operate(float signal)
	{
		//Debug.Log("PushBehavior Operate");
		if (signal > 0.0f)
		{
			//Debug.Log("signal from PushBehavior" + signal.ToString());
			pushing = true;
			
			//Debug.Log("coliding: " + mColiding);
		} 
		if(pushing)
		{
			//Debug.Log("obstacle is null? " + mObstacle == null);
			float z_boundary = (mObstacle != null)?
				mObstacle.transform.position.z + (mObstacle.renderer.bounds.size.z/2) : 
				originPosition.z + 1.0f;
			if(forward)
			{
				Vector3 finger_tip = new Vector3(
					mOperand.transform.position.x,
					mOperand.transform.position.y,
					mOperand.transform.position.z);
				float finger_z_position = finger_tip.z;
				if(mColiding || finger_z_position <= mObstacle.transform.position.z)
				{
					forward = false;
				}
			}
			if(forward) {
				float delta = -0.005f;
				mOperand.transform.position = new Vector3(
					mOperand.transform.position.x,
					mOperand.transform.position.y,
					mOperand.transform.position.z + delta);
			}
			else
			{
				float delta = 0.005f;
				mOperand.transform.position = new Vector3(
					mOperand.transform.position.x,
					mOperand.transform.position.y,
					mOperand.transform.position.z + delta);
			}
			if(mOperand.transform.position.z >= originPosition.z && !forward)
			{
				pushing = false;
				forward = true;
				mColiding = false;
			}
			//Debug.Log("push: " + pushing + " forward: " + forward + " mColliding: " + mColiding);
			return true;
		}
		return false;
	}
	public override Behavior GenerateRecordedBehavior()
	{
		//Debug.Log("PushBehavior GenerateRecordedBehavior");
		return this;
	}
}

public class ButtonPushBehavior : Behavior {
	
	private Vector3 originPosition;
	private bool push_down;
	private bool pushing;
	private GameObject mPusher;
	public bool mTriggered;
	public int mFloor;
	private GameObject helper;
	
	public ButtonPushBehavior(string name, GameObject operand, GameObject pusher, int floor)
		:base(name, operand)
	{
		mName = name;
		mOperand = operand;
		mPusher = pusher;
		originPosition = mOperand.transform.position;
		push_down = true;
		pushing = false;
		mTriggered = false;
		mFloor = floor;
		helper = GameObject.CreatePrimitive(PrimitiveType.Cube);
		helper.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
	}
	
	// input: signal -- degree [0.0, 1.0] to which signal should be operated (0.0 means do nothing, generally)
	// output: true iff should record
	public override bool Operate(float signal)
	{
		//Debug.Log("PushBehavior Operate");
		//float finger_z_position = mPusher.transform.position.z + (mPusher.renderer.bounds.size.z/2 * mPusher.transform.localScale.z);
		Vector3 finger_tip = new Vector3(
			mPusher.transform.position.x,
			mPusher.transform.position.y,
			mPusher.transform.position.z);
		helper.transform.position = finger_tip;
		if (signal > 0.0f)
		{	
			Rect button_bounds = new Rect(mOperand.transform.position.x - mOperand.renderer.bounds.size.x/2, 
			                              mOperand.transform.position.y - mOperand.renderer.bounds.size.y/2, 
			                              mOperand.renderer.bounds.size.x , 
			                              mOperand.renderer.bounds.size.y);
			Vector2 finger_position = new Vector2(finger_tip.x, finger_tip.y);
			//Debug.Log("button_bounds " + button_bounds.ToString());
			//Debug.Log("finger position " + finger_position.ToString());
			//Debug.Log("inside? "  + button_bounds.Contains(finger_position));
			//Debug.Log("finger_tip z: " + finger_tip.z.ToString() + " operand bounds: " + (mOperand.transform.position.z + mOperand.renderer.bounds.size.z/2).ToString());
				//if( button_bounds.Contains(finger_position) &&
			   //((finger_tip.z) <= mOperand.transform.position.z + mOperand.renderer.bounds.size.z/2))
			if(true)
			{
				
				pushing = true;
				mTriggered = true;
			}
		} 
		if(pushing)
		{
			//Debug.Log ("pushing");
			if(push_down)
			{
				//Debug.Log ("push down check");
				if(mOperand.transform.position.z < originPosition.z - mOperand.renderer.bounds.size.z)
				{
					//Debug.Log ("stop push down");
					push_down = false;
				}
			}
			float delta = (push_down) ? (-0.0006f) : (0.0006f);
			//Debug.Log ("push by" + delta.ToString());
			mOperand.transform.position = new Vector3(
				mOperand.transform.position.x,
				mOperand.transform.position.y,
				mOperand.transform.position.z + delta);
			//Debug.Log("z: " + mOperand.transform.position.z.ToString());
			if(mOperand.transform.position.z >= originPosition.z)
			{
				//Debug.Log ("stop pushing");
				pushing = false;
				push_down = true;
			}
			return true;
		}
		return false;
	}
	public override Behavior GenerateRecordedBehavior()
	{
		//Debug.Log("PushBehavior GenerateRecordedBehavior");
		return this;
	}
}

public class CorrectButtonBehavior : ButtonPushBehavior {

	public CorrectButtonBehavior(string name, GameObject operand, GameObject pusher, int floor)
		:base (name, operand, pusher, floor)
	{
	}
}

public class WrongButtonBehavior : ButtonPushBehavior {
	
	public WrongButtonBehavior(string name, GameObject operand, GameObject pusher, int floor)
		:base (name, operand, pusher, floor)
	{
	}
}

public class CorrectLevelSignal : ControlSignal
{
	ButtonPushBehavior mButtomBehavior; 
	public CorrectLevelSignal(ButtonPushBehavior buttomBehavior)
		: base()
	{
		mButtomBehavior = buttomBehavior;
	}
	public override float PollSignal()
	{
		if(mButtomBehavior.mTriggered)
		{
			mButtomBehavior.mTriggered = false;
			return mButtomBehavior.mFloor;
		}
		return 0.0f;
	}
}

public class FloorChangeSignal : ControlSignal
{
	private int mFloor;
	public bool mTriggered;
	public FloorChangeSignal(int floor)
		: base()
	{
		mFloor = floor;
		mTriggered = false;
	}
	public override float PollSignal()
	{
		
		//Debug.Log(mFloor.ToString() + " FloorChangeSignal PollSignal");
		if(mTriggered)
		{
			//Debug.Log("floor " + mFloor.ToString() + " triggered");
			mTriggered = false;
			return mFloor;
		}
		return 0.0f;
	}
}

public class ElevatorMoveBehavior : Behavior {
	private int mCurrFloor;
	Dictionary<int, string> mFloorMap;
	private float mHeight;
	private Vector3 position_offset;
	private bool jump;
	private Vector3 original_position;
	private bool back;
	private bool openDoor;
	protected bool closeDoor;
	private GameObject mLeftDoor;
	private GameObject mRightDoor;
	private Vector3 left_door_position;
	private Vector3 right_door_position;
	Scenario mScenario;
	private GameObject mHellBackground;
	private GameObject mHeavenBackground;
	private GameObject mBathroomBackground;

	
	protected bool IsRightFloor()
	{
		return mFloorMap[mCurrFloor] == "heaven";
	}
	
	public ElevatorMoveBehavior(string name, GameObject operand, Dictionary<int, string> floorMap, GameObject leftdoor, GameObject rightdoor, Scenario scenario, GameObject hell, GameObject heaven, GameObject bathroom)
		: base(name, operand)
	{
		mCurrFloor = 0;
		mOperand.transform.position = new Vector3(1.87f, 0.0f, -2.76f);
		jump = false;
		original_position = mOperand.transform.position;
		openDoor = false;
		closeDoor = false;
		mLeftDoor = leftdoor;
		mRightDoor = rightdoor;
		left_door_position = mLeftDoor.transform.position;
		right_door_position = mRightDoor.transform.position;
		mScenario = scenario;
		mHellBackground = hell;
		mHeavenBackground = heaven;
		mBathroomBackground = bathroom;
		//hellTex = hell;
		mFloorMap = floorMap;
	}
	
	public override bool Operate(float signal)
	{
		if (signal > 0.0f)
		{
			if(!openDoor && !closeDoor)
			{
				jump = true;
				back = false;
				//Debug.Log("signal: " + signal.ToString());
				if(mCurrFloor != (int)signal)
				{
					mCurrFloor = (int)signal;
					string floorcontext = mFloorMap[mCurrFloor];
					
					Debug.Log("go to " + floorcontext + " floor");
					//mHeight = (float)mCurrFloor * 2.0f;
					if(floorcontext == "hell")
					{
						//mBackgroundCanvas.renderer.material.SetTexture("_MainTex", hellTex);
						//mBackgroundCanvas.renderer.renderer.enabled = false;
						mHellBackground.renderer.enabled = true;
						mHeavenBackground.renderer.enabled = false;
						mBathroomBackground.renderer.enabled = false;
					}
					else if(floorcontext == "heaven")
					{
						//mBackgroundCanvas.renderer.material.SetTexture("_MainTex", heavenTex);
						//mBackgroundCanvas.renderer.renderer.enabled = true;
						mHellBackground.renderer.enabled = false;
						mHeavenBackground.renderer.enabled = true;
						mBathroomBackground.renderer.enabled = false;
					}
					else if(floorcontext == "bathroom")
					{
						//mBackgroundCanvas.renderer.material.SetTexture("_MainTex", bathroomTex);
						//mBackgroundCanvas.renderer.renderer.enabled = false;
						mHellBackground.renderer.enabled = false;
						mHeavenBackground.renderer.enabled = false;
						mBathroomBackground.renderer.enabled = true;
					}
					
				}
				return true;
			}
		}
		if(jump)
		{
			float delta = (!back) ? 0.01f : (-0.01f);
			Vector3 oriPosition = mOperand.transform.position;
			if(!back && mOperand.transform.position.y > original_position.y + 0.2f)
			{
				delta = -0.01f;
				back = true;
			}
			mOperand.transform.position = new Vector3 ( mOperand.transform.position.x, mOperand.transform.position.y + delta, mOperand.transform.position.z);
			if(mOperand.transform.position.y <= original_position.y)
			{
				jump = false;
				openDoor = true;
			}
			//Debug.Log ("target height: " + mHeight.ToString() + " current height: " + mOperand.transform.position.y);
			position_offset = mOperand.transform.position - oriPosition;
			
		}
		if(openDoor)
		{
			if(mLeftDoor.transform.position.x >= left_door_position.x + 0.9f)
			{
				openDoor = false;
				if (IsRightFloor())
				{
					// win condition
					mScenario.Victory();
				}
				else
				{
					closeDoor = true;
				}
			}
			if(openDoor)
			{
			mLeftDoor.transform.position = new Vector3 (
				mLeftDoor.transform.position.x + 0.01f,
				mLeftDoor.transform.position.y,
				mLeftDoor.transform.position.z
			);
			
			mRightDoor.transform.position = new Vector3 (
				mRightDoor.transform.position.x - 0.01f,
				mRightDoor.transform.position.y,
				mRightDoor.transform.position.z
				);
			}
		}
		if(closeDoor)
		{
			if(mLeftDoor.transform.position.x <= left_door_position.x)
			{
				closeDoor = false;
			}
			if(closeDoor)
			{
				mLeftDoor.transform.position = new Vector3 (
					mLeftDoor.transform.position.x - 0.01f,
					mLeftDoor.transform.position.y,
					mLeftDoor.transform.position.z
					);
				
				mRightDoor.transform.position = new Vector3 (
					mRightDoor.transform.position.x + 0.01f,
					mRightDoor.transform.position.y,
					mRightDoor.transform.position.z
					);
			}
		}
		
		
		return false;
	}
	
	public override Behavior GenerateRecordedBehavior()
	{
		//Debug.Log("UnstableBehavior GenerateRecordedBehavior");
		TranslateBehavior generated_behavior = new TranslateBehavior("auto generate unstable behavior",    mOperand, position_offset);
		
		return this;
	}
}

struct FlatTransform
{
	public Vector3    m_position;
	public Quaternion m_rotation;
	public Vector3    m_localScale;

	public void Read(Transform transform)
	{
		m_position   = transform.position;
		m_rotation   = transform.rotation;
		m_localScale = transform.localScale;
	}

	public void Write(Transform transform)
	{
		transform.position   = m_position;
		transform.rotation   = m_rotation;
		transform.localScale = m_localScale;
	}
}

public class UnstableHand : Scenario
{
	static public RandomReverter mRandomReverter = null;
	private GameObject mHand;
	private GameObject mElevateKeyPad;
	private GameObject mCorrectButton;
	private GameObject mWrongButton;
	private GameObject mGameCamera;
	private GameObject mElevatorFloor;
	private List<FloorChangeSignal> mFloorSignals = new List<FloorChangeSignal>();
	private List<FloorChangeSignal> mButtonPushSignals = new List<FloorChangeSignal>();
	private PushBehavior mHandPushBehavior;
	private GameObject mHellBackground;
	private GameObject mHeavenBackground;
	private GameObject mBathroomBackground;
	public Texture hellTex;
	public Texture heavenTex;
	public Texture bathroomTex;

	private bool replaying = false;
	
	public ControlScheme mIndirectControls;

	// remembered IK control states
	//   game objects
	GameObject mGameObject_IK_Control;
	GameObject mGameObject_RightArm;
	GameObject mGameObject_RightForeArm;
	GameObject mGameObject_RightHand;
	GameObject mGameObject_IK_fingertip;
	GameObject mGameObject_IK_elbow;
	//   flattened transforms
	FlatTransform mRememberedTransform_IK_Control   = new FlatTransform();
	FlatTransform mRememberedTransform_RightArm     = new FlatTransform();
	FlatTransform mRememberedTransform_RightForeArm = new FlatTransform();
	FlatTransform mRememberedTransform_RightHand    = new FlatTransform();
	FlatTransform mRememberedTransform_IK_fingertip = new FlatTransform();
	FlatTransform mRememberedTransform_IK_elbow     = new FlatTransform();

	void OnDestroy()
	{
		mIndirectControls = null;
	}

	public override void Reset()
	{
		mRandomReverter.Revert();
		mElevatorFloor.transform.position = new Vector3(1.87f, 0.0f, -2.76f);
		//Debug.Log("mElevatorFloor: " + mElevatorFloor.name);

		mRememberedTransform_IK_Control.Write  (mGameObject_IK_Control.transform);
		mRememberedTransform_RightArm.Write    (mGameObject_RightArm.transform);
		mRememberedTransform_RightForeArm.Write(mGameObject_RightForeArm.transform);
		mRememberedTransform_RightHand.Write   (mGameObject_RightHand.transform);
		mRememberedTransform_IK_fingertip.Write(mGameObject_IK_fingertip.transform);
		mRememberedTransform_IK_elbow.Write    (mGameObject_IK_elbow.transform);
		//
		mGameObject_IK_Control.GetComponent<IKScriptNew>().Reset();

		// create indirect controls
		mIndirectControls = new ControlScheme();

		Dictionary<int, string> floorMap = new Dictionary<int, string>();
		int heavenFloor = Random.Range(1, 4);
		floorMap.Add(heavenFloor, "heaven");
		string team_instructions;
		switch(heavenFloor)
		{
			case 1:
			{
			floorMap.Add(2, "hell");
			floorMap.Add(3, "bathroom");
			team_instructions = "Ground floor exit!\nGo!";
			}
			break;
			case 2:
			{
			floorMap.Add(1, "hell");
			floorMap.Add(3, "bathroom");
			team_instructions = "1st floor exit!\nGo!";
			}
			break;
			case 3:
			{
			floorMap.Add(2, "hell");
			floorMap.Add(1, "bathroom");
			team_instructions = "2nd floor exit!\nGo!";
			}
			break;
		}
		mFloorSignals.Clear();
		mButtonPushSignals.Clear();
		//floorMap.Add(1, "hell");
		mFloorSignals.Add(new FloorChangeSignal(1));
		mButtonPushSignals.Add(new FloorChangeSignal(1));
		//floorMap.Add(2, "heaven");
		mFloorSignals.Add(new FloorChangeSignal(2));
		mButtonPushSignals.Add(new FloorChangeSignal(2));
		//floorMap.Add(3, "bathroom");
		mFloorSignals.Add(new FloorChangeSignal(3));
		mButtonPushSignals.Add(new FloorChangeSignal(3));
		
		GameObject leftDoor = GameObject.Find("left door");
		GameObject rightDoor = GameObject.Find("right door");

		ElevatorMoveBehavior elevatorMover = new ElevatorMoveBehavior("elevator move", mElevatorFloor, floorMap, leftDoor, rightDoor, this, mHellBackground, mHeavenBackground, mBathroomBackground);
		for(int i = 0; i < mFloorSignals.Count; ++i)
		{
			mIndirectControls.AddControl(mFloorSignals[i],          elevatorMover);
			GameObject obj = GameObject.Find("button.00" + (i+1).ToString());
			//Debug.Log(obj.name);
			mIndirectControls.AddControl(mButtonPushSignals[i],          new CorrectButtonBehavior("correct button push", obj, mHand, i));
			/*
			ButtonPushBehavior pushBehavior;
			if (i == 0)
			{
				pushBehavior = new CorrectButtonBehavior("correct button push", obj, mHand, i, this);
			}
			else
			{
				pushBehavior = new WrongButtonBehavior("wrong button push", obj, mHand, i);
			}
			mIndirectControls.AddControl(mButtonPushSignals[i], pushBehavior);
			//*/
		}
	}

	// Use this for initialization
	void Start ()
	{
		mRandomReverter = new RandomReverter();

		// find key game objects
		mHand = GameObject.Find("IK_fingertip");
		//Debug.Log("hand obj name: " + mHand.name);
		mElevateKeyPad = GameObject.Find("elevator button keypad");
		//Debug.Log("mElevateKeyPad name: " + mElevateKeyPad.name);
		mCorrectButton = GameObject.Find("button.001");
		mWrongButton = GameObject.Find("button.002");
		mGameCamera = GameObject.Find("Main Camera");
		mElevatorFloor = GameObject.Find("big ground");
		GameObject elevatorWall = GameObject.Find("elevator walls");
		mHellBackground = GameObject.Find("outside environment_hell");
		mHeavenBackground = GameObject.Find("outside environment_heaven");
		mBathroomBackground = GameObject.Find("outside environment_bathroom");
		//
		mGameObject_IK_Control   = GameObject.Find("IK_Control");
		mGameObject_RightArm     = GameObject.Find("mixamorig:RightArm");
		mGameObject_RightForeArm = GameObject.Find("mixamorig:RightForeArm");
		mGameObject_RightHand    = GameObject.Find("mixamorig:RightHand");
		mGameObject_IK_fingertip = GameObject.Find("IK_fingertip");
		mGameObject_IK_elbow     = GameObject.Find("IK_elbow");

		// sample initial state first-time through
		mRememberedTransform_IK_Control.Read  (mGameObject_IK_Control.transform);
		mRememberedTransform_RightArm.Read    (mGameObject_RightArm.transform);
		mRememberedTransform_RightForeArm.Read(mGameObject_RightForeArm.transform);
		mRememberedTransform_RightHand.Read   (mGameObject_RightHand.transform);
		mRememberedTransform_IK_fingertip.Read(mGameObject_IK_fingertip.transform);
		mRememberedTransform_IK_elbow.Read    (mGameObject_IK_elbow.transform);

		// initialize state
		Reset();

		// these are not presently actually used
		//List<ButtonPushBehavior> buttonBehaviorList = new List<ButtonPushBehavior>();
		//buttonBehaviorList.Add(new CorrectButtonBehavior("correct button push", mCorrectButton, mHand, 1));
		//buttonBehaviorList.Add(new WrongButtonBehavior("wrong button push", mWrongButton, mHand, 2));

		float speed = 0.01f;
		// automatic controls
		mControls.AddControl(new TrueSignal(),          					new UnstableBehavior("unstable hand", mHand));

		// first player
		Behavior p1Up    = new TranslateBehavior("player1 move up",    mHand, new Vector3( 0,  1, 0) * speed);
		Behavior p1Down  = new TranslateBehavior("player1 move down",  mHand, new Vector3( 0, -1, 0) * speed);
		Behavior p1Left  = new TranslateBehavior("player1 move left",  mHand, new Vector3( 1,  0, 0) * speed);
		Behavior p1Right = new TranslateBehavior("player1 move right", mHand, new Vector3(-1,  0, 0) * speed);
		SetControlScheme(0, p1Up, p1Down, p1Left, p1Right);

		// second player
		mHandPushBehavior = new PushBehavior("finger push", mHand, mElevateKeyPad);
		SetControlScheme(1, mHandPushBehavior, null, null, null);

		//mControls.AddControl(new TrueSignal(),          scenario.GetBehavior("unstable hand"));
		//mControls.AddControl(new TrueSignal(),          new CorrectButtonBehavior("correct button push", mCorrectButton, mHand, 1));
		//mControls.AddControl(new TrueSignal(),          new CorrectButtonBehavior("wrong button push", mWrongButton, mHand, 2));
	}


		//GameObject helper = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//helper.transform.position = finger_tip;
		//mGameCamera.transform.LookAt(finger_tip);

	// called once per timestep update (critical: do game state updates here!!!)
	
	void FixedUpdate()
	{
		ScenarioUpdate();

		mIndirectControls.Update();

		//if (Input.GetKey(KeyCode.Space))
		//{
		//	Victory();
		//	// BeginReplay();
		//}
	}
	
	void OnTriggerEnter(Collider button) {
		Debug.Log ("hit " + button.name);
		if(button.name == "button.001" )
		{
			mFloorSignals[0].mTriggered = true;
			mButtonPushSignals[0].mTriggered = true;
			mHandPushBehavior.mColiding = true;
		}
		else if(button.name == "button.002" )
		{
			mFloorSignals[1].mTriggered = true;
			mButtonPushSignals[1].mTriggered = true;
			mHandPushBehavior.mColiding = true;
		}
		else if(button.name == "button.003" )
		{
			mFloorSignals[2].mTriggered = true;
			mButtonPushSignals[2].mTriggered = true;
			mHandPushBehavior.mColiding = true;
		}
		else
		{
			mHandPushBehavior.mColiding = true;
		}
	}
}
