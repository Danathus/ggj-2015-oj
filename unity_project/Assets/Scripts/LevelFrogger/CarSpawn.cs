using UnityEngine;
using System.Collections;

public class CarSpawn : Scenario {

	public Object car;
	public bool reversed = false;
	public float spawnTimeMin = 1, spawnTimeMax = 4;

	private float _timeToNextSpawn;
	private RandomReverter _random;

	public override void Reset()
	{
		_random.Revert();
		SetNextSpawn();
	}

	// Use this for initialization
	void Start() {
		_random = new RandomReverter ();
		Reset();
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		_timeToNextSpawn -= Time.fixedDeltaTime;
		if (_timeToNextSpawn < 0) {
			Spawn();
		}
	}

	void Spawn() {
		Quaternion rotation = new Quaternion ();
		rotation.eulerAngles = new Vector3 (0, reversed ? 270 : 90, 0);
		Instantiate(car, this.transform.position, rotation);
		SetNextSpawn();
	}
	
	void SetNextSpawn() {
		_timeToNextSpawn = _random.Range(spawnTimeMin, spawnTimeMax);
	}
}
