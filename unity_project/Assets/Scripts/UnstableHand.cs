using UnityEngine;
using System.Collections;
/// <summary>
/// Rshaking: http://forum.unity3d.com/threads/camera-shake-script-c.142764/

/// </summary>
public class UnstableHand : MonoBehaviour {

	private Vector3 originPosition;
	private Quaternion originRotation;
	
	private float shake_decay;
	private float shake_intensity;
	private float drunk_level = 2.0f;
	private float camera_range = 0.5f;
	private float diff_time;
	private float x_delta;
	private float y_delta;
	// Use this for initialization
	void Start () {
		Shake();
		///shake_intensity = .1f;
	}
	
	// Update is called once per frame
	void Update () {
		if(shake_intensity > 0){
			Vector3 min_moving_range = originPosition - new Vector3 (camera_range, camera_range, 0);
			Vector3 max_moving_range = originPosition + new Vector3 (camera_range, camera_range, 0);
			diff_time += Time.deltaTime;
			if(diff_time > Random.Range(3.0f, 5.0f)) {
				Debug.Log("change delta value, diff time: " + diff_time.ToString());
				x_delta = Random.Range(-drunk_level, drunk_level);
				y_delta = Random.Range(-drunk_level, drunk_level);
				diff_time = 0.0f;
			}
			transform.position = new Vector3 (
				Mathf.Min(Mathf.Max(transform.position.x + x_delta * shake_intensity, min_moving_range.x), max_moving_range.x),
				Mathf.Min(Mathf.Max(transform.position.y + y_delta * shake_intensity, min_moving_range.y), max_moving_range.y),
				transform.position.z);
			if(transform.position.x == min_moving_range.x || transform.position.x == max_moving_range.x ||
			   transform.position.y == min_moving_range.y || transform.position.y == max_moving_range.y){
				x_delta = Random.Range(-drunk_level, drunk_level);
				y_delta = Random.Range(-drunk_level, drunk_level);
			}
			Debug.Log("delta time: " + Time.deltaTime.ToString());
			//Debug.Log("x_delt: " + x_delta.ToString());
			//Debug.Log("new position: " + transform.position.ToString());
			//transform.rotation = new Quaternion(
			//	originRotation.x + Random.Range(-shake_intensity,shake_intensity)*.2f,
			//	originRotation.y + Random.Range(-shake_intensity,shake_intensity)*.2f,
			//	originRotation.z,
			//	originRotation.w);
			///shake_intensity -= shake_decay;
		}
		///transform.Translate(Vector3.forward * Time.deltaTime);
	}
	private void Shake(){
		originPosition = transform.position;
		originRotation = transform.rotation;
		shake_intensity = 0.001f;
		shake_decay = 0.002f;
		diff_time = 0.0f;
		x_delta = Random.Range(-drunk_level, drunk_level);
		y_delta = Random.Range(-drunk_level, drunk_level);
	}
}
