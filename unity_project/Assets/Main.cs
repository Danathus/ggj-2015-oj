using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
	private GameObject mCube;

	// Use this for initialization
	void Start () {
		mCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		mCube.transform.position = new Vector3(0, 0.5F, 0);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("hellooo");
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
            mCube.transform.position = mCube.transform.position - new Vector3(0, 0.1F, 0);
		}
	}
}
