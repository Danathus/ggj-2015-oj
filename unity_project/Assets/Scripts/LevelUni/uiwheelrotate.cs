using UnityEngine;
using System.Collections;

public class uiwheelrotate : MonoBehaviour {
	public float speed = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		this.transform.Rotate(new Vector3 (0, 0, speed * Time.fixedDeltaTime));
	}
}
