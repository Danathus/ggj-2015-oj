using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {

	public float speed = 50.0f;
	public float lifeTime = 10;

	private float _timeAlive = 0.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_timeAlive += Time.fixedDeltaTime;
		if (_timeAlive > lifeTime) {
			Destroy(this.gameObject);
			return;
		}

		this.transform.position += this.transform.TransformDirection(new Vector3(0, 0, 1)) * speed * Time.fixedDeltaTime;
	}
}
