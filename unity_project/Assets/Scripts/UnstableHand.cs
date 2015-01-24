using UnityEngine;
using System.Collections;
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
	private float camera_range = 0.5f;
	private float diff_time;
	private float x_delta;
	private float y_delta;
	private Vector3 position_offset;
	
	public UnstableBehavior(string name, GameObject operand)
		: base(name, operand)
	{
		Debug.Log("UnstableBehavior initialized");
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
			Debug.Log("UnstableBehavior Operate");
			if(shake_intensity > 0){
				Vector3 start_position = mOperand.transform.position;
				Vector3 min_moving_range = originPosition - new Vector3 (camera_range, camera_range, 0);
				Vector3 max_moving_range = originPosition + new Vector3 (camera_range, camera_range, 0);
				diff_time += Time.deltaTime;
				Debug.Log("change delta value, diff time: " + diff_time.ToString());
				if(diff_time > Random.Range(3.0f, 5.0f)) {
					Debug.Log("change delta value, diff time: " + diff_time.ToString());
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
		Debug.Log("UnstableBehavior GenerateRecordedBehavior");
		TranslateBehavior generated_behavior = new TranslateBehavior("auto generate unstable behavior",    mOperand, position_offset);
		
		return generated_behavior;
	}
}

public class PushBehavior : Behavior {
	
	private Vector3 originPosition;
	private bool forward;
	private bool pushing;
	
	public PushBehavior(string name, GameObject operand)
	{
		mName = name;
		mOperand = operand;
		originPosition = mOperand.transform.position;
		forward = true;
		pushing = false;
	}
	
	// input: signal -- degree [0.0, 1.0] to which signal should be operated (0.0 means do nothing, generally)
	// output: true iff should record
	public override bool Operate(float signal)
	{
		Debug.Log("PushBehavior Operate");
		if (signal > 0.0f)
		{
			pushing = true;
		}
		if(pushing)
		{
			float z_boundary = originPosition.z + 1.0f;
			if(forward)
			{
				if((mOperand.transform.position.z + 0.1f) >= z_boundary)
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
		Debug.Log("PushBehavior GenerateRecordedBehavior");
		return this;
	}
}


public class UnstableHand : MonoBehaviour {

	private GameObject mHand;
	private ControlScheme mControls;
	private Vector3 mOriginal_position;
	private bool replaying = false;
	// Use this for initialization
	void Start () {
		mHand = GameObject.Find("Automatic Rifle Standard");
		mOriginal_position = mHand.transform.position;
		
		Scenario scenario = new Scenario();
		scenario.AddBehavior(new UnstableBehavior("unstable hand", mHand));
		scenario.AddBehavior(new PushBehavior("finger push", mHand));
		scenario.AddBehavior(new TranslateBehavior("player1 move up",    mHand, new Vector3( 0,  1, 0) * 0.1f));
		
		mControls = new ControlScheme();
		ControlSignal trueSignal;
		
		mControls.AddControl(new TrueSignal(),          scenario.GetBehavior("unstable hand"));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.W),          scenario.GetBehavior("player1 move up"));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.KeypadEnter),          scenario.GetBehavior("finger push"));
		
	}
	
	// Update is called once per frame
	void Update () {
		
		///transform.Translate(Vector3.forward * Time.deltaTime);
	}
	// called once per timestep update (critical: do game state updates here!!!)
	void FixedUpdate()
	{
		// update all the controls
		if(!replaying)
		{
			mControls.Update();
		}
		if (Input.GetKey(KeyCode.Space))
		{
			replaying = true;
			// start the playback
			Restart();
			ReplayManager.Instance.Play();
		}
	}
	
	// helper functions
	void Restart()
	{
		mHand.transform.position = mOriginal_position;
	}
}
