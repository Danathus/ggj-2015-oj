using UnityEngine;
using System.Collections;

public class Bowl : Scenario {

	public float speed = 150.0f;

	private int _totalCaught = 0;
	private float _playTime = 0.0f;
	private bool _hasEnded = false;

	public override void Reset ()
	{
	}

	public void CaughtCereal()
	{
		_totalCaught ++;
	}

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

		if (!_hasEnded) {
			_playTime += Time.fixedDeltaTime;
			if(_totalCaught == 100)
			{
				this.Victory();
				_hasEnded = true;
				Debug.Log ("Victory");
			}
			else if (_playTime > 15) {
				this.Failure();
				_hasEnded = true;
				Debug.Log ("Failure");
			}
		}

		//this.rigidbody.position += new Vector3(x, 0, y);
		this.rigidbody.velocity = new Vector3 (x, 0, y) * -Time.fixedDeltaTime;
		// Debug.Log (Time.fixeadDeltaTime);
		//Debug.Log (this.rigidbody.velocity);
	}
}
