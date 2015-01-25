using UnityEngine;
using System.Collections;

public class FroggerCamera : Scenario {

	public Transform cameraTarget;
	public float targetDist = 4;
	public float targetY = 2.5f;
	public float speed = 12;

	public override void Reset()
	{
	}

	// Use this for initialization
	void Start() {
		Move(this.gameObject, new Vector3 ());

		mControls.AddControl(new KeyCodeControlSignal(KeyCode.LeftArrow), new MovementCallbackBehavior("player2 move left",  this.gameObject, new Vector3( 1,  0,  0) * Time.fixedDeltaTime, Move));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.RightArrow), new MovementCallbackBehavior("player2 move right", this.gameObject, new Vector3(-1,  0,  0) * Time.fixedDeltaTime, Move));
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		Debug.Log (Input.GetKey (KeyCode.LeftArrow));
		this.transform.LookAt(cameraTarget);

		Vector3 position = this.transform.position;
		Vector3 targetPos = cameraTarget.position;
		Vector3 dir = position - targetPos;
		dir.y = 0;
		position = targetPos + dir.normalized * targetDist;
		position.y = targetY;
		this.transform.position = position;

		mControls.Update ();
	}

	private void Move(GameObject gameObject, Vector3 offset) {
		Vector3 leftDir = gameObject.transform.TransformDirection(new Vector3(-1, 0, 0));
		gameObject.transform.position += leftDir * offset.x * speed;
	}
}
