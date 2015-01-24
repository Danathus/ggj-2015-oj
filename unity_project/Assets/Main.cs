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

class ControlScheme
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

// BEHAVIOR ENTRY POINT ////////////////////////////////////////////////////////

public class Main : MonoBehaviour
{
	private GameObject mPlayer1, mPlayer2;
	private ControlScheme mControls;

	// Use this for initialization
	void Start () {
		// create mock scene with two player objects
		mPlayer1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		mPlayer1.transform.position = new Vector3(-1, 0, 0);
		mPlayer2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		mPlayer2.transform.position = new Vector3( 1, 0, 0);

		// create the behaviors in this scene
		Scenario scenario = new Scenario();
		float speed = 0.1f;
		scenario.AddBehavior(new TranslateBehavior("player1 move up",    mPlayer1, new Vector3( 0,  1, 0) * speed));
		scenario.AddBehavior(new TranslateBehavior("player1 move down",  mPlayer1, new Vector3( 0, -1, 0) * speed));
		scenario.AddBehavior(new TranslateBehavior("player1 move left",  mPlayer1, new Vector3(-1,  0, 0) * speed));
		scenario.AddBehavior(new TranslateBehavior("player1 move right", mPlayer1, new Vector3( 1,  0, 0) * speed));
		scenario.AddBehavior(new TranslateBehavior("player2 move up",    mPlayer2, new Vector3( 0,  1, 0) * speed));
		scenario.AddBehavior(new TranslateBehavior("player2 move down",  mPlayer2, new Vector3( 0, -1, 0) * speed));
		scenario.AddBehavior(new TranslateBehavior("player2 move left",  mPlayer2, new Vector3(-1,  0, 0) * speed));
		scenario.AddBehavior(new TranslateBehavior("player2 move right", mPlayer2, new Vector3( 1,  0, 0) * speed));

		// create the control scheme that maps inputs to these behaviors
		mControls = new ControlScheme();
		mControls.AddControl(KeyCode.W,          scenario.GetBehavior("player1 move up"));
		mControls.AddControl(KeyCode.S,          scenario.GetBehavior("player1 move down"));
		mControls.AddControl(KeyCode.A,          scenario.GetBehavior("player1 move left"));
		mControls.AddControl(KeyCode.D,          scenario.GetBehavior("player1 move right"));
		mControls.AddControl(KeyCode.UpArrow,    scenario.GetBehavior("player2 move up"));
		mControls.AddControl(KeyCode.DownArrow,  scenario.GetBehavior("player2 move down"));
		mControls.AddControl(KeyCode.LeftArrow,  scenario.GetBehavior("player2 move left"));
		mControls.AddControl(KeyCode.RightArrow, scenario.GetBehavior("player2 move right"));
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	// called once per timestep update (critical: do game state updates here!!!)
	void FixedUpdate()
	{
		// update all the controls
		mControls.Update();
	}

	// helper functions
}

// END OF FILE /////////////////////////////////////////////////////////////////
