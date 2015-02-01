using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Box : Scenario {
	public Rigidbody cereal;
	public Transform generationPoint1, generationPoint2;
	public float speed = 5.0f, rotateSpeed = 2.5f, minSpawnOffset = 0.0f, maxSpawnOffset = 0.2f, spawnPerSecond = 10.0f;
	public Renderer bounds;

	private float _spawnTimeAccum = 0.0f;

	private Vector3 _windSpeed;

	private static Stack<GameObject> _freeCereal = new Stack<GameObject>();

	private GameObjectReverter _reverter;
	private RandomReverter _random;
	private Bounder _bounder;

	private CerealDifficulty _difficulty;

	public override void Reset() {
		Debug.Log("Box.Reset()!");
		foreach (Cereal cereal in Object.FindObjectsOfType<Cereal>()) {
			Destroy(cereal.gameObject);
		}
		_freeCereal.Clear ();

		_reverter.Revert ();
		_random.Revert ();
		_spawnTimeAccum = 0.0f;

		_difficulty = ScenarioManager.Instance.GetDifficultyInfo() as CerealDifficulty;
	}

	void Start() {
		_reverter = new GameObjectReverter(this.gameObject);
		_random = new RandomReverter();
		_bounder = new Bounder(bounds);

		Behavior p2Wind  = new DynamicWindBehavior("player2 wind", 			  this.gameObject, WindChanged);
		Behavior p2Up    = new MovementCallbackBehavior("player2 move up",    this.gameObject, new Vector3( 0,  0, -1) * speed * Time.fixedDeltaTime, Move);
		Behavior p2Down  = new MovementCallbackBehavior("player2 move down",  this.gameObject, new Vector3( 0,  0,  1) * speed * Time.fixedDeltaTime, Move);
		Behavior p2Left  = new MovementCallbackBehavior("player2 move left",  this.gameObject, new Vector3( 1,  0,  0) * speed * Time.fixedDeltaTime, Move);
		Behavior p2Right = new MovementCallbackBehavior("player2 move right", this.gameObject, new Vector3(-1,  0,  0) * speed * Time.fixedDeltaTime, Move);
		
		// rig control scheme
		mControls.AddControl(new TrueSignal(), 							   p2Wind );
		//   for buttons		

		SetControlScheme(1, p2Up, p2Down, p2Left, p2Right);

		Reset();
	}

	void Move(GameObject gameObject, Vector3 offset) {
		float x = _bounder.Translate(this.rigidbody.position, offset).x;
		this.rigidbody.position += new Vector3(x, 0, 0);
		this.rigidbody.rotation *= new Quaternion(Mathf.Sin(offset.z), 0, 0, Mathf.Cos(offset.z));
	}

	void WindChanged(GameObject operand, Vector3 wind) {
		_windSpeed = wind;
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
					var offsetAngle = _random.Range(0, 2 * Mathf.PI);
					var offsetDistance = _random.Range(minSpawnOffset, maxSpawnOffset);
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
			gameObject.rigidbody.velocity = gameObject.rigidbody.angularVelocity = _windSpeed * _difficulty.windSpeedMult;
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
