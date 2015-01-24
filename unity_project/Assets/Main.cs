using UnityEngine;
using System.Collections;

// PLAYER CONTROL SCHEME ///////////////////////////////////////////////////////

class ControlScheme
{
	GameObject mGameObject;
	KeyCode mUp, mDown, mLeft, mRight;

	public ControlScheme(GameObject gameObj, KeyCode up, KeyCode down, KeyCode left, KeyCode right)
	{
		mGameObject = gameObj;
		mUp = up;
		mDown = down;
		mLeft = left;
		mRight = right;
	}

	public void Update()
	{
		int up = Input.GetKey(mUp) ? 1 : 0;
		int down = Input.GetKey(mDown) ? 1 : 0;
		int left = Input.GetKey(mLeft) ? 1 : 0;
		int right = Input.GetKey(mRight) ? 1 : 0;
		Vector3 delta = new Vector3(right - left, up - down, 0);
		mGameObject.transform.position = mGameObject.transform.position + delta;
	}
}

// BEHAVIOR ENTRY POINT ////////////////////////////////////////////////////////

public class Main : MonoBehaviour
{
	private GameObject mPlayer1, mPlayer2;
	ControlScheme mPlayer1Control, mPlayer2Control;

	// Use this for initialization
	void Start () {
		// create mock scene with two player objects
		mPlayer1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		mPlayer1.transform.position = new Vector3(-1, 0, 0);
		mPlayer2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		mPlayer2.transform.position = new Vector3( 1, 0, 0);

		mPlayer1Control = new ControlScheme(mPlayer1, KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
		mPlayer2Control = new ControlScheme(mPlayer2, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
	}
	
	// Update is called once per frame
	void Update () {
		mPlayer1Control.Update();
		mPlayer2Control.Update();
	}

	// helper functions
}

// END OF FILE /////////////////////////////////////////////////////////////////
