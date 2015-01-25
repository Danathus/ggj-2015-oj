using UnityEngine;
using System.Collections;
using System.Linq;

public class FroggerMan : Scenario {

	public float speed = 4.0f;

	private bool _enabled = true;

	public override void Reset ()
	{
	}

	// Use this for initialization
	void Start () {
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.W), new MovementCallbackBehavior("player1 move up",    this.gameObject, new Vector3( 0,  0, -1) * Time.fixedDeltaTime, Move));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.S), new MovementCallbackBehavior("player1 move down",  this.gameObject, new Vector3( 0,  0,  1) * Time.fixedDeltaTime, Move));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.A), new MovementCallbackBehavior("player1 move left",  this.gameObject, new Vector3( 1,  0,  0) * Time.fixedDeltaTime, Move));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.D), new MovementCallbackBehavior("player1 move right", this.gameObject, new Vector3(-1,  0,  0) * Time.fixedDeltaTime, Move));

		foreach (var body in this.GetComponentsInChildren<Rigidbody>()) {
			body.isKinematic = true;
			body.collider.enabled = false;
		}
		this.collider.enabled = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		ScenarioUpdate();
		
		mControls.Update ();
	}

	void OnTriggerEnter(Collider other) {
		if (_enabled && other.name.StartsWith ("Car")) {
			foreach (var body in this.GetComponentsInChildren<Rigidbody>()) {
				body.isKinematic = false;
				body.collider.enabled = true;
			}	
			_enabled = false;
		}
	}

	private void Move(GameObject gameObject, Vector3 offset) {
		if (_enabled) {
			gameObject.transform.position += offset * speed;
		}
	}
}
