using UnityEngine;
using System.Collections;

public class MonoWheel : Scenario {

	public Transform wheelMount;
	public float accel = 250, maxSpeed = 500;
	public float angleDeviation = 15, minDegreePerSecond = 10;

	private float _player1Angle = 0;
	private float _player2Angle = 0;
	private Vector3 _accumPlayer1 = new Vector3();
	private Vector3 _accumPlayer2 = new Vector3();
	private float _wheelRadius = 0.8f;
	private float _wheelSpeed;

	public override void Reset ()
	{
	}

	// Use this for initialization
	void Start () {
		Behavior p1Up    = new MovementCallbackBehavior("player1 move up",    this.gameObject, new Vector3( 0,  1, 0), MovePlayer1);
		Behavior p1Down  = new MovementCallbackBehavior("player1 move down",  this.gameObject, new Vector3( 0, -1, 0), MovePlayer1);
		Behavior p1Left  = new MovementCallbackBehavior("player1 move left",  this.gameObject, new Vector3(-1,  0, 0), MovePlayer1);
		Behavior p1Right = new MovementCallbackBehavior("player1 move right", this.gameObject, new Vector3( 1,  0, 0), MovePlayer1);
		Behavior p2Up    = new MovementCallbackBehavior("player2 move up",    this.gameObject, new Vector3( 0,  1, 0), MovePlayer2);
		Behavior p2Down  = new MovementCallbackBehavior("player2 move down",  this.gameObject, new Vector3( 0, -1, 0), MovePlayer2);
		Behavior p2Left  = new MovementCallbackBehavior("player2 move left",  this.gameObject, new Vector3(-1,  0, 0), MovePlayer2);
		Behavior p2Right = new MovementCallbackBehavior("player2 move right", this.gameObject, new Vector3( 1,  0, 0), MovePlayer2);
		
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

	void MovePlayer1(GameObject gameObject, Vector3 offset)
	{
		_accumPlayer1 += offset;
	}
	
	void MovePlayer2(GameObject gameObject, Vector3 offset)
	{
		_accumPlayer2 += offset;
	}

	float GetAngle (Vector3 vec, float oldAngle)
	{
		if (vec.magnitude < 0.1) {
			return oldAngle;
		} else {
			float ret = Mathf.Atan2(vec.y, vec.x) * (180 / Mathf.PI);
			return ret < 0 ? ret + 360 : ret;
		}
	}

	float GetAngleDelta(float angle1, float angle2)
	{
		float delta = (angle1 - angle2) % 360;
		if (delta < -180)
			delta += 360;
		else if (delta > 180)
			delta -= 360;
		return Mathf.Abs (delta);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		ScenarioUpdate ();

		float angle1 = GetAngle(_accumPlayer1, _player1Angle);
		float angle2 = GetAngle(_accumPlayer2, _player2Angle);
		float delta1 = GetAngleDelta (angle1, _player1Angle);
		float delta2 = GetAngleDelta (angle2, _player2Angle);
		float delta = GetAngleDelta (angle1, angle2);

		_player1Angle = angle1;
		_player2Angle = angle2;
		_accumPlayer1.Set (0, 0, 0);
		_accumPlayer2.Set (0, 0, 0);

		Debug.Log ("p1:"+angle1);
		Debug.Log ("p2:"+angle2);
		Debug.Log (delta);

		if (Mathf.Abs (delta - 180) < angleDeviation){// && delta1 + delta2 > minDegreePerSecond * Time.fixedDeltaTime) {
			_wheelSpeed += accel * Time.fixedDeltaTime;
		} else {
			_wheelSpeed -= accel * Time.fixedDeltaTime;
		}
		_wheelSpeed = Mathf.Clamp (_wheelSpeed, 0, maxSpeed);

		wheelMount.Rotate(new Vector3(_wheelSpeed * Time.fixedDeltaTime, 0, 0));
	}
}
