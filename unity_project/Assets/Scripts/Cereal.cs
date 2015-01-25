using UnityEngine;
using System.Collections;

public class Cereal : MonoBehaviour {
	public float lifeTime = 10;
	public float zBottom = -20;

	private Renderer _bowlRenderer;
	private float _timeAlive = 0.0f, _timeInBowl = 0.0f;
	private bool _inBowl = false;

	// Use this for initialization
	void Start () {
		var newRotation = new Quaternion ();
		newRotation.eulerAngles = new Vector3(Random.Range (0, 180), Random.Range (0, 180), Random.Range (0, 180));
		this.transform.rotation = newRotation;
		_bowlRenderer = GameObject.FindGameObjectWithTag("Bowl").renderer;
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		_timeAlive += Time.fixedDeltaTime;
		if (_timeAlive > lifeTime || this.transform.position.y < zBottom) {
			//Destroy(this.gameObject);
			_timeAlive = 0.0f;
			_inBowl = false;


			Box.DestroyCereal(this.gameObject);
			return;
		} else {
			_inBowl = _bowlRenderer.bounds.Contains(this.transform.position);
		}

		if (_inBowl) {
			_timeInBowl += Time.fixedDeltaTime;
			if(_timeInBowl > 1.0f)
			{
				Box.DestroyCereal(this.gameObject);
				GetBowl().CaughtCereal();
				return;
			}
		}
	}

	Bowl GetBowl() {
		return _bowlRenderer.GetComponent<Bowl> ();
	}
}
