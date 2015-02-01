using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {

	public float speed = 50.0f;
	public float lifeTime = 10;

	private float _timeAlive = 0.0f;

	private FroggerManDifficulty _difficulty;
	
	// Use this for initialization
	void Start () {
		_difficulty = ScenarioManager.Instance.GetDifficultyInfo () as FroggerManDifficulty;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_timeAlive += Time.fixedDeltaTime;
		if (_timeAlive > 110 / _difficulty.carSpeed) {
			Destroy(this.gameObject);
			return;
		}
		this.transform.position += this.transform.TransformDirection(new Vector3(0, 0, 1)) * _difficulty.carSpeed * Time.fixedDeltaTime;
	}
}
