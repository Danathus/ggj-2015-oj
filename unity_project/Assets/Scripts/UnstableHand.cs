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
	private bool forward;
	private bool pushing;
	private GameObject mObstacle;
	public PushBehavior(string name, GameObject operand, GameObject obstacle=null)
		:base(name, operand)
	{
		mName = name;
		mOperand = operand;
		mObstacle = obstacle;
		originPosition = mOperand.transform.position;
		forward = true;
		pushing = false;
	}
	
	// input: signal -- degree [0.0, 1.0] to which signal should be operated (0.0 means do nothing, generally)
	// output: true iff should record
	public override bool Operate(float signal)
	{
		//Debug.Log("PushBehavior Operate");
		if (signal > 0.0f)
		{
			pushing = true;
		} 
		if(pushing)
		{
			float z_boundary = (mObstacle != null)?
				mObstacle.transform.position.z - (mObstacle.renderer.bounds.size.z/2 * mObstacle.transform.localScale.z) : 
				originPosition.z + 1.0f;
			if(forward)
			{
				float finger_z_position = mOperand.transform.position.z + (mOperand.renderer.bounds.size.z/2 * mOperand.transform.localScale.z);
				if((finger_z_position + 0.1f) >= z_boundary)
				{
					forward = false;
				}
			}
			float delta = (forward) ? (0.1f) : (-0.1f);
			mOperand.transform.position = new Vector3(
				mOperand.transform.position.x,
				mOperand.transform.position.y,
				mOperand.transform.position.z + delta);
			if(mOperand.transform.position.z <= originPosition.z)
			{
				pushing = false;
				forward = true;
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

public class ButtonPushBehavior : Behavior {
	
	private Vector3 originPosition;
	private bool push_down;
	private bool pushing;
	private GameObject mPusher;
	public bool mTriggered;
	public int mFloor;
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
	}
	
	// input: signal -- degree [0.0, 1.0] to which signal should be operated (0.0 means do nothing, generally)
	// output: true iff should record
	public override bool Operate(float signal)
	{
		//Debug.Log("PushBehavior Operate");
		float finger_z_position = mPusher.transform.position.z + (mPusher.renderer.bounds.size.z/2 * mPusher.transform.localScale.z);
		Vector3 finger_tip = new Vector3(
			mPusher.transform.position.x,
			mPusher.transform.position.y + mPusher.renderer.bounds.size.y/2,
			mPusher.transform.position.z + mPusher.renderer.bounds.size.z/2);
		if (signal > 0.0f)
		{	
			Rect button_bounds = new Rect(mOperand.transform.position.x - mOperand.renderer.bounds.size.x/2, mOperand.transform.position.y - mOperand.renderer.bounds.size.y/2, mOperand.transform.position.x + mOperand.renderer.bounds.size.x/2, mOperand.transform.position.y + mOperand.renderer.bounds.size.y/2);
			Vector2 finger_position = new Vector2(finger_tip.x, finger_tip.y);
			//Debug.Log("button_bounds " + button_bounds.ToString());
			//Debug.Log("finger position " + finger_position.ToString());
			//Debug.Log("inside? "  + button_bounds.Contains(finger_position));
				if( button_bounds.Contains(finger_position) &&
			   ((finger_tip.z) >= mOperand.transform.position.z - mOperand.renderer.bounds.size.z/2))
			{
				pushing = true;
				mTriggered = true;
			}
		} 
		if(pushing)
		{
			float z_boundary = originPosition.z + mOperand.renderer.bounds.size.z/2;
			if(push_down)
			{
				if((finger_tip.z) <= mOperand.transform.position.z - mOperand.renderer.bounds.size.z/2)
				{
					push_down = false;
				}
			}
			float delta = (push_down) ? (0.1f) : (-0.1f);
			mOperand.transform.position = new Vector3(
				mOperand.transform.position.x,
				mOperand.transform.position.y,
				mOperand.transform.position.z + delta);
			if(mOperand.transform.position.z <= originPosition.z)
			{
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
public class ElevatorMoveBehavior : Behavior {
	private int mCurrFloor;
	Dictionary<int, string> mFloorMap;
	public ElevatorMoveBehavior(string name, GameObject operand, Dictionary<int, string> floorMap)
		: base(name, operand)
	{
		mCurrFloor = 1;
	}
	
	public override bool Operate(float signal)
	{
		if (signal > 0.0f)
		{
			Debug.Log("signal: " + signal.ToString());
			if(mCurrFloor != (int)signal)
			{
				mCurrFloor = (int)signal;
				
				Debug.Log("go to " + mCurrFloor.ToString() + " floor");
				
				return true;
			}
		}
		return false;
	}
	
	public override Behavior GenerateRecordedBehavior()
	{
		//Debug.Log("UnstableBehavior GenerateRecordedBehavior");
		//TranslateBehavior generated_behavior = new TranslateBehavior("auto generate unstable behavior",    mOperand, position_offset);
		
		return this;
	}
}

public class UnstableHand : Scenario {

	private GameObject mHand;
	private GameObject mElevateKeyPad;
	private GameObject mCorrectButton;
	private GameObject mWrongButton;
	private GameObject mGameCamera;
	
	private Vector3 mOriginal_position;
	private bool replaying = false;
	// Use this for initialization
	void Start () {
		mHand = GameObject.Find("Automatic Rifle Standard");
		mElevateKeyPad = GameObject.Find("wallBrickExposedShort");
		mCorrectButton = GameObject.Find("Correct Button Test");
		mWrongButton = GameObject.Find("Wrong Button Test");
		mGameCamera = GameObject.Find("Main Camera");
		
		mOriginal_position = mHand.transform.position;
		
		List<ButtonPushBehavior> buttonBehaviorList = new List<ButtonPushBehavior>();
		buttonBehaviorList.Add(new CorrectButtonBehavior("correct button push", mCorrectButton, mHand, 1));
		buttonBehaviorList.Add(new WrongButtonBehavior("wrong button push", mWrongButton, mHand, 2));
		for(int i = 0; i < buttonBehaviorList.Count; ++i)
		{
			scenario.AddBehavior(buttonBehaviorList[i]);
		}
		Dictionary<int, string> floorMap = new Dictionary<int, string>();
		floorMap.Add(1, "hell");
		floorMap.Add(2, "heaven");
		scenario.AddBehavior(new ElevatorMoveBehavior("elevator move", mElevateKeyPad, floorMap));
		float speed = 0.1f;
		mControls.AddControl(new TrueSignal(),          					new UnstableBehavior("unstable hand", mHand));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.W),          	new TranslateBehavior("player1 move up",    mHand, new Vector3( 0,  1, 0) * speed));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.S),          	new TranslateBehavior("player1 move down",  mHand, new Vector3( 0, -1, 0) * speed));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.A),          	new TranslateBehavior("player1 move left",  mHand, new Vector3(-1,  0, 0) * speed));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.D),          	new TranslateBehavior("player1 move right", mHand, new Vector3( 1,  0, 0) * speed));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.KeypadEnter),	new PushBehavior("finger push", mHand, mElevateKeyPad));
		//mControls.AddControl(new TrueSignal(),          scenario.GetBehavior("unstable hand"));
		mControls.AddControl(new TrueSignal(),          scenario.GetBehavior("correct button push"));
		mControls.AddControl(new TrueSignal(),          scenario.GetBehavior("wrong button push"));
		for(int i = 0; i < buttonBehaviorList.Count; ++i)
		{
			mControls.AddControl(new CorrectLevelSignal(buttonBehaviorList[i]),          scenario.GetBehavior("elevator move"));
		}
	}
	
		Vector3 finger_tip = new Vector3(
			mHand.transform.position.x,
			mHand.transform.position.y + mHand.renderer.bounds.size.y/2,
			mHand.transform.position.z + mHand.renderer.bounds.size.z/2);
		//GameObject helper = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//helper.transform.position = finger_tip;
		//mGameCamera.transform.LookAt(finger_tip);
	// called once per timestep update (critical: do game state updates here!!!)
	void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			BeginReplay();
		}
	}
	
	// helper functions
	public override void Reset()
	{
		mHand.transform.position = mOriginal_position;
	}
}
