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
		// create behaviors
		Behavior p1Up    = new MovementCallbackBehavior("player1 move up",    this.gameObject, new Vector3( 0,  0, -1) * Time.fixedDeltaTime, Move);
		Behavior p1Down  = new MovementCallbackBehavior("player1 move down",  this.gameObject, new Vector3( 0,  0,  1) * Time.fixedDeltaTime, Move);
		Behavior p1Left  = new MovementCallbackBehavior("player1 move left",  this.gameObject, new Vector3( 1,  0,  0) * Time.fixedDeltaTime, Move);
		Behavior p1Right = new MovementCallbackBehavior("player1 move right", this.gameObject, new Vector3(-1,  0,  0) * Time.fixedDeltaTime, Move);

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
