using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// PLAYER CONTROL SCHEME ///////////////////////////////////////////////////////

// a behavior is a thing a player can do in the scenario
//   does not define how this behavior is controlled
public abstract class Behavior
{
	public string mName;
	protected GameObject mOperand;

	public Behavior() {}
	public Behavior(string name, GameObject operand)
	{
		mName = name;
		mOperand = operand;
	}

	public abstract void Operate();
}

public class TranslateBehavior : Behavior
{
	Vector3 mOffset;
	public TranslateBehavior(string name, GameObject operand, Vector3 offset)
		: base(name, operand)
	{
		mOffset = offset;
	}

	public override void Operate()
	{
		mOperand.transform.position += mOffset;
	}
}

// a scenario includes an ordered set of behaviors that can be done
class Scenario
{
	List<Behavior> mBehaviors = new List<Behavior>();
	Dictionary<string, Behavior> mNameToBehavior = new Dictionary<string, Behavior>();

	public void AddBehavior(Behavior behavior)
	{
		mBehaviors.Add(behavior);
		mNameToBehavior[behavior.mName] = behavior;
	}
	public Behavior GetBehavior(string name)
	{
		return mNameToBehavior[name];
	}
}

class NewControlScheme
{
	List< KeyValuePair<KeyCode, Behavior> > mBehaviors = new List< KeyValuePair<KeyCode, Behavior> >();

	public void AddControl(KeyCode control, Behavior behavior)
	{
		mBehaviors.Add(new KeyValuePair<KeyCode, Behavior>(control, behavior));
	}

	public void Update()
	{
		foreach (KeyValuePair<KeyCode, Behavior> key_behavior in mBehaviors)
		{
			if (Input.GetKey(key_behavior.Key))
			{
				key_behavior.Value.Operate();
			}
		}
	}
}

// a control scheme has a mapping of controls to behaviors
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
		float speed = 0.1f;
		mGameObject.transform.position = mGameObject.transform.position + delta * speed;
	}
}

// BEHAVIOR ENTRY POINT ////////////////////////////////////////////////////////

public class Main : MonoBehaviour
{
	private GameObject mPlayer1, mPlayer2;
	ControlScheme mPlayer1Control, mPlayer2Control;
	NewControlScheme mNewControls;

	// Use this for initialization
	void Start () {
		// create mock scene with two player objects
		mPlayer1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		mPlayer1.transform.position = new Vector3(-1, 0, 0);
		mPlayer2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		mPlayer2.transform.position = new Vector3( 1, 0, 0);

		// create the behaviors in this scene
		Scenario scenario = new Scenario();
		//Behavior b = new TranslateBehavior("player1 move up",    mPlayer1, new Vector3( 0,  1, 0));
		//scenario.AddBehavior(b);
		scenario.AddBehavior(new TranslateBehavior("player1 move up",    mPlayer1, new Vector3( 0,  1, 0)));
		scenario.AddBehavior(new TranslateBehavior("player1 move down",  mPlayer1, new Vector3( 0, -1, 0)));
		scenario.AddBehavior(new TranslateBehavior("player1 move left",  mPlayer1, new Vector3(-1,  0, 0)));
		scenario.AddBehavior(new TranslateBehavior("player1 move right", mPlayer1, new Vector3( 1,  0, 0)));
		scenario.AddBehavior(new TranslateBehavior("player2 move up",    mPlayer2, new Vector3( 0,  1, 0)));
		scenario.AddBehavior(new TranslateBehavior("player2 move down",  mPlayer2, new Vector3( 0, -1, 0)));
		scenario.AddBehavior(new TranslateBehavior("player2 move left",  mPlayer2, new Vector3(-1,  0, 0)));
		scenario.AddBehavior(new TranslateBehavior("player2 move right", mPlayer2, new Vector3( 1,  0, 0)));

		// create the control scheme that maps inputs to these behaviors
		mNewControls = new NewControlScheme();
		mNewControls.AddControl(KeyCode.W,          scenario.GetBehavior("player1 move up"));
		mNewControls.AddControl(KeyCode.S,          scenario.GetBehavior("player1 move down"));
		mNewControls.AddControl(KeyCode.A,          scenario.GetBehavior("player1 move left"));
		mNewControls.AddControl(KeyCode.D,          scenario.GetBehavior("player1 move right"));
		mNewControls.AddControl(KeyCode.UpArrow,    scenario.GetBehavior("player2 move up"));
		mNewControls.AddControl(KeyCode.DownArrow,  scenario.GetBehavior("player2 move down"));
		mNewControls.AddControl(KeyCode.LeftArrow,  scenario.GetBehavior("player2 move left"));
		mNewControls.AddControl(KeyCode.RightArrow, scenario.GetBehavior("player2 move right"));

		//mPlayer1Control = new ControlScheme(mPlayer1, KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
		//mPlayer2Control = new ControlScheme(mPlayer2, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	// called once per timestep update (critical: do game state updates here!!!)
	void FixedUpdate()
	{
		//mPlayer1Control.Update();
		//mPlayer2Control.Update();

		mNewControls.Update();
	}

	// helper functions
}

// END OF FILE /////////////////////////////////////////////////////////////////
