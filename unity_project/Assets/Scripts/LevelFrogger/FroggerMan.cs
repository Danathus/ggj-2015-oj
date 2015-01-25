using UnityEngine;
using System.Collections;
using System.Linq;

public class FroggerMan : Scenario {

	public float speed = 4.0f;

	private Animator _animator;
	private bool _enabled = true;
	private Vector3 _movement = new Vector3();

	public override void Reset ()
	{
	}

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();

		mControls.AddControl(new KeyCodeControlSignal(KeyCode.W), new MovementCallbackBehavior("player1 move up",    this.gameObject, new Vector3( 0,  0, -1), Move));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.S), new MovementCallbackBehavior("player1 move down",  this.gameObject, new Vector3( 0,  0,  1), Move));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.A), new MovementCallbackBehavior("player1 move left",  this.gameObject, new Vector3( 1,  0,  0), Move));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.D), new MovementCallbackBehavior("player1 move right", this.gameObject, new Vector3(-1,  0,  0), Move));

		foreach (var body in this.GetComponentsInChildren<Rigidbody>()) {
			body.isKinematic = true;
			body.collider.enabled = false;
		}
		this.collider.enabled = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		mControls.Update ();


		if (_movement.sqrMagnitude > 0.0) {
			//this.transform.LookAt(this.transform.position + _movement);
		}

		//this.animation.Play ("walking");


		if(_movement.z < 0) this.transform.position += new Vector3 (0, 0, _movement.z * Time.fixedDeltaTime);
		_animator.SetFloat ("Horizontal", -_movement.x);
		_animator.SetFloat ("Vertical", -_movement.z);
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
