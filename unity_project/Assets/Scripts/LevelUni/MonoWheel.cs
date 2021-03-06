﻿using UnityEngine;
using System.Collections;

public class MonoWheel : Scenario {

	public Transform wheelMount;
	public Transform unicycle;
	public Transform player1Wheel;
	public Transform player2Wheel;
	public Transform leftShoulder;
	public Transform rightShoulder;
	public float accel = 150, doubleaccel = 350, decel = 150, maxSpeed = 500;
	public float angleDeviation = 40, singleAngleOffset = 90;
	public float tiltSpeed = 1;
	public float maxTilt = 10;

	private const float WHEEL_RADIUS = 0.8f;

	private Vector3 _accumPlayer1 = new Vector3();
	private Vector3 _accumPlayer2 = new Vector3();
	private float _player1Angle = 0;
	private float _player2Angle = 0;
	private float _wheelAngle = 0;
	private float _wheelSpeed = 0;
	private float _dirAngle = 90;
	private float _tilt = 0;

	private GameObjectReverter _player1WheelRev, _player2WheelRev;
	private Vector3 _startPos;

	public override void Reset ()
	{
		wheelMount.transform.localRotation = new Quaternion ();
		unicycle.transform.localPosition = _startPos;
		leftShoulder.localRotation = new Quaternion ();
		rightShoulder.localRotation = new Quaternion ();
		_player1WheelRev.Revert ();
		_player2WheelRev.Revert ();
		_accumPlayer1.Set (0, 0, 0);
		_accumPlayer2.Set (0, 0, 0);
		_wheelAngle = 0;
		_wheelSpeed = 0;
		_dirAngle = 90;
		_tilt = 0;
	}

	// Use this for initialization
	void Start () {
		_player1WheelRev = new GameObjectReverter (player1Wheel.gameObject);
		_player2WheelRev = new GameObjectReverter (player2Wheel.gameObject);
		_startPos = unicycle.transform.localPosition;

		Behavior p1Up    = new MovementCallbackBehavior("player1 move up",    this.gameObject, new Vector3( 0,  1, 0), MovePlayer1);
		Behavior p1Down  = new MovementCallbackBehavior("player1 move down",  this.gameObject, new Vector3( 0, -1, 0), MovePlayer1);
		Behavior p1Left  = new MovementCallbackBehavior("player1 move left",  this.gameObject, new Vector3(-1,  0, 0), MovePlayer1);
		Behavior p1Right = new MovementCallbackBehavior("player1 move right", this.gameObject, new Vector3( 1,  0, 0), MovePlayer1);
		Behavior p2Up    = new MovementCallbackBehavior("player2 move up",    this.gameObject, new Vector3( 0,  1, 0), MovePlayer2);
		Behavior p2Down  = new MovementCallbackBehavior("player2 move down",  this.gameObject, new Vector3( 0, -1, 0), MovePlayer2);
		Behavior p2Left  = new MovementCallbackBehavior("player2 move left",  this.gameObject, new Vector3(-1,  0, 0), MovePlayer2);
		Behavior p2Right = new MovementCallbackBehavior("player2 move right", this.gameObject, new Vector3( 1,  0, 0), MovePlayer2);
		
		// create the control scheme that maps inputs to these behaviors
		SetControlScheme(0, p1Up, p1Down, p1Left, p1Right);
		SetControlScheme(1, p2Up, p2Down, p2Left, p2Right);

		UnicycleDifficulty difficulty = ScenarioManager.Instance.GetDifficultyInfo() as UnicycleDifficulty;
		angleDeviation = difficulty.angleThreshold;
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log (other.tag);
		if (other.tag == "WinTrigger") {
			Victory();
		}
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
			float ret = Mathf.Atan2(vec.x, vec.y) * -(180 / Mathf.PI);
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

		_player1Angle = GetAngle(_accumPlayer1, _player1Angle);
		_player2Angle = GetAngle(_accumPlayer2, _player2Angle);

		float delta1 = GetAngleDelta (_wheelAngle, _player1Angle);
		float delta2 = GetAngleDelta (_wheelAngle, 180 - _player2Angle);

		bool player1 = _accumPlayer1.sqrMagnitude > 0.01 && delta1 < angleDeviation;
		bool player2 = _accumPlayer2.sqrMagnitude > 0.01 && delta2 < angleDeviation;

		_accumPlayer1.Set (0, 0, 0);
		_accumPlayer2.Set (0, 0, 0);

		float toAccel = player1 && player2 ? doubleaccel : player1 || player2 ? accel : -decel;
		float toTurn = player1 == player2 ? 0 : player1 ? -singleAngleOffset : singleAngleOffset;

		_dirAngle = (_dirAngle + toTurn * Time.fixedDeltaTime * _wheelSpeed / maxSpeed) % 360;
		_wheelSpeed += toAccel * Time.fixedDeltaTime;
		_wheelSpeed = Mathf.Clamp (_wheelSpeed, 0, maxSpeed);
		_wheelAngle = (_wheelAngle + _wheelSpeed * Time.fixedDeltaTime) % 360;
		float travel = _wheelSpeed * Time.fixedDeltaTime * Mathf.PI / 180 * WHEEL_RADIUS;

		Quaternion rotation = new Quaternion ();
		rotation.eulerAngles = new Vector3 (_wheelAngle + 180, 0, 0);
		wheelMount.localRotation = rotation;
		rotation.eulerAngles = new Vector3 (0, _dirAngle, 0);
		unicycle.rotation = rotation;
		rotation.eulerAngles = new Vector3 (0, 0, _wheelAngle);
		player1Wheel.rotation = rotation;
		rotation.eulerAngles = new Vector3 (0, 0, 180 - _wheelAngle);
		player2Wheel.rotation = rotation;
		unicycle.Translate (new Vector3 (0, 0, travel));

		if (player1 != player2)
		{
			if(player1)
			{
				_tilt += tiltSpeed + Time.fixedDeltaTime;
			}
			else
			{
				_tilt -= tiltSpeed + Time.fixedDeltaTime;
			}
			_tilt = Mathf.Clamp(_tilt, -maxTilt, maxTilt);
		}
		else if(-tiltSpeed < _tilt && _tilt < tiltSpeed)
		{
			_tilt = 0;
		}
		else if(_tilt < 0)
		{
			_tilt += tiltSpeed + Time.fixedDeltaTime;
		}
		else
		{
			_tilt -= tiltSpeed + Time.fixedDeltaTime;
		}

		rotation.eulerAngles = new Vector3 (0, 0, _tilt);
		leftShoulder.localRotation = rotation;
		rotation.eulerAngles = new Vector3 (0, 0, _tilt);
		rightShoulder.localRotation = rotation;
	}
}
