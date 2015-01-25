using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Box : Scenario {
	public Rigidbody cereal;
	public Transform generationPoint1, generationPoint2;
	public float speed = 5.0f, rotateSpeed = 2.5f, minSpawnOffset = 0.0f, maxSpawnOffset = 0.2f, spawnPerSecond = 10.0f;

	private float _spawnTimeAccum = 0.0f;

	private static Stack<GameObject> _freeCereal = new Stack<GameObject>();

	private GameObjectReverter _reverter;
	private System.Random _random = new System.Random(12345);
	private int _randomSeed;

	public override void Reset() {
		foreach (Cereal cereal in Object.FindObjectsOfType<Cereal>()) {
			Destroy(cereal.gameObject);
		}
		_freeCereal.Clear ();

		_reverter.Revert ();
		Random.seed = _randomSeed;
		_random = new System.Random (12345);
	}

	void Start() {
		_reverter = new GameObjectReverter (this.gameObject);
		//_randomSeed = _random.;	

		Behavior p2Up    = new MovementCallbackBehavior("player1 move up",    this.gameObject, new Vector3( 0,  0, -1) * speed * Time.fixedDeltaTime, Move);
		Behavior p2Down  = new MovementCallbackBehavior("player1 move down",  this.gameObject, new Vector3( 0,  0,  1) * speed * Time.fixedDeltaTime, Move);
		Behavior p2Left  = new MovementCallbackBehavior("player1 move left",  this.gameObject, new Vector3( 1,  0,  0) * speed * Time.fixedDeltaTime, Move);
		Behavior p2Right = new MovementCallbackBehavior("player1 move right", this.gameObject, new Vector3(-1,  0,  0) * speed * Time.fixedDeltaTime, Move);
		
		// rig control scheme
		//   for buttons
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.UpArrow),    p2Up   );
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.DownArrow),  p2Down );
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.LeftArrow),  p2Left );
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.RightArrow), p2Right);
		//   for gamepads
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y,  1.0f), p2Up    );
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y, -1.0f), p2Down  );
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X, -1.0f), p2Left  );
		mControls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X,  1.0f), p2Right );
	}

	void Move(GameObject gameObject, Vector3 offset) {
		this.rigidbody.position -= new Vector3(offset.x, 0, 0);
		this.rigidbody.rotation *= new Quaternion(Mathf.Sin(offset.z), 0, 0, Mathf.Cos(offset.z));
		
	}

	// Update is called once per frame
	void FixedUpdate() {
		ScenarioUpdate();

		/*float x = 0, y = 0;

		if (Input.GetKey(KeyCode.LeftArrow)) {
			x -= 1;
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			x += 1;
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			y += 1;
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			y -= 1;
		}

		x *= speed * Time.fixedDeltaTime;
		y *= speed * Time.fixedDeltaTime;*/

		var up = this.rigidbody.rotation * new Vector3(0, 1, 0);
		var tiltAngle = Mathf.Atan2 (up.x, up.y) * (180 / Mathf.PI);
		if (Mathf.Abs(tiltAngle) > 90 && spawnPerSecond != 0) {
			_spawnTimeAccum += Time.fixedDeltaTime;

			int spawnCount = (int)Mathf.Floor(_spawnTimeAccum * spawnPerSecond);
			_spawnTimeAccum -= spawnCount / spawnPerSecond;

			if(spawnCount > 0)
			{
				var left = Vector3.Cross(up, new Vector3(0, 0, 1)).normalized;
				var forward = Vector3.Cross(left, up);
				for(int i = 0; i < spawnCount; i ++)
				{
					var offsetAngle = Random.Range(0, 2 * Mathf.PI);
					var offsetDistance = Random.Range(minSpawnOffset, maxSpawnOffset);
					var offset = (left * Mathf.Sin(offsetAngle) + forward * Mathf.Cos(offsetAngle)) * offsetDistance;

					if(tiltAngle > 0) {
						CreateCereal(generationPoint1.position + offset);
					} else {
						CreateCereal(generationPoint2.position + offset);
					}
				}
			}
		}
		else
		{
			_spawnTimeAccum = 0;
		}
	}

	// Use this for initialization
	void CreateCereal(Vector3 position)
	{
		if (_freeCereal.Count > 0) {
			GameObject gameObject = _freeCereal.Pop();
			gameObject.transform.position = gameObject.rigidbody.position = position;
			gameObject.rigidbody.velocity = gameObject.rigidbody.angularVelocity = new Vector3();
			gameObject.SetActiveRecursively(true);
		} else {
			Instantiate(cereal, position, new Quaternion());
		}
	}
	
	public static void DestroyCereal(GameObject gameObject) {
		gameObject.SetActiveRecursively(false);
		_freeCereal.Push(gameObject);
	}
	
}
