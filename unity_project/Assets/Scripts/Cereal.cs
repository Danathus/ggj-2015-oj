using UnityEngine;
using System.Collections;

public class Cereal : MonoBehaviour {
	public float lifeTime = 10;
	public float zBottom = -20;

	private Renderer _bowlRenderer;
	private float _timeAlive = 0.0f;
	private bool _inBowl = false;

	static int _num = 0, _den = 0;

	// Use this for initialization
	void Start () {
		var newRotation = new Quaternion ();
		newRotation.eulerAngles = new Vector3(Random.Range (0, 180), Random.Range (0, 180), Random.Range (0, 180));
		this.transform.rotation = newRotation;
		_bowlRenderer = GameObject.Find("Bowl").renderer;
		_den ++;
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		if (_inBowl) _num --;

		_timeAlive += Time.fixedDeltaTime;
		if (_timeAlive > lifeTime || this.transform.position.y < zBottom) {
			//Destroy(this.gameObject);
			_timeAlive = 0.0f;
			_inBowl = false;

			Box.DestroyCereal(this.gameObject);
			_den --;
		} else {
			_inBowl = _bowlRenderer.bounds.Contains(this.transform.position);
			if (_inBowl) _num ++;
			//Debug.Log ((float)_num / _den);
			//Debug.Log(_bowlRenderer.transform.position);
			//Debug.Log(_bowlRenderer.bounds.center);
		}
	}
}
