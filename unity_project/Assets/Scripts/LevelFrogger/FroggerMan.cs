using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class FroggerMan : Scenario {

	public float speed = 4.0f;

	private Animator _animator;
	private bool _enabled = true;
	private Vector3 _movement = new Vector3();
	public AudioClip _punch_sound;

	float _horizontalWeight = 0.0f;
	float _VerticalWeight = 0.0f;
	//int _randomSeed;

	GameObjectReverter _manReverter = null;

	protected void GoRagdoll()
	{
		foreach (var body in this.GetComponentsInChildren<Rigidbody>()) {
			body.isKinematic = false;
			body.collider.enabled = true;
		}
		_animator.enabled = false;
		_enabled = false;
	}

	protected void GoCannedAnimation()
	{
		foreach (var body in this.GetComponentsInChildren<Rigidbody>()) {
			body.isKinematic = true;
			body.collider.enabled = false;
		}
		//this.gameObject.rigidbody.isKinematic = false;
		_animator.enabled = true;
		_enabled = true;
	}

	public override void Reset()
	{
		//UnityEngine.Random.seed = _randomSeed;
		foreach (Car car in UnityEngine.Object.FindObjectsOfType<Car>()) {
			Destroy(car.gameObject);
		}

		if (_animator != null)
		{
			_animator.SetFloat ("Horizontal", 0.0f);
			_animator.SetFloat ("Vertical",   0.0f);
			_animator.SetFloat ("Turn", 0.0f);
			_animator.SetBool ("Jump", false);
		}
		this.gameObject.rigidbody.isKinematic = false;
		if (_manReverter != null)
		{
			_manReverter.Revert();
		}
		this.gameObject.rigidbody.isKinematic = true;

		GoCannedAnimation();
		this.collider.enabled = true;

		_movement = new Vector3();
		_horizontalWeight = 0.0f;
		_VerticalWeight = 0.0f;
	}

	// Use this for initialization
	void Start () {
		//_randomSeed = UnityEngine.Random.seed;

		_animator = GetComponent<Animator>();

		// create behaviors
		Behavior p1Up    = new MovementCallbackBehavior("player1 move up",    this.gameObject, new Vector3( 0,  0, -1), Move);
		Behavior p1Down  = new MovementCallbackBehavior("player1 move down",  this.gameObject, new Vector3( 0,  0,  1), Move);
		Behavior p1Left  = new MovementCallbackBehavior("player1 move left",  this.gameObject, new Vector3( 1,  0,  0), Move);
		Behavior p1Right = new MovementCallbackBehavior("player1 move right", this.gameObject, new Vector3(-1,  0,  0), Move);

		// rig control scheme
		//   for buttons
		SetControlScheme(0, p1Up, p1Down, p1Left, p1Right);

		_manReverter = null;
		Reset();
		_manReverter = new GameObjectReverter(this.gameObject);
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
			GoRagdoll();
			AudioSource audio = this.gameObject.AddComponent<AudioSource>();
			audio.clip = _punch_sound;
			audio.Play();
			Failure();
		}
	}

	private void Move(GameObject gameObject, Vector3 offset) {
		if (_enabled) {
			_movement += offset * speed;
		}
	}
}
