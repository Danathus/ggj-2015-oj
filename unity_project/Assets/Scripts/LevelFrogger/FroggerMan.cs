using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class FroggerMan : Scenario {

	public float speed = 4.0f;

	private Animator _animator;
	private bool _enabled = true;
	private Vector3 _movement = new Vector3();

	float _horizontalWeight = 0.0f;
	float _VerticalWeight = 0.0f;
	int _randomSeed;

	public override void Reset ()
	{
		//UnityEngine.Random.seed = _randomSeed;
		foreach (Car car in UnityEngine.Object.FindObjectsOfType<Car>()) {
			Destroy(car.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		_randomSeed = UnityEngine.Random.seed;

		_animator = GetComponent<Animator>();

		// create behaviors
		Behavior p1Up    = new MovementCallbackBehavior("player1 move up",    this.gameObject, new Vector3( 0,  0, -1), Move);
		Behavior p1Down  = new MovementCallbackBehavior("player1 move down",  this.gameObject, new Vector3( 0,  0,  1), Move);
		Behavior p1Left  = new MovementCallbackBehavior("player1 move left",  this.gameObject, new Vector3( 1,  0,  0), Move);
		Behavior p1Right = new MovementCallbackBehavior("player1 move right", this.gameObject, new Vector3(-1,  0,  0), Move);

		// rig control scheme
		//   for buttons
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.W), p1Up   );
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.S), p1Down );
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.A), p1Left );
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.D), p1Right);
		//   for gamepads
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y,  1.0f), p1Up    );
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y, -1.0f), p1Down  );
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X, -1.0f), p1Left  );
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X,  1.0f), p1Right );

		foreach (var body in this.GetComponentsInChildren<Rigidbody>()) {
			body.isKinematic = true;
			body.collider.enabled = false;
		}
		this.collider.enabled = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		ScenarioUpdate();

		if (_movement.sqrMagnitude > 0.0) {
			//this.transform.LookAt(this.transform.position + _movement);
		}

		//this.animation.Play ("walking");

		float w      = 0.1f;
		float targetX = -_movement.x * (1.0f / speed);
		float targetZ = Mathf.Min(_movement.z, 0.0f) * (1.0f / speed);
		_horizontalWeight = _horizontalWeight * (1.0f - w) + targetX * w;
		_VerticalWeight   = _VerticalWeight   * (1.0f - w) + targetZ * w;

		if (_movement.z < -0.0f)
		{
			this.transform.position += new Vector3 (0, 0, -_VerticalWeight * _movement.z * Time.fixedDeltaTime);
		}

		_animator.SetFloat ("Horizontal", _horizontalWeight);
		_animator.SetFloat ("Vertical",   -_VerticalWeight);
		_animator.SetFloat ("Turn", 0);
		_animator.SetBool ("Jump", false);
		_movement.Set (0, 0, 0);
	}

	void OnTriggerEnter(Collider other) {
		if (_enabled && other.name.StartsWith ("Car")) {
			foreach (var body in this.GetComponentsInChildren<Rigidbody>()) {
				body.isKinematic = false;
				body.collider.enabled = true;
			}	
			_animator.enabled = false;
			_enabled = false;
		}
	}

	private void Move(GameObject gameObject, Vector3 offset) {
		if (_enabled) {
			_movement += offset * speed;
		}
	}
}
