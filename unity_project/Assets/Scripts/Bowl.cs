using UnityEngine;
using System.Collections;

public class Bowl : Scenario {

	public float speed = 150.0f;

	private int _totalCaught = 0;
	//private float _playTime = 0.0f;
	private bool _hasEnded = false;
	private Vector3 _accum = new Vector3();
	public Renderer bounds;

	private GameObjectReverter _reverter;
	private Bounder _bounder;

	public override void Reset ()
	{
		Debug.Log("Bowl.Reset()!");
		_reverter.Revert ();
		_totalCaught = 0;
		_accum = new Vector3();
	}

	public void CaughtCereal()
	{
		_totalCaught ++;
	}

	// Use this for initialization
	void Start () {
		_reverter = new GameObjectReverter (this.gameObject);
		_bounder = new Bounder (bounds);

		Behavior p1Up    = new MovementCallbackBehavior("player1 move up",    this.gameObject, new Vector3( 0,  1, 0) * speed, Move);
		Behavior p1Down  = new MovementCallbackBehavior("player1 move down",  this.gameObject, new Vector3( 0, -1, 0) * speed, Move);
		Behavior p1Left  = new MovementCallbackBehavior("player1 move left",  this.gameObject, new Vector3(-1,  0, 0) * speed, Move);
		Behavior p1Right = new MovementCallbackBehavior("player1 move right", this.gameObject, new Vector3( 1,  0, 0) * speed, Move);

		// create the control scheme that maps inputs to these behaviors
		SetControlScheme(0, p1Up, p1Down, p1Left, p1Right);
		Reset();
	}
	
	void Move(GameObject gameObject, Vector3 offset) {
		_accum += new Vector3 (offset.x, 0, offset.y) * -Time.fixedDeltaTime;
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		float x = 0, y = 0;

		//this.rigidbody.velocity = new Vector3(0, 1, 0);

		/*if (Input.GetKey(KeyCode.A)) {
			x -= speed;
		}
		if (Input.GetKey(KeyCode.D)) {
			x += speed;
		}
		if (Input.GetKey(KeyCode.W)) {
			y += speed;
		}
		if (Input.GetKey(KeyCode.S)) {
			y -= speed;
		}*/

		if (!_hasEnded) {
			//_playTime += Time.fixedDeltaTime;
			if(_totalCaught == 50)
			{
				this.Victory();
				_hasEnded = true;
			}
			else //if (_playTime > 15)
			{
				//this.Failure();
				//_hasEnded = true;
			}
		}

		//this.rigidbody.position += new Vector3(x, 0, y);
		// Debug.Log (Time.fixeadDeltaTime);
		//Debug.Log (this.rigidbody.velocity);

		this.rigidbody.velocity = _bounder.Translate(this.rigidbody.position, _accum);
		_accum.Set (0, 0, 0);
	}
}
