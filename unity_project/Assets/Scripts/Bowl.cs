using UnityEngine;
using System.Collections;

public class Bowl : MonoBehaviour {

	public float speed = 150.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		float x = 0, y = 0;

		//this.rigidbody.velocity = new Vector3(0, 1, 0);

		if (Input.GetKey(KeyCode.A)) {
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
		}

		//this.rigidbody.position += new Vector3(x, 0, y);
		this.rigidbody.velocity = new Vector3 (x, y, 0) * -Time.fixedDeltaTime;
		// Debug.Log (Time.fixedDeltaTime);
		Debug.Log (this.rigidbody.velocity);
	}
}
